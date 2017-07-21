using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleStatusManager: IRPCDicObserver
{

	void Start ()
	{
		RPCDicObserver.AddObserver (this);
	}

	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		try {
			Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
			ReceiveBattleStatus (rpcReceive);
		} catch (System.Exception e) {
			//do something with exception
		}
	}

	private void OnAnswerCountFull ()
	{
		if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
			PhaseManager.StartPhase3 ();
		} else {
			PhaseManager.StartPhase2 ();
		}
	}

	private void OnSkillCountFull ()
	{
		if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
			PhaseManager.StartPhase2 ();
		} else {
			PhaseManager.StartPhase3 ();
		}
	}

	public void ReceiveBattleStatus (Dictionary<string, System.Object> battleStatusDetails)
	{
		Dictionary<string, System.Object> newBattleStatus = new Dictionary<string, object> ();
		List<Dictionary<string, System.Object>> newBattleStatusList = new List<Dictionary<string, object>> ();

		foreach (var item in battleStatusDetails) {
			if (UnityEngine.Object.ReferenceEquals (item.Value.GetType (), newBattleStatus.GetType ())) {
				newBattleStatusList.Add ((Dictionary<string, object>)item.Value);

			}
		}

		if (newBattleStatusList.Count > 0) {
			//get last value
			newBattleStatus = newBattleStatusList [newBattleStatusList.Count - 1];

			if (newBattleStatus.ContainsKey (MyConst.BATTLE_STATUS_STATE)) {
				string battleState = newBattleStatus [MyConst.BATTLE_STATUS_STATE].ToString ();
				int battleCount = int.Parse (newBattleStatus [MyConst.BATTLE_STATUS_COUNT].ToString ());

				Debug.Log ("Current Battle State: " + battleState);
				Debug.Log ("Current Battle Count: " + battleCount);

				switch (battleState) {
				case MyConst.BATTLE_STATUS_ANSWER:

					SystemGlobalDataController.Instance.hAnswer = int.Parse (newBattleStatus [MyConst.BATTLE_STATUS_HANSWER].ToString ());
					SystemGlobalDataController.Instance.hTime = int.Parse (newBattleStatus [MyConst.BATTLE_STATUS_HTIME].ToString ());
					SystemGlobalDataController.Instance.vAnswer = int.Parse (newBattleStatus [MyConst.BATTLE_STATUS_VANSWER].ToString ());
					SystemGlobalDataController.Instance.vTime = int.Parse (newBattleStatus [MyConst.BATTLE_STATUS_VTIME].ToString ());

					CheckBattleCount (battleCount, OnAnswerCountFull);

					break;

				case MyConst.BATTLE_STATUS_SKILL:
					CheckBattleCount (battleCount, OnSkillCountFull);
					break;

				case MyConst.BATTLE_STATUS_ATTACK:
					CheckBattleCount (battleCount);
					break;

				}
			}


		}

	

	}

	private void CheckBattleCount (int battleCount, Action action = null)
	{
		if (battleCount > 1) {
			action ();
			SystemLoadScreenController.Instance.StopWaitOpponentScreen ();
		}
	}
}

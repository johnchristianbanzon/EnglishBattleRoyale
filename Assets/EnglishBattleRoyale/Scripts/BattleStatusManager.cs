using System.Collections.Generic;
using UnityEngine;

public class BattleStatusManager : SingletonMonoBehaviour<BattleStatusManager>, IRPCDicObserver
{

	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		try {
			Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
			ReceiveBattleStatus (rpcReceive);
		} catch (System.Exception e) {
			//do something with exception
		}
	}

	public void ReceiveBattleStatus (Dictionary<string, System.Object> battleStatusDetails)
	{
		Dictionary<string, System.Object> newBattleStatus = new Dictionary<string, object> ();
		List<Dictionary<string, System.Object>> newBattleStatusList = new List<Dictionary<string, object>> ();

		foreach (var item in battleStatusDetails) {
			if (Object.ReferenceEquals (item.Value.GetType (), newBattleStatus.GetType ())) {
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

					GameData.Instance.hAnswer = int.Parse (newBattleStatus [MyConst.BATTLE_STATUS_HANSWER].ToString ());
					GameData.Instance.hTime = int.Parse (newBattleStatus [MyConst.BATTLE_STATUS_HTIME].ToString ());
					GameData.Instance.vAnswer = int.Parse (newBattleStatus [MyConst.BATTLE_STATUS_VANSWER].ToString ());
					GameData.Instance.vTime = int.Parse (newBattleStatus [MyConst.BATTLE_STATUS_VTIME].ToString ());


					if (battleCount > 1) {
						Debug.Log ("switching phases");
						if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
							PhaseManager.StartPhase3 ();
						} else {
							PhaseManager.StartPhase2 ();
						}
						SystemLoadScreenController.Instance.StopWaitOpponentScreen ();
					} 

					break;

				case MyConst.BATTLE_STATUS_SKILL:
					if (battleCount > 1) {
						Debug.Log ("switching phases");
						if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
							PhaseManager.StartPhase2 ();
						} else {
							PhaseManager.StartPhase3 ();
						}
						SystemLoadScreenController.Instance.StopWaitOpponentScreen ();
					}
					break;

				case MyConst.BATTLE_STATUS_ATTACK:
					if (battleCount > 1) {
						SystemLoadScreenController.Instance.StopWaitOpponentScreen ();
					}
					break;

				}
			}
		}

	}
}

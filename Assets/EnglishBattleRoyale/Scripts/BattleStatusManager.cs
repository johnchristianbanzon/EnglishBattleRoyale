using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleStatusManager: IRPCDicObserver
{

	public void Init ()
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

	private void ReceiveBattleStatus (Dictionary<string, System.Object> battleStatusDetails)
	{
		Dictionary<string, System.Object> newBattleStatus = new Dictionary<string, object> ();
		List<Dictionary<string, System.Object>> newBattleStatusList = new List<Dictionary<string, object>> ();

		//if each item is the same type of newbattlestatus, add to list
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
				switch (battleState) {


				case MyConst.BATTLE_STATUS_ANSWER:
					if (GameManager.isHost) {
						if (newBattleStatus [MyConst.RPC_DATA_PLAYER_ANSWER_PARAM].ToString () != "0") {
							GameManager.playerAnswerParam = JsonUtility.FromJson<QuestionResultCountModel> (newBattleStatus [MyConst.RPC_DATA_PLAYER_ANSWER_PARAM].ToString ());
						}

						if (newBattleStatus [MyConst.RPC_DATA_ENEMY_ANSWER_PARAM].ToString () != "0") {
							GameManager.enemyAnswerParam = JsonUtility.FromJson<QuestionResultCountModel> (newBattleStatus [MyConst.RPC_DATA_ENEMY_ANSWER_PARAM].ToString ());
						}
					} else {
						if (newBattleStatus [MyConst.RPC_DATA_ENEMY_ANSWER_PARAM].ToString () != "0") {
							GameManager.playerAnswerParam = JsonUtility.FromJson<QuestionResultCountModel> (newBattleStatus [MyConst.RPC_DATA_ENEMY_ANSWER_PARAM].ToString ());
						}

						if (newBattleStatus [MyConst.RPC_DATA_PLAYER_ANSWER_PARAM].ToString () != "0") {
							GameManager.enemyAnswerParam = JsonUtility.FromJson<QuestionResultCountModel> (newBattleStatus [MyConst.RPC_DATA_PLAYER_ANSWER_PARAM].ToString ());
						}
					}
						

					CheckBattleCount (battleCount, delegate() {
						if (GameManager.isHost) {
							SystemFirebaseDBController.Instance.UpdateBattleStatus (MyConst.BATTLE_STATUS_ATTACK, 0);
						}


					});
					break;

				case MyConst.BATTLE_STATUS_ATTACK:
					if (ScreenBattleController.Instance.GetIsPhase1 ()) {
						ScreenBattleController.Instance.StartPhase2 ();
					}

					CheckBattleCount (battleCount, delegate() {
						if (GameManager.isHost) {
							SystemFirebaseDBController.Instance.UpdateBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0, "0", "0");
						}
					});
					break;

				}
			}
		}
	}

	private void CheckBattleCount (int battleCount, Action action)
	{
		//Reminders: change to 1 if not testing
		if (battleCount > 1) {
			action ();
			SystemLoadScreenController.Instance.StopWaitOpponentScreen ();
		}
	}
}

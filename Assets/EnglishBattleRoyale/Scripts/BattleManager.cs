using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleManager: IRPCDicObserver
{
	private static int characterActionCounter = 0;
	private static AttackModel playerAttack;
	private static AttackModel enemyAttack;

	public void Init ()
	{
		RPCDicObserver.AddObserver (this);
	}

	//after confirming that both players have sent skill, send attack
	public static void CountCharacters ()
	{
		characterActionCounter++;
		Debug.Log ("CHARACTER SENT COUNT " + characterActionCounter);

		//Reminders: Change to 2 if not testing
		if (characterActionCounter == 2) {
			ScreenBattleController.Instance.partState.StartBattle ();
		}
	}

	public static void ClearBattleData ()
	{
		characterActionCounter = 0;
		playerAttack = null;
		enemyAttack = null;
	}

	#region Send Attack to Firebase

	//send attack to firebase
	public static void SendAttack ()
	{
		SystemFirebaseDBController.Instance.AttackPhase (new AttackModel (ScreenBattleController.Instance.partState.player.playerBaseDamage));
		Debug.Log ("SENDING ATTACK");
	}

	#endregion

	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		try {
			Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
			if (rpcReceive.ContainsKey (MyConst.RPC_DATA_PARAM)) {
				bool userHome = (bool)rpcReceive [MyConst.RPC_DATA_USERHOME];

				Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive [MyConst.RPC_DATA_PARAM];
				if (param.ContainsKey (MyConst.RPC_DATA_ATTACK)) {
					AttackModel attack = JsonUtility.FromJson<AttackModel> (param [MyConst.RPC_DATA_ATTACK].ToString ());

					if (userHome.Equals (GameManager.isHost)) {
						playerAttack = attack;
					} else {
						enemyAttack = attack;
					}
				}
			}
		} catch (System.Exception e) {
			//do something later
		}
	}

	public static bool CheckBothAttack ()
	{
		Debug.Log ("Checking both attack");
		if (playerAttack != null && enemyAttack != null) {
			return true;
		} else {
			return false;
		}
	}

	public static void ComputeBothAttack ()
	{
		ComputePlayerAttack ();
		ComputeEnemyAttack ();
	}

	public static bool CheckPlayerAttack ()
	{
		Debug.Log ("Checking player attack");
		if (playerAttack != null) {
			return true;
		} else {
			return false;
		}
	}

	public static void ComputePlayerAttack ()
	{
		BattleLogic.AttackCompute (true, playerAttack);
		playerAttack = null;
	}

	public static bool CheckEnemyAttack ()
	{
		Debug.Log ("Checking enemy attack");
		if (enemyAttack != null) {
			return true;
		} else {
			return false;
		}
	}

	public static void ComputeEnemyAttack ()
	{
		BattleLogic.AttackCompute (false, enemyAttack);
		enemyAttack = null;
	}
		

	public static int GetBattleOrder ()
	{
		int battleOrder = 0;
		QuestionResultCountModel playerAnswerParam = GameManager.playerAnswerParam;
		QuestionResultCountModel enemyAnswerParam = GameManager.enemyAnswerParam;

		//TO-DO: REFACTOR THIS CODE IF POSSIBLE
		if (playerAnswerParam.correctCount > enemyAnswerParam.correctCount) {
			battleOrder = 0;
		} else if (playerAnswerParam.correctCount < enemyAnswerParam.correctCount) {
			battleOrder = 1;
		} else {
			if (playerAnswerParam.speedyAwesomeCount > enemyAnswerParam.speedyAwesomeCount) {
				battleOrder = 0;
			} else if (playerAnswerParam.speedyAwesomeCount < enemyAnswerParam.speedyAwesomeCount) {
				battleOrder = 1;
			} else {
				if (playerAnswerParam.speedyGoodCount > enemyAnswerParam.speedyGoodCount) {
					battleOrder = 0;
				} else if (playerAnswerParam.speedyGoodCount < enemyAnswerParam.speedyGoodCount) {
					battleOrder = 1;
				} else {
					if (playerAnswerParam.speedyRottenCount > enemyAnswerParam.speedyRottenCount) {
						battleOrder = 0;
					} else if (playerAnswerParam.speedyRottenCount < enemyAnswerParam.speedyRottenCount) {
						battleOrder = 1;
					} else {
						battleOrder = 2;
					}
				}
			}
		}

		Debug.Log ("BATTLE ORDER IS" + battleOrder);
		return battleOrder;
	}



}

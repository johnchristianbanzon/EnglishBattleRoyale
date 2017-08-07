using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleManager: IRPCDicObserver
{
	private static Queue<Action> playerActionsQueue = new Queue<Action>();
	private static  Queue<Action> enemyActionsQueue = new Queue<Action>();
	private static int characterActionCounter = 0;
	private static int characterAttackCounter = 0;

	//set first player actions which is activating characters
	public static void SetPlayerActionQueue (Queue<Action> action)
	{
		playerActionsQueue = action;
		CheckActionList ();
	}

	//set first enemy actions which is activating characters
	public static void SetEnemyActionQueue (Queue<Action> action)
	{
		enemyActionsQueue = action;
		CheckActionList ();
	}

	//after confirming that both players have sent skill, send attack
	private static void CheckActionList ()
	{
		characterActionCounter++;
		if (characterActionCounter == 2) {
			SendAttack ();
			characterActionCounter = 0;
		}
	}

	private static void ClearQueues ()
	{
		if (playerActionsQueue.Count > 0) {
			playerActionsQueue.Clear ();
		}
		if (enemyActionsQueue.Count > 0) {
			enemyActionsQueue.Clear ();
		}
	}

	#region Send Attack to Firebase

	public void OnStartPhase ()
	{
		Debug.Log ("Starting attack phase");
		RPCDicObserver.AddObserver (this);

		//Check toggle on characters on start of the phase and send it
		ScreenBattleController.Instance.partCharacter.ActivateToggledCharacters ();

		ScreenBattleController.Instance.partCharacter.ShowAutoActivateButtons (false);
		PartAnswerIndicatorController.Instance.ResetAnswer ();
	}
		
	//send attack to firebase
	private static void SendAttack ()
	{
		Debug.Log ("Sending Attack to Firebase");
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [MyConst.RPC_DATA_ATTACK] = SystemGlobalDataController.Instance.player.playerBaseDamage;
		SystemFirebaseDBController.Instance.AttackPhase (new AttackModel (JsonUtility.ToJson (param)));
		Debug.Log ("Sending Attack Complete");
	}


	//show skill buttons after attack phase is done
	public void OnEndPhase ()
	{
		ClearQueues ();
		ScreenBattleController.Instance.partCharacter.ShowAutoActivateButtons (true);
		RPCDicObserver.RemoveObserver (this);
	}

	#endregion

	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		try {
			Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
			if (rpcReceive.ContainsKey (MyConst.RPC_DATA_PARAM)) {
				bool userHome = (bool)rpcReceive [MyConst.RPC_DATA_USERHOME];
				SystemGlobalDataController.Instance.isSender = userHome;

				Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive [MyConst.RPC_DATA_PARAM];
				AttackModel attack = JsonUtility.FromJson<AttackModel> (param [MyConst.RPC_DATA_ATTACK].ToString ());
				if (SystemGlobalDataController.Instance.isSender.Equals (SystemGlobalDataController.Instance.isHost)) {
					playerActionsQueue.Enqueue (delegate() {
						AttackLogic (true, attack);
					});
					CheckAttackList ();
				} else {
					enemyActionsQueue.Enqueue (delegate() {
						AttackLogic (false, attack);
					});
					CheckAttackList ();
				}
			}
		} catch (System.Exception e) {
			//do something later
		}
	}



	public static void AttackLogic (bool isPLayer, AttackModel attack)
	{
		
//		int attackSequence = 0;
//
//		if (attackerParam [MyConst.RPC_DATA_ATTACK] != null) {
//			int damage = int.Parse (attackerParam [MyConst.RPC_DATA_ATTACK].ToString ());
//
//			if (attackerBool.Equals (SystemGlobalDataController.Instance.isHost)) {
//				Debug.Log ("PLAYER DAMAGE: " + damage);
//				enemyHP -= damage;
//				if (sameAttack == false) {
//					attackSequence = 1;
//				}
//
//			} else {
//				Debug.Log ("ENEMY DAMAGE: " + damage);
//				playerHP -= damage;
//				if (sameAttack == false) {
//					attackSequence = 2;
//				}
//			}
//
//			hp (enemyHP, playerHP);
//		}
//
//		return attackSequence;
	}

	private static void CheckAttackList ()
	{
		characterAttackCounter++;
		if (characterAttackCounter == 2) {
			CheckOrder ();
			characterAttackCounter = 0;
			Debug.Log ("Starting battle logic");
		}


	}


	public static void CheckOrder ()
	{
//		//change order of list if host or visitor
//		if (SystemGlobalDataController.Instance.isHost) {
//			if (userHome [0] != SystemGlobalDataController.Instance.isHost) {
//				ChangeOrderReduce (0, 1, attackparam, userHome, param);
//			}
//		} else {
//			if (userHome [1] != SystemGlobalDataController.Instance.isHost) {
//				ChangeOrderReduce (1, 0, attackparam, userHome, param);
//			}
//	
//		}
	}



	//
	//	private void ChangeOrderReduce (int index0, int index1,Action <List<bool> , List<Dictionary<string, System.Object>>> attackparam, List<bool> userHome, List<Dictionary<string, System.Object>> param)
	//	{
	//		bool tempName = userHome [index0];
	//		Dictionary<string, System.Object> tempParam = param [index0];
	//
	//		userHome.Insert (index0, userHome [index1]);
	//		userHome.Insert (index1, tempName);
	//		param.Insert (index0, param [index1]);
	//		param.Insert (index1, tempParam);
	//
	//		attackparam (userHome, param);
	//	}
	//
	//	public int GetAttackOrder(){
	//		int attackOrder = 0;
	//		QuestionResultCountModel playerAnswerParam = SystemGlobalDataController.Instance.playerAnswerParam;
	//		QuestionResultCountModel enemyAnswerParam = SystemGlobalDataController.Instance.enemyAnswerParam;
	//
	//		if (playerAnswerParam.correctCount > enemyAnswerParam.correctCount) {
	//			attackOrder = 0;
	//		} else if (playerAnswerParam.correctCount < enemyAnswerParam.correctCount) {
	//			attackOrder = 1;
	//		} else {
	//			if (playerAnswerParam.speedyCount > enemyAnswerParam.speedyCount) {
	//				attackOrder = 0;
	//			} else if (playerAnswerParam.speedyCount < enemyAnswerParam.speedyCount) {
	//				attackOrder = 1;
	//			} else {
	//				attackOrder = 2;
	//			}
	//		}
	//		return attackOrder;
	//	}
	//
	//	public void CheckBattle(bool secondCheck,float enemyHP, float playerHP, Action<string, bool> battleResult){
	//
	//		if (enemyHP <= 0 || playerHP <= 0) {
	//			SystemLoadScreenController.Instance.StopWaitOpponentScreen ();
	//
	//			if (enemyHP > 0 && playerHP <= 0) {
	//				battleResult ("lose", true);
	//
	//			} else if (playerHP > 0 && enemyHP <= 0) {
	//				battleResult ("win", true);
	//
	//			} else {
	//				battleResult ("draw", true);
	//
	//			}
	//
	//		} else {
	//			battleResult ("", false);
	//		}
	//
	//	}
	//
	//

}

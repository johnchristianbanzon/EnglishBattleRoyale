using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleManager: IRPCDicObserver
{
	private static Queue<Action> playerActionsQueue = new Queue<Action> ();
	private static  Queue<Action> enemyActionsQueue = new Queue<Action> ();
	private static int characterActionCounter = 0;
	private static int characterAttackCounter = 0;

	//set first player actions which is activating characters
	public static void SetPlayerActionQueue (Queue<Action> action)
	{
		Debug.Log("RECEIVE PLAYER QUEUE");
		playerActionsQueue = action;
		CheckActionList ();
	}

	//set first enemy actions which is activating characters
	public static void SetEnemyActionQueue (Queue<Action> action)
	{
		Debug.Log("RECEIVE ENEMY QUEUE");
		enemyActionsQueue = action;
		CheckActionList ();
	}

	//after confirming that both players have sent skill, send attack
	private static void CheckActionList ()
	{
		characterActionCounter++;
		Debug.Log("CHARACTER COUNT " + characterActionCounter);

		//Reminders: Change to 2 if not testing
		if (characterActionCounter == 2) {
			SendAttack ();
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
		SystemFirebaseDBController.Instance.AttackPhase (new AttackModel (SystemGlobalDataController.Instance.player.playerBaseDamage));
		Debug.Log ("SENDING ATTACK");
	}


	//show skill buttons after attack phase is done
	public void OnEndPhase ()
	{
		ClearQueues ();
		ScreenBattleController.Instance.partCharacter.ShowAutoActivateButtons (true);
		RPCDicObserver.RemoveObserver (this);
		characterActionCounter = 0;
		characterAttackCounter = 0;
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
				if (param.ContainsKey (MyConst.RPC_DATA_ATTACK)) {
					AttackModel attack = JsonUtility.FromJson<AttackModel> (param [MyConst.RPC_DATA_ATTACK].ToString ());

					if (SystemGlobalDataController.Instance.isSender.Equals (SystemGlobalDataController.Instance.isHost)) {
						playerActionsQueue.Enqueue (delegate() {
							AttackCompute (true, attack);
						});
					} else {
						enemyActionsQueue.Enqueue (delegate() {
							AttackCompute (false, attack);
						});
					}
					CheckAttackList ();
				}

			
			}
		} catch (System.Exception e) {
			//do something later
		}
	}

	public static void AttackCompute (bool isPLayer, AttackModel attack)
	{
		if (isPLayer) {
			Debug.Log ("PLAYER DAMAGE: " + attack.attackDamage);
			ScreenBattleController.Instance.partState.enemy.playerHP -= attack.attackDamage;

		} else {
			Debug.Log ("ENEMY DAMAGE: " + attack.attackDamage);
			ScreenBattleController.Instance.partState.player.playerHP -= attack.attackDamage;
		}
	}


	private static void CheckAttackList ()
	{
		characterAttackCounter++;
		Debug.Log ("CHARACTER ATTACK COUNTER " + characterAttackCounter);
		if (characterAttackCounter == 2) {
			SetBattle ();
			Debug.Log ("Starting battle logic");
		}
	}

	public static void SetBattle ()
	{
		Queue<Queue<Action>> actionsQueue = new Queue<Queue<Action>> ();

		int attackOrder = GetBattleOrder ();
		switch (attackOrder) {
		case 0:
			actionsQueue.Enqueue (playerActionsQueue);
			actionsQueue.Enqueue (enemyActionsQueue);
			ScreenBattleController.Instance.partState.SetActionsWithDelay (actionsQueue);
			break;
		case 1:
			actionsQueue.Enqueue (enemyActionsQueue);
			actionsQueue.Enqueue (playerActionsQueue);
			ScreenBattleController.Instance.partState.SetActionsWithDelay (actionsQueue);
			break;
		case 2:
			actionsQueue.Enqueue (playerActionsQueue);
			actionsQueue.Enqueue (enemyActionsQueue);
			ScreenBattleController.Instance.partState.SetActionsWithOutDelay (actionsQueue);
			break;
		}

	}

	public static int GetBattleOrder ()
	{
		int battleOrder = 0;
		QuestionResultCountModel playerAnswerParam = SystemGlobalDataController.Instance.playerAnswerParam;
		QuestionResultCountModel enemyAnswerParam = SystemGlobalDataController.Instance.enemyAnswerParam;

		if (playerAnswerParam.correctCount > enemyAnswerParam.correctCount) {
			battleOrder = 0;
		} else if (playerAnswerParam.correctCount < enemyAnswerParam.correctCount) {
			battleOrder = 1;
		} else {
			if (playerAnswerParam.speedyCount > enemyAnswerParam.speedyCount) {
				battleOrder = 0;
			} else if (playerAnswerParam.speedyCount < enemyAnswerParam.speedyCount) {
				battleOrder = 1;
			} else {
				battleOrder = 2;
			}
		}
		return battleOrder;
	}



}

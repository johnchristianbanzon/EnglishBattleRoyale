using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleManager: IRPCDicObserver
{
	private static int characterActionCounter = 0;
	private static int characterAttackCounter = 0;

	//after confirming that both players have sent skill, send attack
	public static void CountCharacters ()
	{
		characterActionCounter++;
		Debug.Log("CHARACTER COUNT " + characterActionCounter);

		//Reminders: Change to 2 if not testing
		if (characterActionCounter == 2) {
			SendAttack ();
		}
	}

	private static void ClearCounters(){
		characterActionCounter = 0;
		characterAttackCounter = 0;
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
		ScreenBattleController.Instance.partCharacter.ShowAutoActivateButtons (true);
		RPCDicObserver.RemoveObserver (this);
		ClearCounters ();
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

					if (userHome.Equals (SystemGlobalDataController.Instance.isHost)) {
						playerAttack = attack;
					} else {
						enemyAttack = attack;
					}
					CheckAttackList ();
				}

			
			}
		} catch (System.Exception e) {
			//do something later
		}
	}

	private static AttackModel playerAttack;
	private static AttackModel enemyAttack;

	private static Action GetPlayerAttack(){
		return PlayerAttackCompute;
	}

	private static void PlayerAttackCompute(){
		AttackCompute (true, playerAttack);
	}

	private static Action GetEnemyAttack(){
		return EnemyAttackCompute;
	}

	private static void EnemyAttackCompute(){
		AttackCompute (false, enemyAttack);
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
		}
	}

	public static void SetBattle ()
	{
		Queue<Action> actionsQueue = new Queue<Action> ();

		int attackOrder = GetBattleOrder ();
		switch (attackOrder) {
		case 0:
			actionsQueue.Enqueue (CharacterManager.GetPlayerCharacterActivate ());
			actionsQueue.Enqueue (GetPlayerAttack ());
			actionsQueue.Enqueue (CharacterManager.GetEnemyCharacterActivate ());
			actionsQueue.Enqueue (GetEnemyAttack ());
			ScreenBattleController.Instance.partState.SetActions (actionsQueue);
			break;
		case 1:
			actionsQueue.Enqueue (CharacterManager.GetEnemyCharacterActivate());
			actionsQueue.Enqueue (GetEnemyAttack ());
			actionsQueue.Enqueue (CharacterManager.GetPlayerCharacterActivate());
			actionsQueue.Enqueue (GetPlayerAttack ());
			ScreenBattleController.Instance.partState.SetActions (actionsQueue);
			break;
		case 2:
			actionsQueue.Enqueue (CharacterManager.GetEnemyCharacterActivate());
			actionsQueue.Enqueue (CharacterManager.GetPlayerCharacterActivate());
			actionsQueue.Enqueue (GetEnemyAttack ());
			actionsQueue.Enqueue (GetPlayerAttack ());
			ScreenBattleController.Instance.partState.SetActions (actionsQueue);
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

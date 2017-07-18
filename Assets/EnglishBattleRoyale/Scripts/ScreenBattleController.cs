using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBattleController: SingletonMonoBehaviour<ScreenBattleController>, IRPCDicObserver
{

	private List<bool> userHome = new List<bool> ();
	private List<Dictionary<string, System.Object>> param = new List<Dictionary<string, object>> ();
	private Dictionary<bool, Dictionary<string, object>> thisCurrentParameter = new Dictionary<bool, Dictionary<string, object>> ();
	private int attackCount = 0;

	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
		if (rpcReceive.ContainsKey ("param")) {
			bool userHome = (bool)rpcReceive ["userHome"];
			GlobalDataManager.attackerBool = userHome;

			Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive ["param"];
			string stringParam = param ["Attack"].ToString ();

			Dictionary<string, System.Object> attackerParam = JsonConverter.JsonStrToDic (stringParam);
			thisCurrentParameter.Add (GlobalDataManager.attackerBool, attackerParam);
			if (thisCurrentParameter.Count == 2) {
				Attack (thisCurrentParameter);
				thisCurrentParameter.Clear ();
			} 
		}
	}

	public void Attack (Dictionary<bool, Dictionary<string, object>> currentParam)
	{
		foreach (KeyValuePair<bool, Dictionary<string, System.Object>> newParam in currentParam) {
			userHome.Add (newParam.Key);
			param.Add (newParam.Value);
		}

		//change order of list if host or visitor
		if (GlobalDataManager.isHost) {
			if (userHome [0] != GlobalDataManager.isHost) {
				ChangeUserOrder (0, 1);
			}
		} else {
			if (userHome [1] !=GlobalDataManager.isHost) {
				ChangeUserOrder (1, 0);
			}

		}

		Debug.Log ("HOST IS" + userHome [0]);

		//set attack order between opponents
		int attackOrder = 0;

		if (GlobalDataManager.hAnswer > GlobalDataManager.vAnswer) {
			attackOrder = 0;
		} else if (GlobalDataManager.hAnswer < GlobalDataManager.vAnswer) {
			attackOrder = 1;
		} else {
			if (GlobalDataManager.hTime > GlobalDataManager.vTime) {
				attackOrder = 0;
			} else if (GlobalDataManager.hTime < GlobalDataManager.vTime) {
				attackOrder = 1;
			} else {
				attackOrder = 2;
			}
		}
			
		switch (attackOrder) {
		case 0:
			Debug.Log ("player first attack");
			StartCoroutine (SetAttack (0, 1, 2));

			break;
		case 1:
			Debug.Log ("enemy first attack");

			StartCoroutine (SetAttack (1, 0, 2));

			break;
		case 2:
			Debug.Log ("same attack");
			StartCoroutine (SetAttack (0, 1, 0, true));
			StartCoroutine (StartAttackSequence (3));
			break;
		}

	}

	private void ChangeUserOrder (int index0, int index1)
	{
		bool tempName = userHome [index0];
		Dictionary<string, System.Object> tempParam = param [index0];

		userHome.Insert (index0, userHome [index1]);
		userHome.Insert (index1, tempName);
		param.Insert (index0, param [index1]);
		param.Insert (index1, tempParam);
	}



	private IEnumerator SetAttack (int firstIndex, int secondIndex, int yieldTime, bool isSameAttack = false)
	{
		AttackParameter (userHome [firstIndex], param [firstIndex], isSameAttack);
		yield return new WaitForSeconds (yieldTime);
		AttackParameter (userHome [secondIndex], param [secondIndex], isSameAttack);

		userHome.Clear ();
	}

	public void CheckBattleStatus (bool secondCheck)
	{
		StartCoroutine (CheckBattleDelay (secondCheck));
	}



	IEnumerator CheckBattleDelay (bool secondCheck)
	{
		if (BattleController.Instance.EnemyHP <= 0 || BattleController.Instance.PlayerHP <= 0) {
			SystemLoadScreenController.Instance.StopWaitOpponentScreen ();
			CameraWorksController.Instance.StartWinLoseCamera ();

			if (BattleController.Instance.EnemyHP > 0 && BattleController.Instance.PlayerHP <= 0) {
				BattleController.Instance.ShowWinLose ("lose", "win", "LOSE", AudioEnum.Lose);

			} else if (BattleController.Instance.PlayerHP > 0 && BattleController.Instance.EnemyHP <= 0) {
				BattleController.Instance.ShowWinLose ("win", "lose", "WIN", AudioEnum.Win);

			} else {
				BattleController.Instance.ShowWinLose ("lose", "lose", "DRAW", AudioEnum.Lose);

			}

			StopAllCoroutines ();

		} else {
			if (secondCheck) {
				if (GlobalDataManager.isHost) {
					if (GlobalDataManager.modePrototype == ModeEnum.Mode1) {
						SystemFirebaseDBController.Instance.UpdateAnswerBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0, 0, 0, 0, 0);
					} else if (GlobalDataManager.modePrototype == ModeEnum.Mode2) {
						SystemFirebaseDBController.Instance.UpdateBattleStatus (MyConst.BATTLE_STATUS_SKILL, 0);

					}
				}
				yield return new WaitForSeconds (3);
				PhaseManager.StartPhase1 ();
				//reset effects done by skill and battle data
				GlobalDataManager.ResetPlayer();


				param.Clear ();
				Debug.Log ("player damage reset! now damage is: " + GlobalDataManager.player.playerBaseDamage);
			}
		}
	}


	private void AttackParameter (bool attackerBool, Dictionary<string, System.Object> attackerParam, bool sameAttack = false)
	{
		if (attackerParam [ParamNames.Attack.ToString ()] != null) {
			int damage = int.Parse (attackerParam [ParamNames.Attack.ToString ()].ToString ());

			if (attackerBool.Equals (GlobalDataManager.isHost)) {
				Debug.Log ("PLAYER DAMAGE: " + damage);
				BattleController.Instance.EnemyHP -= damage;
				if (sameAttack == false) {
					StartCoroutine (StartAttackSequence (1));
				}

			} else {
				Debug.Log ("ENEMY DAMAGE: " + damage);
				BattleController.Instance.PlayerHP -= damage;
				if (sameAttack == false) {
					StartCoroutine (StartAttackSequence (2));
				}
			}
		}

	}



	IEnumerator StartAttackSequence (int sequenceType)
	{

		switch (sequenceType) {
		case 1:
			
			StartAttackSequenceReduce (AudioEnum.Attack, true, "attack");
			yield return new WaitForSeconds (0.5f);
			StartAttackSequenceReduce (AudioEnum.Hit, false, "hit");
			CheckAttackCount ();
			break;
		case 2:
			StartAttackSequenceReduce (AudioEnum.Hit, true, "hit");
			yield return new WaitForSeconds (0.5f);
			StartAttackSequenceReduce (AudioEnum.Attack, false, "attack");
			CheckAttackCount ();
			break;
		case 3:
			StartAttackSequenceReduce (AudioEnum.Attack, true, "attack");
			StartAttackSequenceReduce (AudioEnum.Attack, false, "attack");
			yield return new WaitForSeconds (0.5f);
			StartAttackSequenceReduce (AudioEnum.Hit, true, "hit");
			StartAttackSequenceReduce (AudioEnum.Hit, false, "hit");

			CheckBattleStatus (true);
			break;

		}

	}

	private void CheckAttackCount(){
		attackCount++;
		if (attackCount == 2) {
			CheckBattleStatus (true);
			attackCount = 0;
		} else {
			CheckBattleStatus (false);
		}
	}

	private void StartAttackSequenceReduce (AudioEnum audioType, bool isPlayer, string animParam)
	{

		CharacterAvatarsController.Instance.SetTriggerAnim (isPlayer, animParam);
		AudioController.Instance.PlayAudio (audioType);
	}


}

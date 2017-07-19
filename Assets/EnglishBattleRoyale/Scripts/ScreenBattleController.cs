using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBattleController: SingletonMonoBehaviour<ScreenBattleController>, IRPCDicObserver
{
	private string playerName;
	private float playerHP;
	private float playerGP;

	private string enemyName;
	private float enemyHP;

	private List<bool> userHome = new List<bool> ();
	private List<Dictionary<string, System.Object>> param = new List<Dictionary<string, object>> ();
	private Dictionary<bool, Dictionary<string, object>> thisCurrentParameter = new Dictionary<bool, Dictionary<string, object>> ();
	private int attackCount = 0;

	public float PlayerHP { 
		get {
			return playerHP;
		} 
		set {
			playerHP = value;
			PartBattleUIController.Instance.playerHPText.text = playerHP.ToString();
			TweenFacade.TweenPlayerHPSlider (playerHP,2,true,PartBattleUIController.Instance.playerHPBar);
		} 
	}

	public float PlayerGP { 
		get {
			return playerGP;
		} 
		set {
			playerGP = value;
			PartBattleUIController.Instance.playerGPText.text = playerGP.ToString();
			TweenFacade.TweenPlayerHPSlider (playerHP,2,true,PartBattleUIController.Instance.playerGPBar);
		} 
	}

	public float EnemyHP { 
		get {
			return enemyHP;
		} 
		set {
			enemyHP = value;
			PartBattleUIController.Instance.enemyHPText.text = enemyHP.ToString();
			TweenFacade.TweenPlayerHPSlider (playerHP,2,true,PartBattleUIController.Instance.enemyHPBar);
		} 
	}

	void Start ()
	{
		RPCDicObserver.AddObserver (this);
		foreach (KeyValuePair<Firebase.Database.DataSnapshot, bool> initialState in SystemGlobalDataController.Instance.InitialState) {
			SetStateParam (initialState.Key, initialState.Value);
		}
	}

	private void SetStateParam (Firebase.Database.DataSnapshot dataSnapShot, bool isHome)
	{
		Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
		if (rpcReceive.ContainsKey ("param")) {
			Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive ["param"];
			ReceiveInitialState (param, isHome);
		}
	}

	private void ReceiveInitialState (Dictionary<string, System.Object> initialState, bool isHome)
	{
		string playerName = (string)initialState ["playerName"];
		int playerHP = int.Parse (initialState ["playerHP"].ToString ());
		int playerGP = int.Parse (initialState ["playerGP"].ToString ());

		if (isHome) {
			SetInitialPlayerState (playerName, playerHP, playerGP);
		} else {
			SetInitialEnemyState (playerName, playerHP);
		}
	}

	private void SetInitialPlayerState (string name, float hP, float gP)
	{
		playerName = name;
		playerHP = hP;
		playerGP = gP;
		PartBattleUIController.Instance.SetInitialPlayerUI (playerName, playerHP, playerGP);
	}

	private void SetInitialEnemyState (string name, float hP)
	{
		enemyName = name;
		enemyHP = hP;
		PartBattleUIController.Instance.SetInitialEnemyUI (enemyName, enemyHP);
	}





	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
		if (rpcReceive.ContainsKey ("param")) {
			bool userHome = (bool)rpcReceive ["userHome"];
			SystemGlobalDataController.Instance.attackerBool = userHome;

			Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive ["param"];
			string stringParam = param ["Attack"].ToString ();

			Dictionary<string, System.Object> attackerParam = JsonConverter.JsonStrToDic (stringParam);
			thisCurrentParameter.Add (SystemGlobalDataController.Instance.attackerBool, attackerParam);
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
		if (SystemGlobalDataController.Instance.isHost) {
			if (userHome [0] != SystemGlobalDataController.Instance.isHost) {
				ChangeUserOrder (0, 1);
			}
		} else {
			if (userHome [1] != SystemGlobalDataController.Instance.isHost) {
				ChangeUserOrder (1, 0);
			}

		}

		Debug.Log ("HOST IS" + userHome [0]);

		//set attack order between opponents
		int attackOrder = 0;

		if (SystemGlobalDataController.Instance.hAnswer > SystemGlobalDataController.Instance.vAnswer) {
			attackOrder = 0;
		} else if (SystemGlobalDataController.Instance.hAnswer < SystemGlobalDataController.Instance.vAnswer) {
			attackOrder = 1;
		} else {
			if (SystemGlobalDataController.Instance.hTime > SystemGlobalDataController.Instance.vTime) {
				attackOrder = 0;
			} else if (SystemGlobalDataController.Instance.hTime < SystemGlobalDataController.Instance.vTime) {
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



	IEnumerator SetAttack (int firstIndex, int secondIndex, int yieldTime, bool isSameAttack = false)
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

	public void ShowWinLose (string winLoseText, AudioEnum winLoseAudio)
	{
		AudioController.Instance.PlayAudio (winLoseAudio);
		GameObject battleResult = SystemPopupController.Instance.ShowPopUp ("PopUpBattleResult");
		battleResult.GetComponent<PopUpBattleResultController> ().SetBattleResultText (winLoseText);
	}


	IEnumerator CheckBattleDelay (bool secondCheck)
	{
		if (enemyHP <= 0 || playerHP <= 0) {
			SystemLoadScreenController.Instance.StopWaitOpponentScreen ();
			PartCameraWorksController.Instance.StartWinLoseCamera ();

			if (enemyHP > 0 && playerHP <= 0) {
				ShowWinLose ("LOSE", AudioEnum.Lose);

			} else if (playerHP > 0 && enemyHP <= 0) {
				ShowWinLose ("WIN", AudioEnum.Win);

			} else {
				ShowWinLose ("DRAW", AudioEnum.Lose);

			}

			StopAllCoroutines ();

		} else {
			if (secondCheck) {
				if (SystemGlobalDataController.Instance.isHost) {
					if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode1) {
						SystemFirebaseDBController.Instance.UpdateAnswerBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0, 0, 0, 0, 0);
					} else if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
						SystemFirebaseDBController.Instance.UpdateBattleStatus (MyConst.BATTLE_STATUS_SKILL, 0);

					}
				}
				yield return new WaitForSeconds (3);
				PartPhaseController.Instance.StartPhase1 ();
				//reset effects done by skill and battle data
				SystemGlobalDataController.Instance.ResetPlayer ();


				param.Clear ();
				Debug.Log ("player damage reset! now damage is: " + SystemGlobalDataController.Instance.player.playerBaseDamage);
			}
		}
	}


	private void AttackParameter (bool attackerBool, Dictionary<string, System.Object> attackerParam, bool sameAttack = false)
	{
		if (attackerParam [ParamNames.Attack.ToString ()] != null) {
			int damage = int.Parse (attackerParam [ParamNames.Attack.ToString ()].ToString ());

			if (attackerBool.Equals (SystemGlobalDataController.Instance.isHost)) {
				Debug.Log ("PLAYER DAMAGE: " + damage);
				enemyHP -= damage;
				if (sameAttack == false) {
					StartCoroutine (StartAttackSequence (1));
				}

			} else {
				Debug.Log ("ENEMY DAMAGE: " + damage);
				playerHP -= damage;
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

	private void CheckAttackCount ()
	{
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
		AudioController.Instance.PlayAudio (audioType);
	}


}

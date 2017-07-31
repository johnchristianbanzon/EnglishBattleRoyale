using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/* Controls the battle */
public class PartStateController : MonoBehaviour, IGameTimeObserver,IRPCDicObserver
{
	public Text playerNameText;


	public Slider playerHPBar;
	public Text playerHPText;
	public Text playerGPText;

	public Slider playerGPBar;


	public Slider enemyHPBar;
	public Text enemyNameText;
	public Text enemyHPText;

	private string playerName;
	private float playerHP;
	private float playerGP;

	private string enemyName;
	private float enemyHP;

	private List<bool> userHome = new List<bool> ();
	private List<Dictionary<string, System.Object>> param = new List<Dictionary<string, object>> ();
	private Dictionary<bool, Dictionary<string, object>> thisCurrentParameter = new Dictionary<bool, Dictionary<string, object>> ();
	private int attackCount = 0;

	//get set player hp, everytime it is set, it updates the hp UI
	public float PlayerHP { 
		get {
			return playerHP;
		} 
		set {
			playerHP = value;
			playerHPText.text = playerHP.ToString ();
			TweenFacade.TweenPlayerHPSlider (playerHP, 2, true, playerHPBar);
		} 
	}
	//get set player gp, everytime it is set, it updates the hp UI
	public float PlayerGP { 
		get {
			return playerGP;
		} 
		set {
			playerGP = value;
			playerGPText.text = playerGP.ToString ();
			TweenFacade.TweenPlayerHPSlider (playerHP, 2, true, playerGPBar);
		} 
	}
	//get set enemy hp, everytime it is set, it updates the hp UI
	public float EnemyHP { 
		get {
			return enemyHP;
		} 
		set {
			enemyHP = value;
			enemyHPText.text = enemyHP.ToString ();
			TweenFacade.TweenPlayerHPSlider (playerHP, 2, true, enemyHPBar);
		} 
	}


	void Start ()
	{
		GameTimeManager.AddObserver (this);
		GameTimeManager.StartPreBattleTimer ();
		AudioController.Instance.PlayAudio (AudioEnum.Bgm);

		ScreenBattleController.Instance.partCameraWorks.StartIntroCamera ();

		RPCDicObserver.AddObserver (this);

		//get initial state for enemy and player stored in SystemGlobalDataController
		foreach (KeyValuePair<Firebase.Database.DataSnapshot, bool> initialState in SystemGlobalDataController.Instance.InitialState) {
			SetStateParam (initialState.Key, initialState.Value);
		}
	}

	#region INITIAL STATE

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
		SetInitialPlayerUI (playerName, playerHP, playerGP);
	}

	private void SetInitialEnemyState (string name, float hP)
	{
		enemyName = name;
		enemyHP = hP;
		SetInitialEnemyUI (enemyName, enemyHP);
	}

	public void SetInitialPlayerUI (string name, float hP, float gP)
	{
		playerHPBar.value = hP;
		playerHPBar.maxValue = hP;
		playerNameText.text = name;
		playerGPText.text = gP.ToString ();
		playerGPBar.value = gP;
		playerGPBar.maxValue = SystemGlobalDataController.Instance.player.playerMaxGP;
	}

	public void SetInitialEnemyUI (string name, float hP)
	{
		enemyNameText.text = name;
		enemyHPText.text = hP.ToString ();
		enemyHPBar.value = hP;
		enemyHPBar.maxValue = hP;
	}

	#endregion

	#region BATTLE LOGIC AND ANIMATION

	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
		if (rpcReceive.ContainsKey ("param")) {
			bool userHome = (bool)rpcReceive ["userHome"];
			SystemGlobalDataController.Instance.isSender = userHome;

			Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive ["param"];
			string stringParam = param ["Attack"].ToString ();

			Dictionary<string, System.Object> attackerParam = JsonConverter.JsonStrToDic (stringParam);
			thisCurrentParameter.Add (SystemGlobalDataController.Instance.isSender, attackerParam);
			if (thisCurrentParameter.Count == 2) {
				Attack (thisCurrentParameter);
				thisCurrentParameter.Clear ();
			} 
		}
	}

	public void Attack (Dictionary<bool, Dictionary<string, object>> currentParam)
	{

		KeyValuePair<bool, Dictionary<string, System.Object>> getParam = BattleLogic.GetAttackParam (currentParam);
		userHome.Add (getParam.Key);
		param.Add (getParam.Value);

		//change order of list if host or visitor
		BattleLogic.ChangeOrder (delegate(List<bool> userHome, List<Dictionary<string, object>> param) {
			this.userHome = userHome;
			this.param = param;
		}, userHome, param);


		Debug.Log ("HOST IS" + userHome [0]);

		//set attack order between opponents
		int attackOrder = BattleLogic.GetAttackOrder ();

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

	IEnumerator SetAttack (int firstIndex, int secondIndex, int yieldTime, bool isSameAttack = false)
	{
		AttackCalculate (userHome [firstIndex], param [firstIndex], isSameAttack);
		yield return new WaitForSeconds (yieldTime);
		AttackCalculate (userHome [secondIndex], param [secondIndex], isSameAttack);
		userHome.Clear ();
	}


	private void AttackCalculate (bool attackerBool, Dictionary<string, System.Object> attackerParam, bool sameAttack = false)
	{
		int attackSequence = BattleLogic.AttackLogic (delegate(float enemyHP, float playerHP) {
			this.enemyHP = enemyHP;
			this.playerHP = playerHP;
		}, enemyHP, playerHP, attackerBool, attackerParam, sameAttack);

		if (attackSequence != 0) {
			StartCoroutine (StartAttackSequence (attackSequence));
		}
	}

	//Animation and sound when attacking
	IEnumerator StartAttackSequence (int sequenceType)
	{

		switch (sequenceType) {
		case 1:
			//animate and sound here
			yield return new WaitForSeconds (0.5f);
			//animate and sound here
			CheckAttackCount ();
			break;
		case 2:
			//animate and sound here
			yield return new WaitForSeconds (0.5f);
			//animate and sound here
			CheckAttackCount ();
			break;
		case 3:
			//animate and sound here
			yield return new WaitForSeconds (0.5f);
			//animate and sound here
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


	public void CheckBattleStatus (bool secondCheck)
	{
		StartCoroutine (CheckBattleStatusDelay (secondCheck));
	}

	IEnumerator CheckBattleStatusDelay (bool secondCheck)
	{
		bool battleOver = false;
		BattleLogic.CheckBattle (secondCheck, enemyHP, playerHP, delegate(string battleResult, bool isBattleOver) {

			ShowWinLose (battleResult);
			StopAllCoroutines ();
			battleOver = isBattleOver;
		});

		if (!battleOver) {
			if (secondCheck) {
				if (SystemGlobalDataController.Instance.isHost) {
					if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode1) {
						SystemFirebaseDBController.Instance.UpdateAnswerBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0, 0, 0, 0, 0);
					} else if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
						SystemFirebaseDBController.Instance.UpdateBattleStatus (MyConst.BATTLE_STATUS_SKILL, 0);

					}
				}
				yield return new WaitForSeconds (3);	
				PhaseManager.StartPhase1 ();
				//reset effects done by skill and battle data
				SystemGlobalDataController.Instance.ResetPlayer ();
				param.Clear ();
				Debug.Log ("player damage reset! now damage is: " + SystemGlobalDataController.Instance.player.playerBaseDamage);
			}
		}
	}

	public void ShowWinLose (string winLoseText)
	{
		GameObject battleResult = SystemPopupController.Instance.ShowPopUp ("PopUpBattleResult");
		battleResult.GetComponent<PopUpBattleResultController> ().SetBattleResultText (winLoseText);
	}

	#endregion

	#region Start and end battle phase

	//attack when start phase
	public void OnStartPhase ()
	{
		ScreenBattleController.Instance.partSkill.ShowAutoActivateButtons (false);
		Debug.Log ("Starting attack phase");
		PartAnswerIndicatorController.Instance.ResetAnswer ();
		Attack ();
	}
	//send attack to firebase
	public void Attack ()
	{
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.Attack.ToString ()] = SystemGlobalDataController.Instance.player.playerBaseDamage + SystemGlobalDataController.Instance.gpEarned;
		SystemFirebaseDBController.Instance.AttackPhase (new AttackModel (JsonConverter.DicToJsonStr (param)));
	}
	//show skill buttons after attack phase is done
	public void OnEndPhase ()
	{
		ScreenBattleController.Instance.partSkill.ShowAutoActivateButtons (true);
	}

	#endregion

	#region TIMER Subscriber

	public Text gameTimerText;
	private bool hasAnswered = false;

	public void OnStartPreBattleTimer (int timer)
	{
		StopTimer ();
		StartCoroutine (PreBattleStartTimer (timer));
	}

	public void OnStartSkillTimer (Action action, int timer)
	{
		StopTimer ();
		StartCoroutine (StartSkillTimer (action, timer));
	}

	public void OnStartSelectQuestionTimer (Action action, int timer)
	{
		StopTimer ();
		StartCoroutine (StartSelectQuestionTimer (action, timer));
	}

	public void OnStartQuestionTimer (Action action, int timer)
	{
		StopTimer ();
		StartCoroutine (StartQuestionTimer (action, timer));
	}

	public void OnHasAnswered (bool hasAnswered)
	{
		this.hasAnswered = hasAnswered;
	}

	public void OnStopTimer ()
	{
		StopTimer ();
	}

	public void OnToggleTimer (bool toggleTimer)
	{
		gameTimerText.enabled = toggleTimer;
	}

	#region PrebattleTimer


	IEnumerator PreBattleStartTimer (int timer)
	{
		OnToggleTimer (true);
		int timeLeft = timer;

		while (timeLeft > 0) {
			gameTimerText.text = "" + timeLeft;
			timeLeft--;
			yield return new WaitForSeconds (1);

		}

		OnToggleTimer (false);
		PhaseManager.StartPhase1 ();
	}

	#endregion

	#region SkillTimer


	IEnumerator StartSkillTimer (Action action, int timer)
	{
		OnToggleTimer (true);
		int timeLeft = timer;

		while (timeLeft > 0) {
			gameTimerText.text = "" + timeLeft;
			timeLeft--;
			yield return new WaitForSeconds (1);
		}

		action ();
		OnToggleTimer (false);
		SystemFirebaseDBController.Instance.SkillPhase ();
	}

	#endregion

	#region SelectQuestionTimer

	IEnumerator StartSelectQuestionTimer (Action action, int timer)
	{
		OnToggleTimer (true);
		int timeLeft = timer;

		while (timeLeft > 0 && hasAnswered == false) {
			gameTimerText.text = "" + timeLeft;
			timeLeft--;
			yield return new WaitForSeconds (1);
		}

		action ();
		OnToggleTimer (false);
	}

	#endregion

	#region QuestionTimer

	IEnumerator StartQuestionTimer (Action action, int timer)
	{
		OnToggleTimer (true);
		int timeLeft = timer;

		while (timeLeft > 0) {
			gameTimerText.text = "" + timeLeft;
			timeLeft--;
			action ();
			yield return new WaitForSeconds (1);
		}
		OnToggleTimer (false);
	}

	#endregion

	public void StopTimer ()
	{
		StopAllCoroutines ();
	}

	#endregion

}

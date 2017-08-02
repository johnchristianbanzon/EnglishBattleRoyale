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

	public PlayerModel player{ get; set; }

	public PlayerModel enemy{ get; set; }

	private List<bool> userHome = new List<bool> ();
	private List<Dictionary<string, System.Object>> param = new List<Dictionary<string, object>> ();
	private int attackCount = 0;

	void Update ()
	{
		if (player != null && enemy != null) {
			playerHPText.text = player.playerHP.ToString ();
			playerHPBar.value = player.playerHP;

			playerGPText.text = player.playerGP.ToString ();
			playerGPBar.value = player.playerGP;

			enemyHPText.text = enemy.playerHP.ToString ();
			enemyHPBar.value = enemy.playerHP;
		}
	}


	void Start ()
	{
		Init ();
	}

	private void Init(){
		TimeManager.AddGameTimeObserver (this);
		RPCDicObserver.AddObserver (this);
		TimeManager.StartPreBattleTimer (3);
		AudioController.Instance.PlayAudio (AudioEnum.Bgm);

		ScreenBattleController.Instance.partCameraWorks.StartIntroCamera ();

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
		PlayerModel player = JsonUtility.FromJson<PlayerModel> (initialState ["PlayerRPC"].ToString ());

		if (isHome) {
			SetInitialPlayerUI (player);
		} else {
			SetInitialEnemyUI (player);
		}
	}

	public void SetInitialPlayerUI (PlayerModel player)
	{
		playerNameText.text = player.playerName;

		playerHPText.text = player.playerHP.ToString ();
		playerHPBar.value = player.playerHP;
		playerHPBar.maxValue = player.playerHP;

		playerGPText.text = player.playerGP.ToString ();
		playerGPBar.value = player.playerGP;
		playerGPBar.maxValue = player.playerGP;
	}

	public void SetInitialEnemyUI (PlayerModel enemy)
	{
		enemyNameText.text = enemy.playerName;
		enemyHPText.text = enemy.playerHP.ToString ();
		enemyHPBar.value = enemy.playerHP;
		enemyHPBar.maxValue = enemy.playerHP;
	}

	#endregion

	//REFACCCTOOOOOOOR AAAAALLLLLLL
	#region BATTLE LOGIC AND ANIMATION

	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		try {
			Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
			if (rpcReceive.ContainsKey ("param")) {
				bool userHome = (bool)rpcReceive ["userHome"];
				SystemGlobalDataController.Instance.isSender = userHome;

				Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive ["param"];
				AttackModel attack = JsonUtility.FromJson<AttackModel> (param ["AttackRPC"].ToString ());


//				if (thisCurrentParameter.Count == 2) {
//					Attack (thisCurrentParameter);
//					thisCurrentParameter.Clear ();
//				} 
			}
		} catch (System.Exception e) {
			//do something later
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
			enemy.playerHP = enemyHP;
			player.playerHP = playerHP;
		}, enemy.playerHP, player.playerHP, attackerBool, attackerParam, sameAttack);

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
		BattleLogic.CheckBattle (secondCheck, enemy.playerHP, player.playerHP, delegate(string battleResult, bool isBattleOver) {

			ShowWinLose (battleResult);
			StopAllCoroutines ();
			battleOver = isBattleOver;
		});

		if (!battleOver) {
			if (secondCheck) {
				if (SystemGlobalDataController.Instance.isHost) {
					if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode1) {
						SystemFirebaseDBController.Instance.UpdateBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0,"0","0");
					} else if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
						SystemFirebaseDBController.Instance.UpdateBattleStatus (MyConst.BATTLE_STATUS_CHARACTER, 0);

					}
				}
				yield return new WaitForSeconds (3);	
				ScreenBattleController.Instance.StartPhase1 ();
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
		ScreenBattleController.Instance.partCharacter.ShowAutoActivateButtons (false);
		Debug.Log ("Starting attack phase");
		PartAnswerIndicatorController.Instance.ResetAnswer ();
		Attack ();
	}
	//send attack to firebase
	public void Attack ()
	{
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.Attack.ToString ()] = SystemGlobalDataController.Instance.player.playerBaseDamage + SystemGlobalDataController.Instance.gpEarned;
		SystemFirebaseDBController.Instance.AttackPhase (new AttackModel (JsonUtility.ToJson (param)));
	}
	//show skill buttons after attack phase is done
	public void OnEndPhase ()
	{
		ScreenBattleController.Instance.partCharacter.ShowAutoActivateButtons (true);
	}

	#endregion

	#region TIMER Subscriber

	public Text gameTimerText;
	public Text preBattleTimerText;

	public void OnStartGameTimer (int timer)
	{
		StartCoroutine (StartTimer (timer));
	}


	public void OnStartPreBattleTimer (int timer)
	{
		StartCoroutine (StartTimer (timer, delegate() {
			preBattleTimerText.enabled = false;
			TimeManager.StartGameTimer (180);
			ScreenBattleController.Instance.StartPhase1 ();
		}));
	}


	IEnumerator StartTimer (int timer, Action action = null)
	{
		int timeLeft = timer;

		while (timeLeft > 0) {
			preBattleTimerText.text = "" + timeLeft;
			timeLeft--;
			yield return new WaitForSeconds (1);

		}

		action ();
	}
		
	#endregion

}

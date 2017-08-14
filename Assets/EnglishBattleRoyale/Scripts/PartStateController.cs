using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/* Controls the battle */
public class PartStateController : MonoBehaviour, IGameTimeObserver
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

	private void Init ()
	{
		TimeManager.AddGameTimeObserver (this);
		TimeManager.StartPreBattleTimer (3);
		AudioController.Instance.PlayAudio (AudioEnum.Bgm);

		ScreenBattleController.Instance.partCameraWorks.StartIntroCamera ();

		//get initial state for enemy and player stored in SystemGlobalDataController
		foreach (KeyValuePair<Firebase.Database.DataSnapshot, bool> initialState in GameManager.initialState) {
			SetStateParam (initialState.Key, initialState.Value);
		}
	}

	#region INITIAL STATE

	private void SetStateParam (Firebase.Database.DataSnapshot dataSnapShot, bool isHome)
	{
		Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
		if (rpcReceive.ContainsKey (MyConst.RPC_DATA_PARAM)) {
			Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive [MyConst.RPC_DATA_PARAM];

			ReceiveInitialState (param, isHome);
		}
	}

	private void ReceiveInitialState (Dictionary<string, System.Object> initialState, bool isHome)
	{
		PlayerModel player = JsonUtility.FromJson<PlayerModel> (initialState [MyConst.RPC_DATA_PLAYER].ToString ());

		if (isHome) {
			
			SetInitialPlayerUI (player);
		} else {
			SetInitialEnemyUI (player);
		}
	}

	public void SetInitialPlayerUI (PlayerModel player)
	{
		this.player = player;
		playerNameText.text = player.playerName;

		playerHPText.text = player.playerHP.ToString ();
		playerHPBar.maxValue = player.playerHP;
		playerHPBar.value = player.playerHP;


		playerGPText.text = player.playerGP.ToString ();
		playerGPBar.maxValue = player.playerMaxGP;
		playerGPBar.value = player.playerGP;
	
	}

	public void SetInitialEnemyUI (PlayerModel enemy)
	{
		this.enemy = enemy;
		enemyNameText.text = enemy.playerName;
		enemyHPText.text = enemy.playerHP.ToString ();
		enemyHPBar.maxValue = enemy.playerHP;
		enemyHPBar.value = enemy.playerHP;

	}

	#endregion

	#region DamageCoroutine

	public void StartBattle ()
	{
		StartCoroutine (StartBattleCoroutine ());
	}

	IEnumerator StartBattleCoroutine ()
	{

		int attackOrder = BattleManager.GetBattleOrder ();
		switch (attackOrder) {
		case 0:
			Debug.Log ("PLAYER ATTACK FIRST");
			yield return StartCoroutine (BattleLogicCoroutine (true, false));
			yield return new WaitForSeconds (2);
			yield return StartCoroutine (BattleLogicCoroutine (false, true));
			break;

		case 1:
			Debug.Log ("ENEMY ATTACK FIRST");
			yield return StartCoroutine (BattleLogicCoroutine (false, false));
			yield return new WaitForSeconds (2);
			yield return StartCoroutine (BattleLogicCoroutine (true, true));
			break;

		case 2:
			Debug.Log ("BOTH ATTACK SAME TIME");
			CharacterManager.PlayerCharacterActivate ();
			CharacterManager.EnemyCharacterActivate ();
			yield return new WaitForSeconds (3);
			BattleManager.SendAttack ();
			CheckHP (false);
			yield return new WaitForSeconds (2);
			yield return StartCoroutine (CheckBothAttackCoroutine ());
			CheckHP (true);
			break;

		}
	}

	IEnumerator CheckBothAttackCoroutine ()
	{
		
		if (BattleManager.CheckBothAttack ()) {
			BattleManager.ComputeBothAttack ();
		} else {
			yield return new WaitForSeconds (1);
			yield return StartCoroutine (CheckBothAttackCoroutine ());
		}
	}

	IEnumerator CheckPlayerAttackCoroutine ()
	{
		if (BattleManager.CheckPlayerAttack ()) {
			BattleManager.ComputePlayerAttack ();
		} else {
			yield return new WaitForSeconds (1);
			yield return StartCoroutine (CheckPlayerAttackCoroutine ());
		}
	}

	IEnumerator CheckEnemyAttackCoroutine ()
	{
		if (BattleManager.CheckEnemyAttack ()) {
			BattleManager.ComputeEnemyAttack ();
		} else {
			yield return new WaitForSeconds (1);
			yield return StartCoroutine (CheckEnemyAttackCoroutine ());
		}
	}

	IEnumerator BattleLogicCoroutine (bool isPLayer, bool isSecondCheck)
	{
		if (isPLayer) {
			CharacterManager.PlayerCharacterActivate ();
			yield return new WaitForSeconds (3);
			BattleManager.SendAttack ();
			yield return new WaitForSeconds (1);
			yield return StartCoroutine (CheckPlayerAttackCoroutine ());
			CheckHP (isSecondCheck);
		} else {
			CharacterManager.EnemyCharacterActivate ();
			yield return new WaitForSeconds (3);
			BattleManager.SendAttack ();
			yield return new WaitForSeconds (1);
			yield return StartCoroutine (CheckEnemyAttackCoroutine ());
			CheckHP (isSecondCheck);
		}
	}

	//check HP of each player, if there is winner, stop battle
	private void CheckHP (bool isSecondCheck)
	{
		SystemLoadScreenController.Instance.StopWaitOpponentScreen ();
		if (enemy.playerHP <= 0 || player.playerHP <= 0) {

			if (enemy.playerHP > 0 && player.playerHP <= 0) {
				ScreenBattleController.Instance.partAvatars.SetTriggerAnim (true, "lose");
				ScreenBattleController.Instance.partAvatars.SetTriggerAnim (false, "win");
			} else if (player.playerHP > 0 && enemy.playerHP <= 0) {
				ScreenBattleController.Instance.partAvatars.SetTriggerAnim (true, "win");
				ScreenBattleController.Instance.partAvatars.SetTriggerAnim (false, "lose");
			} else {
				ScreenBattleController.Instance.partAvatars.SetTriggerAnim (true, "win");
				ScreenBattleController.Instance.partAvatars.SetTriggerAnim (false, "win");
			}
			StopAllCoroutines ();
			return;
		} 

		if (isSecondCheck) {
			ResetPlayerDamage ();
			ScreenBattleController.Instance.partAvatars.player.UnLoadArmPowerEffect ();
			ScreenBattleController.Instance.partAvatars.enemy.UnLoadArmPowerEffect ();
			BattleManager.ClearBattleData ();
			Invoke ("StartPhase1", 1);
		}
	}

	private void StartPhase1(){
		ScreenBattleController.Instance.StartPhase1 ();
	}

	private void ResetPlayerDamage ()
	{
		player.playerBaseDamage = GameManager.player.playerBaseDamage;
	}

	#endregion

	#region TIMER Subscriber

	public Text gameTimerText;
	public Text preBattleTimerText;

	public void OnStartGameTimer (int timer)
	{
		StartCoroutine (StartTimer (timer, true));
	}


	public void OnStartPreBattleTimer (int timer)
	{
		StartCoroutine (StartTimer (timer, false, delegate() {
			preBattleTimerText.enabled = false;
			ScreenBattleController.Instance.StartPhase1 ();
			TimeManager.StartGameTimer (180);
		}));
	}


	IEnumerator StartTimer (int timer, bool isGameTimer, Action action = null)
	{
		int timeLeft = timer;

		while (timeLeft > 0) {
			if (isGameTimer) {
				gameTimerText.text = "" + timeLeft;
			} else {
				preBattleTimerText.text = "" + timeLeft;
			}
			timeLeft--;
			yield return new WaitForSeconds (1);

		}

		action ();
	}

	#endregion

}

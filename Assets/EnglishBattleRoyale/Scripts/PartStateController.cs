using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/* Controls the battle */
public class PartStateController : MonoBehaviour, IGameTimeObserver
{
	public GameObject playerCardContainer;
	public GameObject enemyCardContainer;

	public Text playerNameText;

	public Slider playerHPBar;
	public Text playerHPText;
	public Text playerGPText;
	public Slider playerGPBar;


	public Slider enemyHPBar;
	public Text enemyNameText;
	public Text enemyHPText;

	public Text playerHitComboCountText;
	public Text playerTotalDamageText;

	public Text enemyHitComboCountText;
	public Text enemyTotalDamageText;

	public PlayerModel player{ get; set; }

	public PlayerModel enemy{ get; set; }

	void Update ()
	{
		if (player != null && enemy != null) {
			if (player.playerHP > 100) {
				player.playerHP = 100;
			}

			if (enemy.playerHP > 100) {
				enemy.playerHP = 100;
			}

			if (player.playerHP < 0) {
				player.playerHP = 0;
			}

			if (enemy.playerHP < 0) {
				enemy.playerHP = 0;
			}

			if (player.playerGP > player.playerMaxGP) {
				player.playerGP = player.playerMaxGP;
			}

			if (player.playerGP < 0) {
				player.playerGP = 0;
			}

			playerHPText.text = player.playerHP.ToString ();
			playerHPBar.value = player.playerHP;

			playerGPText.text = player.playerGP.ToString ();
			playerGPBar.value = player.playerGP;

			enemyHPText.text = enemy.playerHP.ToString ();
			enemyHPBar.value = enemy.playerHP;
		}
	}
		

	#region INITIAL STATE

	public void SetStateParam (Firebase.Database.DataSnapshot dataSnapShot, bool isHome)
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

	#region COROUTINES

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
			yield return StartCoroutine (BattleLogicCoroutine (false, true));
			break;

		case 1:
			Debug.Log ("ENEMY ATTACK FIRST");
			yield return StartCoroutine (BattleLogicCoroutine (false, false));
			yield return StartCoroutine (BattleLogicCoroutine (true, true));
			break;
		}
	}

	IEnumerator BattleLogicCoroutine (bool isPLayer, bool isSecondCheck)
	{
		if (isPLayer) {
			yield return StartCoroutine (CharacterActivateCoroutine (true));
			BattleManager.SendAttack ();
			yield return new WaitForSeconds (1);
			yield return StartCoroutine (CheckAttackCoroutine (true));
			CheckHP (isSecondCheck);
		} else {
			yield return StartCoroutine (CharacterActivateCoroutine (false));
			yield return new WaitForSeconds (1);
			yield return StartCoroutine (CheckAttackCoroutine (false));
			CheckHP (isSecondCheck);
		}
	}

	//check HP of each player, if there is winner, stop battle
	private void CheckHP (bool isSecondCheck)
	{
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
			playerHitComboCountText.text = "";
			playerTotalDamageText.text = "";
			enemyTotalDamageText.text = "";
			enemyHitComboCountText.text = "";
			Invoke ("StartPhase1", 1);
			Debug.Log ("DONE CHECKING: STARTING PHASE 1");
		}
	}

	IEnumerator CharacterActivateCoroutine (bool isPlayer)
	{
		while (CharacterManager.GetCharacterCount (isPlayer) > 0) {
			yield return new WaitForSeconds (1);
			CharacterManager.CharacterActivate (isPlayer);
		}
	}

	IEnumerator CheckAttackCoroutine (bool isPlayer)
	{
		if (BattleManager.CheckAttack (isPlayer)) {
			SystemLoadScreenController.Instance.StopWaitOpponentScreen ();
			yield return BattleManager.ComputeAttack (isPlayer);
		} else {
			yield return new WaitForSeconds (1);
			yield return StartCoroutine (CheckAttackCoroutine (isPlayer));
		}
	}


	public IEnumerator StartBattleAnimation (bool isPLayer, float attackDamage, Action action)
	{
		string hitComboCount = "";
		string totalDamageCount = "";
		QuestionResultCountModel playerAnswerParam = GameManager.playerAnswerParam;
		QuestionResultCountModel enemyAnswerParam = GameManager.enemyAnswerParam;

		QuestionResultCountModel answerParam = null;
		if (isPLayer) {
			answerParam = playerAnswerParam;
		} else {
			answerParam = enemyAnswerParam;
		}

		for (int i = 0; i <= answerParam.correctCount; i++) {
			ScreenBattleController.Instance.partAvatars.SetTriggerAnim (isPLayer, "attack1");
			ScreenBattleController.Instance.partAvatars.SetTriggerAnim (!isPLayer, "hit1");

			if (isPLayer) {
				ScreenBattleController.Instance.partAvatars.enemy.LoadHitEffect ();
			} else {
				ScreenBattleController.Instance.partAvatars.player.LoadHitEffect ();
			}

			hitComboCount = (i + 1) + " HIT COMBO";
			SystemSoundController.Instance.PlaySFX ("SFX_HIT");

			yield return new WaitForSeconds (1);
		}

		action ();
		totalDamageCount = attackDamage + " DAMAGE";
	
		if (isPLayer) {
			playerHitComboCountText.text = hitComboCount;
			playerTotalDamageText.text = totalDamageCount;
		} else {
			enemyHitComboCountText.text = hitComboCount;
			enemyTotalDamageText.text = totalDamageCount;
		}

		yield return new WaitForSeconds (1);

	}

	private void StartPhase1 ()
	{
		ScreenBattleController.Instance.StartPhase1 ();
	}

	private void ResetPlayerDamage ()
	{
		player.playerBaseDamage = GameManager.player.playerBaseDamage;
	}

	#endregion

	#region TIMER Subscriber

	public Text preBattleTimerText;

	public void OnStartPreBattleTimer (int timer)
	{
		StartCoroutine (StartTimer (timer, delegate() {
			preBattleTimerText.enabled = false;
			ScreenBattleController.Instance.StartPhase1 ();
		}));
	}


	IEnumerator StartTimer (int timer,Action action = null)
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

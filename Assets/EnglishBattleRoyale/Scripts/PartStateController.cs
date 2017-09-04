using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
	public Text playerAwesomeTotalDamageText;
	public Text playerSkillText;

	public Text enemyHitComboCountText;
	public Text enemyTotalDamageText;
	public Text enemyAwesomeTotalDamageText;
	public Text enemySkillText;

	public Image playerAwesomeIndicator;
	public Image enemyAwesomeIndicator;

	void Start ()
	{
		playerAwesomeIndicator.enabled = false;
		enemyAwesomeIndicator.enabled = false;
	}


	#region UPDATE PLAYER UI

	public void UpdatePlayerUI (bool isPlayer, PlayerModel player)
	{
		if (isPlayer) {
			playerNameText.text = player.name;
			playerHPText.text = player.hp.ToString ();
			playerHPBar.maxValue = player.hp;
			playerHPBar.value = player.hp;
			playerGPText.text = player.gp.ToString ();
			playerGPBar.maxValue = player.maxGP;
			playerGPBar.value = player.gp;
		} else {
			enemyNameText.text = player.name;
			enemyHPText.text = player.hp.ToString ();
			enemyHPBar.maxValue = player.hp;
			enemyHPBar.value = player.hp;
		}
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
		PlayerModel player = PlayerManager.GetPlayer (true);
		PlayerModel enemy = PlayerManager.GetPlayer (false);

		if (player.hp <= 0 || enemy.hp <= 0) {

			if (enemy.hp > 0 && player.hp <= 0) {
				ScreenBattleController.Instance.partAvatars.SetTriggerAnim (true, "lose");
				ScreenBattleController.Instance.partAvatars.SetTriggerAnim (false, "win");
			} else if (player.hp > 0 && enemy.hp <= 0) {
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
			BattleManager.ClearBattleData ();
			playerHitComboCountText.text = "";
			playerTotalDamageText.text = "";
			enemyTotalDamageText.text = "";
			enemyHitComboCountText.text = "";
			playerAwesomeTotalDamageText.text = "";
			enemyAwesomeTotalDamageText.text = "";
			Invoke ("StartPhase1", 1);
		}
	}

	IEnumerator CharacterActivateCoroutine (bool isPlayer)
	{
		//character skill interval
		while (CharacterManager.GetCharacterCount (isPlayer) > 0) {
			yield return new WaitForSeconds (2);
			CharacterManager.CharacterActivate (isPlayer);
		}

		yield return null;
	}

	IEnumerator CheckAttackCoroutine (bool isPlayer)
	{
		if (BattleManager.CheckAttack (isPlayer)) {
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
		string awesomeCount = "";
		QuestionResultCountModel playerAnswerParam = PlayerManager.GetQuestionResultCount (true);
		QuestionResultCountModel enemyAnswerParam = PlayerManager.GetQuestionResultCount (false);

		QuestionResultCountModel answerParam = null;
		if (isPLayer) {
			answerParam = playerAnswerParam;
		} else {
			answerParam = enemyAnswerParam;
		}

		int awesomeCounter = 0;
		for (int i = 0; i <= answerParam.correctCount; i++) {

			string attackAnimName = "attack" + (i % 3);
			//random animation
			ScreenBattleController.Instance.partAvatars.SetTriggerAnim (isPLayer, attackAnimName);

			//wait for attack animation to finish
			yield return StartCoroutine (AttackWaitAnimationCoroutine (isPLayer, attackAnimName));

			ScreenBattleController.Instance.partAvatars.SetTriggerAnim (!isPLayer, "hit1");
			SystemSoundController.Instance.PlaySFX ("SFX_HIT");

			if (isPLayer) {
				ScreenBattleController.Instance.partAvatars.LoadHitEffect (false);
				//load power effect in arms for every awesome count
				if (i < PlayerManager.GetQuestionResultCount (true).speedyAwesomeCount) {
					awesomeCounter++;
					ScreenBattleController.Instance.partAvatars.LoadArmPowerEffect (true);

					playerAwesomeTotalDamageText.text = awesomeCounter + " AWESOME";
					StartCoroutine (ShowAwesomeIndicatorCoroutine (true));
				}
				playerHitComboCountText.text = (i + 1) + " HIT COMBO";
				;
			} else {
				ScreenBattleController.Instance.partAvatars.LoadHitEffect (true);
				//load power effect in arms for every awesome count
				if (i < PlayerManager.GetQuestionResultCount (false).speedyAwesomeCount) {
					awesomeCounter++;
					ScreenBattleController.Instance.partAvatars.LoadArmPowerEffect (false);

					enemyAwesomeTotalDamageText.text = awesomeCounter + " AWESOME";
					StartCoroutine (ShowAwesomeIndicatorCoroutine (false));
				}

				enemyHitComboCountText.text = (i + 1) + " HIT COMBO";
			}

		}

		action ();
		totalDamageCount = attackDamage + " DAMAGE";
	
		if (isPLayer) {
			playerTotalDamageText.text = totalDamageCount;

		} else {
			enemyTotalDamageText.text = totalDamageCount;
	
		}

		yield return new WaitForSeconds (1);

	}

	IEnumerator AttackWaitAnimationCoroutine (bool isPlayer, string attackAnimName)
	{
		Animator anim = ScreenBattleController.Instance.partAvatars.GetPlayerAnimator (isPlayer);
		while (true){

			if (anim.GetCurrentAnimatorStateInfo (0).IsName (attackAnimName) &&
			    anim.IsInTransition (0) &&
			    anim.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
				
				Debug.Log ("waiting for animation to finish");
				break;
			}
			yield return null;
		}

		yield break;
	}


	IEnumerator ShowAwesomeIndicatorCoroutine (bool isPlayer)
	{
		if (isPlayer) {
			playerAwesomeIndicator.enabled = true;
			yield return new WaitForSeconds (1);
			playerAwesomeIndicator.enabled = false;
		} else {
			enemyAwesomeIndicator.enabled = true;
			yield return new WaitForSeconds (1);
			enemyAwesomeIndicator.enabled = false;
		}
	}

	public void ShowSkillIndicator (bool isPlayer, string skillText)
	{
		StartCoroutine (ShowSkillIndicatorCoroutine (isPlayer, skillText));
	}

	IEnumerator ShowSkillIndicatorCoroutine (bool isPlayer, string skillText)
	{
		if (isPlayer) {
			playerSkillText.text = skillText;
			yield return new WaitForSeconds (1);
			playerSkillText.text = "";
		} else {
			enemySkillText.text = skillText;
			yield return new WaitForSeconds (1);
			enemySkillText.text = "";
		}
	}

	private void StartPhase1 ()
	{
		ScreenBattleController.Instance.StartPhase1 ();
	}

	private void ResetPlayerDamage ()
	{
		PlayerManager.SetIsPlayer (true);
		PlayerManager.Player.td = 0;
	}

	#endregion

	#region TIMER Subscriber

	public Text preBattleTimerText;

	public void OnStartPreBattleTimer (int timer)
	{
		StartCoroutine (StartTimer (timer, delegate() {
			ScreenBattleController.Instance.StartPhase1 ();
		}));
	}

	public void OnStartCharacterSelectTimer (int timer, Action action)
	{
		preBattleTimerText.enabled = true;
		preBattleTimerText.text = "";
		StartCoroutine (StartTimer (timer, action));
	}


	IEnumerator StartTimer (int timer, Action action = null)
	{
		int timeLeft = timer;

		while (timeLeft > 0) {
			
			preBattleTimerText.text = timeLeft.ToString ();

			timeLeft--;
			yield return new WaitForSeconds (1);

		}

		action ();
		preBattleTimerText.enabled = false;
	}

	#endregion

}

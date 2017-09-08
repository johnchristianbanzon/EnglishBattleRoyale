using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System;

public class DamageResultsController : MonoBehaviour
{

	public Text hitComboCountText;
	public Text totalDamageText;
	public Image awesomeIndicator;

	void OnEnable(){
		totalDamageText.text = "";
		hitComboCountText.text = "";

		awesomeIndicator.enabled = false;
		totalDamageText.enabled = false;
		hitComboCountText.enabled = false;
	}

	public IEnumerator SetDamageResults (bool isPlayer, float attackDamage, QuestionResultCountModel answerParam, Action action)
	{
		totalDamageText.enabled = true;
		hitComboCountText.enabled = true;


		int awesomeCounter = 0;
		for (int i = 0; i <= answerParam.correctCount; i++) {

			string attackAnimName = "attack" + (i % 3);
			//random animation
			ScreenBattleController.Instance.partAvatars.SetTriggerAnim (isPlayer, attackAnimName);

			ScreenBattleController.Instance.partAvatars.LoadHitEffect (false);
			//load power effect in arms for every awesome count
			if (i < PlayerManager.GetQuestionResultCount (true).speedyAwesomeCount) {
				ScreenBattleController.Instance.partAvatars.LoadArmPowerEffect (true);
			}
			if ((i + 1) > 1) {
				hitComboCountText.text = "" + (i + 1) + "Hits";
			} else {
				hitComboCountText.text = "" + (i + 1) + "Hit";
			}


			;

			//wait for attack animation to finish
			yield return StartCoroutine (AttackWaitAnimationCoroutine (isPlayer, attackAnimName));


		}
			
		action ();
		yield return new WaitForSeconds (0.5f);
		totalDamageText.text = attackDamage + " DAMAGE";
		if (answerParam.correctCount >= 5) {
			yield return new WaitForSeconds (0.5f);
			yield return StartCoroutine (ShowAwesomeIndicatorCoroutine (true));
		}


		yield return new WaitForSeconds (1);
		Destroy (this.gameObject);

		
	}

	//wait for current attack animation to end before proceeding to next attack
	IEnumerator AttackWaitAnimationCoroutine (bool isPlayer, string attackAnimName)
	{
		Animator anim = ScreenBattleController.Instance.partAvatars.GetPlayerAnimator (isPlayer);
		while (true) {

			if (anim.GetCurrentAnimatorStateInfo (0).IsName (attackAnimName) &&
			    anim.GetCurrentAnimatorStateInfo (0).normalizedTime >= 0.9f) {

				ScreenBattleController.Instance.partAvatars.SetTriggerAnim (!isPlayer, "hit1");
				SystemSoundController.Instance.PlaySFX ("SFX_HIT");
				break;
			}
			yield return null;
		}
		yield break;
	}

	IEnumerator ShowAwesomeIndicatorCoroutine (bool isPlayer)
	{
		
		awesomeIndicator.enabled = true;
		yield return new WaitForSeconds (1);
		awesomeIndicator.enabled = false;
		
	}

}

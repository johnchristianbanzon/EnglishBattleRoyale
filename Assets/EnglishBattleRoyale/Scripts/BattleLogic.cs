using UnityEngine;

public class BattleLogic
{

	public static void AttackCompute (bool isPLayer, AttackModel attack)
	{
		if (isPLayer) {
			Debug.Log ("PLAYER DAMAGE: " + attack.attackDamage);
			ScreenBattleController.Instance.partState.enemy.playerHP -= attack.attackDamage;

			QuestionResultCountModel playerAnswerParam = GameManager.playerAnswerParam;
			if (playerAnswerParam.speedyAwesomeCount > 0) {
				ScreenBattleController.Instance.partAvatars.SetTriggerAnim (true, "attack2");
				ScreenBattleController.Instance.partAvatars.SetTriggerAnim (false, "hit2");
			} else {
				ScreenBattleController.Instance.partAvatars.SetTriggerAnim (true, "attack1");
				ScreenBattleController.Instance.partAvatars.SetTriggerAnim (false, "hit1");
			}
		


		} else {
			Debug.Log ("ENEMY DAMAGE: " + attack.attackDamage);
			ScreenBattleController.Instance.partState.player.playerHP -= attack.attackDamage;

			QuestionResultCountModel enemyAnswerParam = GameManager.enemyAnswerParam;
			if (enemyAnswerParam.speedyAwesomeCount > 0) {
				ScreenBattleController.Instance.partAvatars.SetTriggerAnim (false, "attack2");
				ScreenBattleController.Instance.partAvatars.SetTriggerAnim (true, "hit2");
			} else {
				ScreenBattleController.Instance.partAvatars.SetTriggerAnim (false, "attack1");
				ScreenBattleController.Instance.partAvatars.SetTriggerAnim (true, "hit1");
			}
		}
	}

}

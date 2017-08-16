using UnityEngine;

public class BattleLogic
{
	public static void AttackCompute (bool isPLayer, AttackModel attack)
	{
		ScreenBattleController.Instance.partState.StartBattleAnimation (isPLayer, attack.attackDamage, delegate() {
			if (isPLayer) {
				Debug.Log ("PLAYER DAMAGE: " + attack.attackDamage);
				ScreenBattleController.Instance.partState.enemy.playerHP -= attack.attackDamage;
			} else {
				Debug.Log ("ENEMY DAMAGE: " + attack.attackDamage);
				ScreenBattleController.Instance.partState.player.playerHP -= attack.attackDamage;
			}
		});
	}
}

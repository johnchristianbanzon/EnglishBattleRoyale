using UnityEngine;
using System.Collections;

public class BattleLogic
{
	public static IEnumerator AttackCompute (bool isPLayer, AttackModel attack)
	{
		yield return ScreenBattleController.Instance.partState.StartBattleAnimation (isPLayer, attack.attackDamage, delegate() {
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

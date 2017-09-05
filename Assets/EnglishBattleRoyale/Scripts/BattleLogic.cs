using UnityEngine;
using System.Collections;

public class BattleLogic
{
	public static IEnumerator AttackCompute (bool isPLayer, AttackModel attack)
	{
		yield return ScreenBattleController.Instance.partState.StartBattleAnimation (isPLayer, attack.attackDamage, delegate() {


			PlayerManager.SetIsPlayer(isPLayer);
			PlayerManager.Player.hp -= attack.attackDamage;


			if (isPLayer) {
				Debug.Log ("PLAYER DAMAGE: " + attack.attackDamage);

			} else {
				Debug.Log ("ENEMY DAMAGE: " + attack.attackDamage);
			}

			PlayerManager.UpdateStateUI(isPLayer);
		});
	}


}

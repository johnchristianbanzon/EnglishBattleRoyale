using UnityEngine;
using System.Collections;

public class BattleLogic
{
	public static IEnumerator AttackCompute (bool isPLayer, AttackModel attack)
	{
		yield return CharacterLogic.ActivateCharacters ();

		yield return ScreenBattleController.Instance.partState.StartBattleAnimation (isPLayer, attack.attackDamage, delegate() {

			PlayerManager.SetIsPlayer(!isPLayer);

			if (isPLayer) {
				Debug.Log ("BEFORE ENEMY HP: " + PlayerManager.Player.hp);

			} else {
				Debug.Log ("BEFORE PLAYER HP: " + PlayerManager.Player.hp);
			}


			PlayerManager.Player.hp -= attack.attackDamage;


			if (isPLayer) {
				Debug.Log ("PLAYER DAMAGE: " + attack.attackDamage);
				Debug.Log ("NOW ENEMY HP: " + PlayerManager.Player.hp);

			} else {
				Debug.Log ("ENEMY DAMAGE: " + attack.attackDamage);

				Debug.Log ("NOW PLAYER HP: " + PlayerManager.Player.hp);
			}
				
			PlayerManager.UpdateStateUI(!isPLayer);
		});
	}


}

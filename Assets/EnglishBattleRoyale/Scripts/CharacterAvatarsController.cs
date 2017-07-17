using UnityEngine;

public class CharacterAvatarsController :  SingletonMonoBehaviour<CharacterAvatarsController>
{
	public Animator playerAnim;
	public Animator enemyAnim;

	public void SetTriggerAnim(bool isPLayer, string param){
		if (isPLayer) {
			playerAnim.SetTrigger(param);
		} else {
			enemyAnim.SetTrigger(param);
		}
	}
}

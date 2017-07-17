using UnityEngine;

public class CharacterAnimationController :  SingletonMonoBehaviour<CharacterAnimationController>
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

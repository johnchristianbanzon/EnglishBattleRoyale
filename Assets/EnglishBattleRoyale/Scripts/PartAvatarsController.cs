using UnityEngine;

public class PartAvatarsController :  MonoBehaviour
{
	public Animator playerAnim;
	public Animator enemyAnim;


	public PlayerController player;
	public PlayerController enemy;

	#region Animation

	public void SetTriggerAnim (bool isPLayer, string param)
	{
		if (isPLayer) {
			playerAnim.SetTrigger (param);
		} else {
			enemyAnim.SetTrigger (param);
		}
	}

	public Animator GetPlayerAnimator (bool isPLayer)
	{
		if (isPLayer) {
			return playerAnim;
		} else {
			return enemyAnim;
		}
	}

	#endregion

	#region Player particle effects

	//effect when player get awesome answers
	public void LoadArmPowerEffect (bool isPlayer)
	{
		GameObject armPowerEffectL;
		GameObject armPowerEffectR;
		if (isPlayer) {
			armPowerEffectL = LoadEffect ("ArmPower", player.handContainerL);
			armPowerEffectR = LoadEffect ("ArmPower", player.handContainerR);
		} else {
			armPowerEffectL = LoadEffect ("ArmPower", enemy.handContainerL);
			armPowerEffectR = LoadEffect ("ArmPower", enemy.handContainerR);
		}

		Destroy (armPowerEffectL, 1);
		Destroy (armPowerEffectR, 1);
	}

	//effect when player is hit
	public void LoadHitEffect (bool isPlayer)
	{
		GameObject hitEffect;
		if (isPlayer) {
			hitEffect = LoadEffect ("Hit", player.hitContainer);
		} else {
			hitEffect = LoadEffect ("Hit", enemy.hitContainer);
		}


		Destroy (hitEffect, 1);
	}


	public float LoadCardSkillEffect (bool isPlayer, int particleID)
	{
		CharacterEnums.SkillEffect skillEffect = (CharacterEnums.SkillEffect)particleID;

		//put effect to enemy if the effect is meant for enemy
		if (skillEffect == CharacterEnums.SkillEffect.SkillSlash ||
		    skillEffect == CharacterEnums.SkillEffect.SkillPunch ||
		    skillEffect == CharacterEnums.SkillEffect.SkillPoison ||
		    skillEffect == CharacterEnums.SkillEffect.SkillBomb ||
			skillEffect == CharacterEnums.SkillEffect.SkillWarhammer ||
			skillEffect == CharacterEnums.SkillEffect.SkillSleep ||
			skillEffect == CharacterEnums.SkillEffect.SkillGreatSword
		
		) {

			isPlayer = !isPlayer;
		}


		GameObject skillEffectObject;
		if (isPlayer) {
			skillEffectObject = LoadEffect (skillEffect.ToString (), player.skillEffectContainer);
			return skillEffectObject.GetComponent<SkillEffectController> ().GetSkillDuration ();
		} else {

			skillEffectObject = LoadEffect (skillEffect.ToString (), enemy.skillEffectContainer);
			return skillEffectObject.GetComponent<SkillEffectController> ().GetSkillDuration ();
		}

	}


	private GameObject LoadEffect (string effectName, GameObject effectParent)
	{
		GameObject effect = SystemResourceController.Instance.LoadPrefab ("ParticleEffects/" + effectName, effectParent);
		return effect;
	}

	#endregion
}

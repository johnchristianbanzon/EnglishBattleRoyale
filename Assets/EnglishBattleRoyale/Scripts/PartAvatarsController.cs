using UnityEngine;

public class PartAvatarsController :  MonoBehaviour
{
	public Animator playerAnim;
	public Animator enemyAnim;


	public PlayerController player;
	public PlayerController enemy;

	public void SetTriggerAnim (bool isPLayer, string param)
	{
		if (isPLayer) {
			playerAnim.SetTrigger (param);
		} else {
			enemyAnim.SetTrigger (param);
		}
	}

	#region Player particle effects


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


	public void LoadCardSkillEffect (bool isPlayer, int charID)
	{
		//remove later if complete character effects
		if (charID > 8) {
			return;
		}

		CharacterEnums.SkillEffect skillEffect = (CharacterEnums.SkillEffect)charID;

		//put effect to enemy if the effect is meant for enemy
		if (skillEffect == CharacterEnums.SkillEffect.SkillSlash ||
		    skillEffect == CharacterEnums.SkillEffect.SkillPunch ||
		    skillEffect == CharacterEnums.SkillEffect.SkillPoison ||
		    skillEffect == CharacterEnums.SkillEffect.SkillBomb) {

			isPlayer = !isPlayer;
		}


		GameObject skillEffectObject;
		if (isPlayer) {
			skillEffectObject = LoadEffect (skillEffect.ToString (), player.skillEffectContainer);
		} else {

			skillEffectObject = LoadEffect (skillEffect.ToString (), enemy.skillEffectContainer);
		}

		Destroy (skillEffectObject, 1);
	}


	private GameObject LoadEffect (string effectName, GameObject effectParent)
	{
		GameObject effect = SystemResourceController.Instance.LoadPrefab ("ParticleEffects/" + effectName, effectParent);
		return effect;
	}

	#endregion
}

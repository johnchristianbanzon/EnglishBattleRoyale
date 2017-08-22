using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject handContainerL;
	public GameObject handContainerR;

	public GameObject hitContainer;

	private GameObject armPowerEffectL;
	private GameObject armPowerEffectR;

	public void LoadArmPowerEffect(){
		armPowerEffectL = LoadEffect ("ArmPower", handContainerL);
		armPowerEffectR = LoadEffect ("ArmPower", handContainerR);
		Destroy (armPowerEffectL, 1);
		Destroy (armPowerEffectR, 1);
	}

	public void LoadHitEffect(){
		GameObject hitEffect = LoadEffect ("Hit", hitContainer);
		Destroy (hitEffect, 1);
	}


	private GameObject LoadEffect(string effectName, GameObject effectParent){
		GameObject effect = SystemResourceController.Instance.LoadPrefab ("ParticleEffects/" + effectName, effectParent);
		return effect;
	}
}

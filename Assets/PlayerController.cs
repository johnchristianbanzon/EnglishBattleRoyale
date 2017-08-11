using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject handContainerL;
	public GameObject handContainerR;

	public GameObject hitContainer;

	private GameObject armPowerEffectL;
	private GameObject armPowerEffectR;
	private GameObject hitEffect;


	public void LoadArmPowerEffect(){
		armPowerEffectL = LoadEffect ("ArmPower", handContainerL);
		armPowerEffectR = LoadEffect ("ArmPower", handContainerR);
	}

	public void UnLoadArmPowerEffect(){
		Destroy (armPowerEffectL);
		Destroy (armPowerEffectR);
	}

	public void LoadHitEffect(){
		hitEffect = LoadEffect ("Hit", hitContainer);
		Destroy (hitEffect, 1);
	}


	private GameObject LoadEffect(string effectName, GameObject effectParent){
		GameObject effect = SystemResourceController.Instance.LoadPrefab ("ParticleEffects/" + effectName, effectParent);
		return effect;
	}
}

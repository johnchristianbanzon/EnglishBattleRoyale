using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject handContainerL;
	public GameObject handContainerR;

	public ParticleSystem skillAura;

	public GameObject hitContainer;

	private GameObject armPowerEffectL;
	private GameObject armPowerEffectR;



	public void LoadArmPowerEffect(){
		armPowerEffectL = LoadEffect ("ArmPower", handContainerL);
		armPowerEffectR = LoadEffect ("ArmPower", handContainerR);
	}

	public void UnLoadArmPowerEffect(){
		Destroy (armPowerEffectL);
		Destroy (armPowerEffectR);
	}

	public void LoadHitEffect(){
		GameObject hitEffect = LoadEffect ("Hit", hitContainer);
		Destroy (hitEffect, 1);
	}

	public void LoadSkillAuraEffect(int skillID){
		ParticleSystem.MainModule main = skillAura.main;
		Color skillColor = Color.black;
		switch (skillID) {
		case 1:
			skillColor = Color.yellow;
			break;
		case 2:
			skillColor = Color.blue;
			break;
		case 3:
			skillColor = Color.red;
			break;
		case 4:
			skillColor = Color.green;
			break;
		}

		main.startColor = skillColor;
		ShowSkillAura ();
	}

	private void ShowSkillAura(){
		skillAura.gameObject.SetActive (true);
		Invoke ("HideSkillAura", 1);
	}

	private void HideSkillAura(){
		skillAura.gameObject.SetActive (false);
	}


	private GameObject LoadEffect(string effectName, GameObject effectParent){
		GameObject effect = SystemResourceController.Instance.LoadPrefab ("ParticleEffects/" + effectName, effectParent);
		return effect;
	}
}

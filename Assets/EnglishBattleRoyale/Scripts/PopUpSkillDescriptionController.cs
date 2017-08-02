using UnityEngine.UI;
using UnityEngine;

public class PopUpSkillDescriptionController : MonoBehaviour {

	public Text skillDescriptionText;

	public void SkillDescription(string skillDescription){
		skillDescriptionText.text = skillDescription;
	}

	public void CloseDescription ()
	{
		Destroy (this.gameObject);
	}

}

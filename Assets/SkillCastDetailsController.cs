using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillCastDetailsController : MonoBehaviour {

	public Text skillDetailNameText;
	public Text skillDetailCalculationText;

	public IEnumerator SkillDetailCoroutine (CharacterModel character)
	{
		skillDetailNameText.text = character.name;
		skillDetailCalculationText.text  = character.effectDescription;
		yield return new WaitForSeconds (2);
		Destroy (this.gameObject);
	}
}

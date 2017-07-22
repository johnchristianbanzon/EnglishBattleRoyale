using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedSkillController : MonoBehaviour
{
	private List<SkillModel> equipCardList = new List<SkillModel> ();
	private List<GameObject> cardsObject = new List<GameObject> ();
	public UnlockedSkillController unlockedSkill;
	 

	// Use this for initialization
	void Start ()
	{
		equipCardList = SkillManager.GetEquipSkillList ();

		for (int i = 0; i < equipCardList.Count; i++) {
			if (this.transform.GetChild (i).childCount == 0) {
				GameObject charCard = SystemResourceController.Instance.LoadPrefab ("CharCard", this.transform.GetChild (i).gameObject);
				charCard.GetComponent<CharCardController> ().SetCardParameter (equipCardList [i],true);
				cardsObject.Add (charCard.gameObject);
	
			}
		}

		unlockedSkill.ShowCharacterCards (equipCardList);
		SkillManager.SetSkillEnqueue (equipCardList);
	}

	public void InitiateSwapping ()
	{
		ShakeSkillCards ();
	}

	public void ShakeSkillCards ()
	{
		foreach (GameObject card in cardsObject) {
			TweenFacade.TweenDoPunchRotation (card.transform, 0.5f, new Vector3 (0, 0, 1), 10, 1f);
		}
	}

}

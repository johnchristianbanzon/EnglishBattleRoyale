using System.Collections.Generic;
using UnityEngine;

public class EquippedSkillController : MonoBehaviour {

	private List<SkillModel> equipCardList = new List<SkillModel> ();
	public UnlockedSkillController unlockedSkill;

	// Use this for initialization
	void Start ()
	{
		equipCardList = SkillManager.GetEquipSkillList ();

		for (int i = 0; i < equipCardList.Count; i++) {
			if (this.transform.GetChild (i).childCount == 0) {
				GameObject charCard = SystemResourceController.Instance.LoadPrefab ("CharCard", this.transform.GetChild (i).gameObject);
				charCard.GetComponent<CharCardController> ().SetCardParameter (equipCardList [i]);
			}
		}

		unlockedSkill.ShowCharacterCards (equipCardList);
		SkillManager.SetSkillEnqueue (equipCardList);
	}
}

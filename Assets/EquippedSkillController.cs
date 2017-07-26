using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedSkillController : MonoBehaviour
{
	private List<SkillModel> equipCardList = new List<SkillModel> ();
	private List<GameObject> cardsObject = new List<GameObject> ();
	public UnlockedSkillController unlockedSkill;

	void Start ()
	{
		equipCardList = SkillManager.GetEquipSkillList ();

		for (int i = 0; i < equipCardList.Count; i++) {
			if (this.transform.GetChild (i).childCount == 0) {
				GameObject charCard = SystemResourceController.Instance.LoadPrefab ("CharCard", this.transform.GetChild (i).gameObject);
				charCard.GetComponent<CharCardController> ().SetCardParameter (equipCardList [i], true);
				cardsObject.Add (charCard.gameObject);
			}
		}
		unlockedSkill.ShowCharacterCards (equipCardList);
		SkillManager.SetSkillEnqueue (equipCardList);
	}

	public void ShakeSkillCards ()
	{
		foreach (GameObject card in cardsObject) {
			TweenFacade.TweenDoPunchRotation (card.transform, 0.5f, new Vector3 (0, 0, 1), 10, 1f);
		}
	}

	//TEMPORARY SOLUTION... DO NOT DEPEND ON HEIRARCHY
	private void UpdateEquipCardList ()
	{
		equipCardList.Clear ();
		for (int i = 0; i < this.transform.childCount; i++) {
			equipCardList.Add (this.transform.GetChild (i).GetChild (0).GetComponent<CharCardController> ().GetCardParameter ());
		}
	}

	public void UpdateSkillList ()
	{
		UpdateEquipCardList ();
		SkillManager.SetSkillEnqueue (equipCardList);
	}

}

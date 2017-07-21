using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnlockedSkillController : MonoBehaviour
{
	private List<SkillModel> charCardList = new List<SkillModel> ();
	public Text charCardCount;
	public GameObject unlockedSkillTitle;
	public GameObject currentSelectedCardSlot;
	public GameObject currectSelectedCharacterCard;
	// Show unlocked cards, if cards already in equip, do not show
	public void ShowCharacterCards (List<SkillModel> equipCardList)
	{
		charCardList = SkillManager.GetCSVSkillList ();
		charCardCount.text = charCardList.Count.ToString () + "/20";
		for (int i = 0; i < charCardList.Count; i++) {
			bool hasCardInEquip = false;

			for (int j = 0; j < equipCardList.Count; j++) {
				if (equipCardList [j].skillName.Equals (charCardList [i].skillName)) {
					hasCardInEquip = true;
					break;
				} 
			}

			if (!hasCardInEquip) {
				for (int k = 0; k < this.transform.childCount; k++) {
					if (this.transform.GetChild (k).childCount == 0) {
						GameObject charCard = SystemResourceController.Instance.LoadPrefab ("CharCard", this.transform.GetChild (k).gameObject);
						charCard.GetComponent<CharCardController> ().SetCardParameter (charCardList[i]);
						charCard.GetComponent<Button>	().onClick.AddListener (() => {
							OnClickCharacterCard(charCard.GetComponent<Button>());
						});
						break;
					}
				}
			}
			hasCardInEquip = false;
		}
			
	}
	public void OnClickCharacterCard(Button clickedCharacterCard){
		currentSelectedCardSlot = clickedCharacterCard.transform.parent.gameObject;
		currectSelectedCharacterCard = clickedCharacterCard.gameObject;
		GameObject selectionPrefab = SystemResourceController.Instance.LoadPrefab ("SkillSelectPrefab", clickedCharacterCard.gameObject);
		selectionPrefab.transform.SetAsFirstSibling ();
		selectionPrefab.transform.position = clickedCharacterCard.transform.position;
		GameObject useButton = selectionPrefab.transform.GetChild (2).gameObject;
		useButton.GetComponent<Button> ().onClick.AddListener (() => {
			OnClickCharacterUse(clickedCharacterCard.gameObject);
		});
		TweenFacade.TweenScaleToLarge (selectionPrefab.transform.parent.transform,Vector3.one,0.3f);
	}

	public void OnClickCharacterUse(GameObject characterUsed){
		Destroy (characterUsed.transform.GetChild (0).gameObject);
		characterUsed.transform.parent = unlockedSkillTitle.transform.parent;
		TweenFacade.TweenMoveTo (characterUsed.transform,unlockedSkillTitle.transform.localPosition,0.3f);
		PartDeckController.Instance.equippedSkillController.InitiateSwapping ();
	}

}

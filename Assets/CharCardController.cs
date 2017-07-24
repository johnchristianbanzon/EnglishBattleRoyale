using UnityEngine;
using UnityEngine.UI;

public class CharCardController : MonoBehaviour
{
	private Text gpCost;
	private Image skillImage;
	private SkillModel charCard;
	private GameObject charCardSettings;
	private bool isEquipped = false;
	private GameObject infoButton;
	private GameObject useButton;
	private bool isInsideCard = false;

	public void SetCardParameter (SkillModel charCard, bool isEquipped)
	{
		this.charCard = charCard;
		this.gpCost.text = charCard.skillGpCost.ToString();
		skillImage.sprite = SystemResourceController.Instance.LoadCharacterCardSprite (charCard.skillName);
		this.isEquipped = isEquipped;
	}

	public void InfoButton(){
		GameObject popUpSkillOverview = SystemPopupController.Instance.ShowPopUp ("PopUpSkillOverview");
		popUpSkillOverview.GetComponent<PopUpSkillOverviewController> ().SetCharCard (charCard);
	}

	public void UseButton(){
		HideCardSettings ();
		this.transform.parent = PartDeckController.Instance.unlockedSkillController.unlockedSkillTitle.transform;
		TweenFacade.TweenMoveTo (this.transform,Vector3.zero,0.6f);
		PartDeckController.Instance.equippedSkillController.InitiateSwapping ();
		PartDeckController.Instance.unlockedSkillController.aboutToSwapCard = true;
	}
	public void OnClickCharacterCard(GameObject clickedCard){
		ShowCardSettings (isEquipped);
		if (!PartDeckController.Instance.unlockedSkillController.aboutToSwapCard) {
			PartDeckController.Instance.unlockedSkillController.aboutToSwapCard = false;
			PartDeckController.Instance.unlockedSkillController.currectSelectedCharacterCard = this.gameObject;
			PartDeckController.Instance.unlockedSkillController.currentSelectedCardSlot = this.gameObject.transform.parent.gameObject;
		} else {
			charCardSettings.SetActive (false);
			ReplaceEquippedCharacter (PartDeckController.Instance.unlockedSkillController.currectSelectedCharacterCard.gameObject);
			PartDeckController.Instance.unlockedSkillController.aboutToSwapCard = false;
			TweenFacade.StopTweens ();
		}
	}

	public void ShowCardSettings(bool equipped){
		charCardSettings.SetActive (true);
		if (equipped) {
			useButton.SetActive (false);
		}
	}

	public void ReplaceEquippedCharacter(GameObject selectedCharacter){
		selectedCharacter.transform.parent = this.transform.parent;
		isEquipped = false;
		selectedCharacter.GetComponent<CharCardController> ().isEquipped = true;
		useButton.SetActive (true);
		this.transform.parent = PartDeckController.Instance.unlockedSkillController.currentSelectedCardSlot.transform;

	}


	public void OnPointerEnter(){
		isInsideCard = true;
	}

	public void OnPointerExit(){
		isInsideCard = false;
	}

	void Update(){
		if (Input.GetMouseButtonDown(0) && isInsideCard == false) {
				HideCardSettings ();
		}
	}

	public void HideCardSettings(){
		charCardSettings.SetActive (false);
		if (!isEquipped) {
			useButton.SetActive (true);
		}
	}
}

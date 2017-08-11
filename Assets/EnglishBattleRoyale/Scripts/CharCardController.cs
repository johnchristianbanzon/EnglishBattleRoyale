using UnityEngine;
using UnityEngine.UI;

public class CharCardController : MonoBehaviour
{
	public Text gpCost;
	public Image characterImage;
	private CharacterModel charCard;
	public GameObject charCardSettings;
	private bool isEquipped = false;
	public GameObject infoButton;
	public GameObject useButton;
	private bool isInsideCard = false;

	public static GameObject currentSelectedCardSlot;
	public static GameObject currectSelectedCharacterCard;
	public static bool isSwappable = false;

	public CharacterModel GetCardParameter(){
		return charCard;
	}

	public void SetCardParameter (CharacterModel charCard, bool isEquipped)
	{
		this.charCard = charCard;
		this.gpCost.text = charCard.characterGPCost.ToString();
		characterImage.sprite = SystemResourceController.Instance.LoadCharacterCardSprite (charCard.characterID);
		this.isEquipped = isEquipped;
	}

	public void InfoButton(){
		GameObject popUpSkillOverview = SystemPopupController.Instance.ShowPopUp ("PopUpCharacterOverview");
		popUpSkillOverview.GetComponent<PopUpCharacterOverviewController> ().SetCharCard (charCard);
	}

	public void UseButton(){
		HideCardSettings ();
		TweenFacade.TweenMoveTo (this.transform,Vector3.zero,0.6f);
		PartDeckController.Instance.equippedSkillController.ShakeSkillCards ();
		isSwappable = true;
	}

	public void OnClickCharacterCard(GameObject clickedCard){
		ShowCardSettings (isEquipped);
		if (!isSwappable) {
			currectSelectedCharacterCard = this.gameObject;
			currentSelectedCardSlot = this.gameObject.transform.parent.gameObject;
		} else {
			charCardSettings.SetActive (false);
			ReplaceEquippedCharacter (currectSelectedCharacterCard.gameObject);
			isSwappable = false;
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
		if (isEquipped) {
			selectedCharacter.transform.parent = this.transform.parent;
			isEquipped = false;
			selectedCharacter.GetComponent<CharCardController> ().isEquipped = true;
			useButton.SetActive (true);
			this.transform.parent = currentSelectedCardSlot.transform;
			PartDeckController.Instance.equippedSkillController.UpdateSkillList ();
		}
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

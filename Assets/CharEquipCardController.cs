using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharEquipCardController : MonoBehaviour
{
	private Vector2 startPos;
	public Text charGpCost;
	public Image charImage;
	public Button charButton;
	private CharacterModel charCard;


	public void OnEndDrag ()
	{
		charImage.raycastTarget = true;
		this.transform.position = startPos;
	}

	public void SetCharacter (CharacterModel charCard)
	{
		this.charCard = charCard;
		charImage.sprite = SystemResourceController.Instance.LoadCharacterCardSprite (charCard.characterID);
		charGpCost.text = charCard.characterGPCost.ToString ();
	}

	public void ShowCharacterDescription (int skillNumber)
	{
		GameObject characterDescription = SystemPopupController.Instance.ShowPopUp ("PopUpCharacterOverview");
		characterDescription.GetComponent<PopUpCharacterOverviewController> ().SetCharCard (charCard);
	}

	public void ToggleButtonInteractable (bool toggle)
	{
		charButton.interactable = toggle;
	}

	public void OnBeginDrag ()
	{
		startPos = this.transform.position;
		charImage.raycastTarget = false;
	}

	public void OnDrag ()
	{
		Vector2 pos = new Vector2 (0, 0);
		Canvas myCanvas = SystemGlobalDataController.Instance.gameCanvas;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
		this.transform.position = myCanvas.transform.TransformPoint (pos);

	}


}

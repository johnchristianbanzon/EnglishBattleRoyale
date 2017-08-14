using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharEquipCardController : MonoBehaviour
{
	private Vector2 startPos;
	public Text charGpCost;
	public Image charImage;
	public Button charButton;
	public CharacterModel charCard;
	private GameObject selectedObject;
	private static int selectedIndex = 0;
	public GameObject characterLayout;
	private static GameObject containerPlacer;
	private static bool isDragging = false;
	private static CharacterModel[] charArray = new CharacterModel[3];


	public void OnEndDrag ()
	{
		charImage.raycastTarget = true;
		selectedObject.transform.SetParent (characterLayout.transform);
		selectedObject.transform.SetSiblingIndex (selectedIndex);
		Destroy (containerPlacer);
		isDragging = false;
		this.transform.position = startPos;

	
		Invoke ("SendNewCharOrder", 0.1f);
	}

	private void SendNewCharOrder ()
	{
		for (int i = 0; i < charArray.Length; i++) {
			charArray [i] = this.transform.parent.GetChild (i).GetComponent<CharEquipCardController> ().charCard;
		}

		CharacterManager.SetCharacterOrder (charArray);
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
		selectedObject = this.gameObject;
		startPos = this.transform.position;
		selectedObject.transform.SetParent (characterLayout.transform.parent);
		containerPlacer = SystemResourceController.Instance.LoadPrefab ("Input-UI", characterLayout);
		charImage.raycastTarget = false;
		isDragging = true;
	}

	public void OnPointerEnter ()
	{
		if (isDragging) {
			selectedIndex = transform.GetSiblingIndex ();
			containerPlacer.transform.SetSiblingIndex (selectedIndex);
		}
	}

	public void OnDrag ()
	{
		Vector2 pos = new Vector2 (0, 0);
		Canvas myCanvas = SystemGlobalDataController.Instance.gameCanvas;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
		this.transform.position = myCanvas.transform.TransformPoint (pos);

	}


}

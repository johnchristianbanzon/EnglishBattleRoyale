using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class CharEquipCardController : MonoBehaviour
{
	private Vector2 startPos;
	public Text charGpCost;
	public Image charGPContainer;
	public Image charImage;
	public Button charButton;
	private CharacterModel charCard;
	private GameObject selectedObject;
	private static int selectedIndex = 0;
	public GameObject characterLayout;
	private static GameObject containerPlacer;
	private static bool isDragging = false;
	private static CharacterModel[] charArray = new CharacterModel[3];
	private bool isTap = false;
	private Coroutine checkTap;

	#region CHARACTER DATA

	public void SetCharacter (CharacterModel charCard)
	{
		this.charCard = charCard;
		charImage.sprite = SystemResourceController.Instance.LoadCharacterCardSprite (charCard.characterID);
		charGpCost.text = charCard.characterGPCost.ToString ();

		Debug.Log ("SETTING NEW CHARACTER");
		NewCardAnimation ();
	}

	public CharacterModel GetCharacter(){
		return charCard;
	}

	public void ShowCharacterDescription (int skillNumber)
	{
		GameObject characterDescription = SystemPopupController.Instance.ShowPopUp ("PopUpCharacterOverview");
		characterDescription.GetComponent<PopUpCharacterOverviewController> ().SetCharCard (charCard);
	}

	public void InfoButton(){
		GameObject popUpSkillOverview = SystemPopupController.Instance.ShowPopUp ("PopUpCharacterOverview");
		popUpSkillOverview.GetComponent<PopUpCharacterOverviewController> ().SetCharCard (charCard);
	}

	public void OnPointerDown(){
		checkTap = StartCoroutine (CheckTapTimeCoroutine());

	}

	public void OnPointerUp(){
		if (isTap) {
			InfoButton ();
		}
		StopCoroutine (checkTap);
	}

	IEnumerator CheckTapTimeCoroutine(){
		isTap = true;
		yield return new WaitForSeconds (0.2f);
		isTap = false;
	}

	#endregion

	#region CHANGE CARD ORDER

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

	private CharacterModel[] EquipCards(){
		CharacterModel[] charEquipCard =   new CharacterModel[3];
		for (int i = charArray.Length-1; i >= 0; i--) {
			charEquipCard[i] = this.transform.parent.GetChild (i).GetComponent<CharEquipCardController> ().GetCharacter();
		}
		return charEquipCard;
	}

	private void SendNewCharOrder ()
	{
		for (int i = 0; i < EquipCards ().Length; i++) {
			charArray[i] = EquipCards () [i];
		}
		CharacterManager.SetCharacterOrder (charArray);

		//Checks hierarchy of character UI and updates it's index
		ScreenBattleController.Instance.partCharacter.SetCharacterOrder ();
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

	#endregion


	#region CARD ANIMATION

	public void NewCardAnimation(){
		this.transform.localScale = Vector3.zero;
		TweenFacade.TweenNewCharacterCard (this.transform);
	}

	public void ActivateCardAnimation(){
		TweenFacade.TweenActivateCharacterCard (this.transform);
	}


	#endregion


}

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
	private bool isTap = false;
	private Coroutine checkTap;
	private bool isShowInfo = false;
	private bool isCardUsed = false;
	public Text priorityNumberText;
	public Image useEffect;
	private int priorityNumber = 0;

	private GameObject popUpSkillOverview;



	void Start(){
		useEffect.enabled = false;
		priorityNumberText.enabled = false;
	}

	#region CHARACTER DATA

	public void SetCharacter (CharacterModel charCard)
	{
		this.charCard = charCard;
		charImage.sprite = SystemResourceController.Instance.LoadCharacterCardSprite (charCard.iD);
		charGpCost.text = charCard.gpCost.ToString ();

		NewCardAnimation ();
	}

	public void InfoButton ()
	{
		isShowInfo = !isShowInfo;

		if (isShowInfo) {
			popUpSkillOverview = SystemPopupController.Instance.ShowPopUp ("PopUpCharacterOverview");
			popUpSkillOverview.GetComponent<PopUpCharacterOverviewController> ().SetCharCard (charCard, false);
		} else {
			Destroy (popUpSkillOverview);
		}
	}

	//if not enough gp, character is not interactable
	void Update ()
	{
		if (charCard != null) {
			if (ScreenBattleController.Instance.partState.playerGPBar.value >= charCard.gpCost) {
				charButton.interactable = true;
			} else {
				charButton.interactable = false;
			}

			if (isCardUsed) {
				charButton.interactable = true;
			}
		}
	}
		
	//deduct or add gp bar when trying to use or not use character
	public void UseCharacter ()
	{
		if (isCardUsed == false) {
//			ScreenBattleController.Instance.partState.playerGPBar.value -= charCard.gpCost;
			useEffect.enabled = true;
			priorityNumberText.enabled = true;
			ScreenBattleController.Instance.partCharacter.priorityNumberList.Add (this);

		} else {
//			ScreenBattleController.Instance.partState.playerGPBar.value += charCard.gpCost;
			useEffect.enabled = false;
			priorityNumberText.enabled = false;
			Debug.Log (priorityNumber);
			ScreenBattleController.Instance.partCharacter.priorityNumberList.RemoveAt (priorityNumber);
			priorityNumber = 0;

		}
		ScreenBattleController.Instance.partCharacter.UpdateCharCardPriority ();
		isCardUsed = !isCardUsed;

	}

	public void UpdatePriorityNumber(){
		if (ScreenBattleController.Instance.partCharacter.priorityNumberList.Count > 0) {
			for (int i = 0; i < ScreenBattleController.Instance.partCharacter.priorityNumberList.Count; i++) {
				if (ScreenBattleController.Instance.partCharacter.priorityNumberList [i].Equals (this)) {
					priorityNumber = i;

					priorityNumberText.text = "" + (priorityNumber + 1);
				}
			}
		}
	}

	public void OnPointerDown ()
	{
		checkTap = StartCoroutine (CheckTapTimeCoroutine ());

	}

	//show character overview if button not tapped, else activate skill
	public void OnPointerUp ()
	{
		if (isTap) {
			UseCharacter ();
		} else {
			InfoButton ();
		}
		StopCoroutine (checkTap);
	}

	IEnumerator CheckTapTimeCoroutine ()
	{
		isTap = true;
		yield return new WaitForSeconds (0.2f);
		isTap = false;
	}

	#endregion


	#region CARD ANIMATION

	public void NewCardAnimation ()
	{
		this.transform.localScale = Vector3.zero;
		TweenFacade.TweenNewCharacterCard (this.transform);
	}

	public void ActivateCardAnimation ()
	{
		TweenFacade.TweenActivateCharacterCard (this.transform);
	}


	#endregion


}

﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

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

	private bool isInterActable = false;

	private GameObject popUpSkillOverview;

	public void SetIsInterActable (bool isInterActable)
	{
		this.isInterActable = isInterActable;
		charButton.interactable = isInterActable;
	}

	public void ResetCardUsed ()
	{
		useEffect.enabled = false;
		isCardUsed = false;
		priorityNumber = 0;
	}

	public void CheckCard (Action<bool, int> onResult)
	{
		onResult (isCardUsed, priorityNumber);
	}


	void Start ()
	{
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

		StopCoroutine (checkTap);
	}

	//if not enough gp, character is not interactable
	void Update ()
	{
		if (isInterActable) {
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
		} else {
			charButton.interactable = false;
		}
	}
		
	//deduct or add gp bar when trying to use or not use character
	public void UseCharacter ()
	{
		if (isCardUsed == false) {
			ScreenBattleController.Instance.partState.playerGPBar.value -= charCard.gpCost;
			useEffect.enabled = true;
			priorityNumberText.enabled = true;
			ScreenBattleController.Instance.partCharacter.priorityNumberList.Add (this);

		} else {
			ScreenBattleController.Instance.partState.playerGPBar.value += charCard.gpCost;
			useEffect.enabled = false;
			priorityNumberText.enabled = false;
			Debug.Log (priorityNumber);
			ScreenBattleController.Instance.partCharacter.priorityNumberList.RemoveAt (priorityNumber);
			priorityNumber = 0;

		}
		ScreenBattleController.Instance.partCharacter.UpdateCharCardPriority ();
		isCardUsed = !isCardUsed;
		StopCoroutine (checkTap);
	}

	public void UpdatePriorityNumber ()
	{
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
		if (isInterActable) {
			checkTap = StartCoroutine (CheckTapTimeCoroutine ());
		}
	}

	//show character overview if button not tapped, else activate skill
	public void OnPointerUp ()
	{
		if (isInterActable) {
			if (isTap) {
				UseCharacter ();
			} 

		}
	}

	IEnumerator CheckTapTimeCoroutine ()
	{
		isTap = true;
		yield return new WaitForSeconds (0.2f);
		isTap = false;
		InfoButton ();
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

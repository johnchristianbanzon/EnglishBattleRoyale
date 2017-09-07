﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PartCharacterController : MonoBehaviour
{
	public GameObject charCardsContainer;
	private CharEquipCardController[] charCards = new CharEquipCardController[3];
	public List<CharEquipCardController> priorityNumberList{ get; set; }
	void Start ()
	{
		priorityNumberList = new List<CharEquipCardController>(3);
		ShowCharacters (false);
		SetCharacterOrder ();
		 
		//Set starting skills during start of battle
		CharacterManager.SetStartCharacters ();

	}

	public void UpdateCharCardPriority(){
		for (int i = 0; i < charCards.Length; i++) {
			charCards [i].UpdatePriorityNumber ();
		}
	}

	public CharEquipCardController[] GetCharCards(){
		return charCards;
	}

	public void OnStartPhase ()
	{
		//Pick character time
		TimeManager.StartCharacterSelectTimer (5, StartCharacterPhase);
	}

	private void StartCharacterPhase ()
	{
		//Check toggle on characters on start of the phase and send it
		CharacterManager.StartCharacters ();
		PartAnswerIndicatorController.Instance.ResetAnswer ();
	}

	public void OnEndPhase ()
	{
		//Hide character selection
		ShowCharacters(false);
		for (int i = 0; i < charCards.Length; i++) {
			charCards [i].ResetCardUsed ();
		}
	}

	public void SetCharacterUI (int characterNumber, CharacterModel charCard)
	{
		charCards [characterNumber].SetCharacter (charCard);
	
	}


	public void ActivateCharacterUI (int characterNumber)
	{
		charCards [characterNumber].ActivateCardAnimation ();
	}

	public void SetCharacterOrder ()
	{
		for (int i = 0; i < charCards.Length; i++) {
			charCards [i] = charCardsContainer.transform.GetChild (i).GetComponent<CharEquipCardController> ();
		}
	}

	public void ShowCharacters (bool isShow)
	{
		if (isShow) {
			for (int i = 0; i < charCards.Length; i++) {
				charCards [i].SetIsInterActable (true);
			}
		} else {
			for (int i = 0; i < charCards.Length; i++) {
				charCards [i].SetIsInterActable (false);
			}
		}
	}

		
	#region COROUTINES

	public void ChangeCharacterCard (Action removeCard, Action newCard)
	{
		StartCoroutine (ChangeCharacterCardCoroutine (removeCard, newCard));
		
	}

	IEnumerator ChangeCharacterCardCoroutine (Action removeCard, Action newCard)
	{
		removeCard ();
		yield return new WaitForSeconds (1);
		newCard ();
	}

	#endregion
		
}

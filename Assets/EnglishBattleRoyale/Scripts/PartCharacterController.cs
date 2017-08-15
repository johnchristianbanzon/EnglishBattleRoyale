﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PartCharacterController : MonoBehaviour
{
	public CharEquipCardController[] charCards;

	void Start ()
	{
		//Set starting skills during start of battle
		CharacterManager.SetStartCharacters ();
	}

	public void OnStartPhase ()
	{
		//Check toggle on characters on start of the phase and send it
		CharacterManager.StartCharacters ();
		ShowAutoActivateButtons (false);
		PartAnswerIndicatorController.Instance.ResetAnswer ();

	}

	//show skill buttons after attack phase is done
	public void OnEndPhase ()
	{
		ShowAutoActivateButtons (true);
	}

	public void SetCharacterUI (int characterNumber, CharacterModel charCard)
	{
		charCards [characterNumber].SetCharacter (charCard);
	
	}

	public void ActivateCharacterUI (int characterNumber)
	{
		charCards [characterNumber].ActivateCardAnimation();
	}

	private void OnEndQuestionTime ()
	{
		ButtonEnable (false);
	}

	public void ShowAutoActivateButtons (bool isShow)
	{
		ButtonEnable (isShow);

	}

	public void ButtonEnable (bool buttonEnable)
	{
		charCards [0].ToggleButtonInteractable (buttonEnable);
		charCards [1].ToggleButtonInteractable (buttonEnable);
		charCards [2].ToggleButtonInteractable (buttonEnable);
	}

	#region COROUTINES

	public void ChangeCharacterCard(Action removeCard, Action newCard){
		StartCoroutine (ChangeCharacterCardCoroutine(removeCard,newCard));
		
	}

	IEnumerator ChangeCharacterCardCoroutine(Action removeCard, Action newCard){
		removeCard ();
		yield return new WaitForSeconds (1);
		newCard ();
	}

	#endregion
		
}

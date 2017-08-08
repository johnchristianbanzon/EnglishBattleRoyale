using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PartCharacterController : MonoBehaviour
{
	public Button[] characterButton;
	private bool[] characterButtonToggleOn = new bool[3];

	public Text[] skillGpCost;

	void Start ()
	{
		characterButtonToggleOn [0] = false;
		characterButtonToggleOn [1] = false;
		characterButtonToggleOn [2] = false;

		//Set starting skills during start of battle
		CharacterManager.SetStartCharacters ();
	}

	public void OnStartPhase ()
	{
		//Check toggle on characters on start of the phase and send it
		ActivateToggledCharacters ();
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
		skillGpCost [characterNumber].text = charCard.characterGPCost.ToString () + "GP";
		characterButton [characterNumber].GetComponent<Image> ().sprite = SystemResourceController.Instance.LoadCharacterCardSprite (charCard.characterID);
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
		characterButton [0].interactable = buttonEnable;
		characterButton [1].interactable = buttonEnable;
		characterButton [2].interactable = buttonEnable;
	}

	//toggle on/off activate characters
	public void SelectCharacter (int characterNumber)
	{
		characterButtonToggleOn [characterNumber] = !characterButtonToggleOn [characterNumber];
		ActivateCharacterIndicator (characterNumber);
	}

	private void ActivateCharacterIndicator (int skillNumber)
	{
		Outline characterButtonOutline = characterButton [skillNumber].GetComponent<Outline> ();
		if (characterButtonToggleOn [skillNumber]) {
			//if clicked
			characterButtonOutline.enabled = true;
			characterButtonOutline.effectColor = new Color32 (255, 96, 26, 255);
		} else {
			//if not
			characterButtonOutline.enabled = false;
		}
	}

	//call this to activate toggled characters
	public void ActivateToggledCharacters ()
	{
		CharacterManager.StartCharacter (characterButtonToggleOn);
	}

	public void ShowSkillDescription (int skillNumber)
	{
		GameObject skillDescription = SystemPopupController.Instance.ShowPopUp ("PopUpSkillDescription");
		skillDescription.GetComponent<PopUpSkillDescriptionController> ().SkillDescription (CharacterManager.GetCharacter (skillNumber).characterDescription);
	}



}

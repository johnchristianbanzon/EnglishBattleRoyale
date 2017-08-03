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

	public void SetCharacterUI (int characterNumber, CharacterModel charCard)
	{
		skillGpCost [characterNumber].text = charCard.characterGPCost.ToString () + "GP";
		characterButton [characterNumber].GetComponent<Image> ().sprite = SystemResourceController.Instance.LoadCharacterCardSprite (charCard.characterID);
	}

	private void CharacterButtonInteractable (int characterNumber, Button button)
	{
		if (CharacterManager.GetCharacter (characterNumber).characterGPCost > ScreenBattleController.Instance.partState.player.playerGP) {
			button.interactable = false;
		} else {
			button.interactable = true;
		}
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

	public void SelectCharacter (int characterNumber)
	{
		characterButtonToggleOn [characterNumber - 1] = !characterButtonToggleOn [characterNumber - 1];
		ActivateCharacterIndicator (characterNumber - 1);
		

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

	public void CheckCharacterActivate ()
	{
		
		for (int i = 0; i < characterButtonToggleOn.Length; i++) {
			if (characterButtonToggleOn [i]) {
				SelectedSkill (i);
			}
		}
		
	}


	public void ShowSkillDescription (int skillNumber)
	{
		GameObject skillDescription = SystemPopupController.Instance.ShowPopUp ("PopUpSkillDescription");
		skillDescription.GetComponent<PopUpSkillDescriptionController> ().SkillDescription (CharacterManager.GetCharacter (skillNumber).characterDescription);
	}



	private void SelectedSkill (int skillNumber)
	{
		SelectSkillActivate (delegate() {
			CharacterManager.ActivateCharacter (skillNumber);
		}, delegate() {
			SystemGlobalDataController.Instance.skillChosenCost = CharacterManager.GetCharacter (skillNumber).characterGPCost;
		});
	}

	private void SelectSkillActivate (Action activateSkill, Action skillCost)
	{
		activateSkill ();
	}




}

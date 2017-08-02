using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PartCharacterController : MonoBehaviour
{
	public bool activateAutoCharacter;
	public Button[] characterButton;
	public Button attackButton;
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
		skillGpCost [characterNumber].text = charCard.characterGPCost.ToString() + "GP";
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

	public void OnStartPhase ()
	{
		if (!activateAutoCharacter) {
			Debug.Log ("Starting Skill Phase");
			if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
				ButtonEnable (true);
			} else {
				CharacterButtonInteractable (1, characterButton [0]);
				CharacterButtonInteractable (2, characterButton [1]);
				CharacterButtonInteractable (3, characterButton [2]);
			}

			attackButton.interactable = true;
			attackButton.gameObject.SetActive (true);


		} else {
			SystemFirebaseDBController.Instance.SkillPhase ();
		}
	}

	public void OnEndPhase ()
	{
		if (!activateAutoCharacter) {
			attackButton.gameObject.SetActive (false);
			ButtonEnable (false);
		}
	}

	public void ShowAutoActivateButtons (bool isShow)
	{
		if (activateAutoCharacter) {
			ButtonEnable (isShow);
		}
	}


	public void AttackButton ()
	{
		ButtonEnable (false);
		SystemFirebaseDBController.Instance.SkillPhase ();
	}

	public void ButtonEnable (bool buttonEnable)
	{
		characterButton [0].interactable = buttonEnable;
		characterButton [1].interactable = buttonEnable;
		characterButton [2].interactable = buttonEnable;
		attackButton.interactable = buttonEnable;
	}

	public void SelectCharacter (int characterNumber)
	{
		if (activateAutoCharacter) {
			characterButtonToggleOn [characterNumber - 1] = !characterButtonToggleOn [characterNumber - 1];
			ActivateCharacterIndicator (characterNumber - 1);
		} else {
			if (characterButton [characterNumber - 1].interactable) {
				SelectedSkill (characterNumber);
			}
		}

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
		if (activateAutoCharacter) {
			for (int i = 0; i < characterButtonToggleOn.Length; i++) {
				if (characterButtonToggleOn [i]) {
					SelectedSkill (i);
				}
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
		if (activateAutoCharacter) {
			activateSkill ();
		} else {
			//change to mode 2
			if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
				SystemGlobalDataController.Instance.playerSkillChosen = delegate() {
					activateSkill ();
				};
				skillCost ();
				SystemFirebaseDBController.Instance.SkillPhase ();
				Debug.Log ("skilled!");
			} else {
				activateSkill ();
			}
			ButtonEnable (false);
		}
	}




}

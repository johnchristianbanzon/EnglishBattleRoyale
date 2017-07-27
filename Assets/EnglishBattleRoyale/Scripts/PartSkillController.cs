using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PartSkillController : MonoBehaviour
{
	public bool activateAutoSkill;
	public Button[] skillButton;
	public Button attackButton;
	private bool[] skillButtonToggleOn = new bool[3];

	public Text[] skillGpCost;

	void Start ()
	{
		skillButtonToggleOn [0] = false;
		skillButtonToggleOn [1] = false;
		skillButtonToggleOn [2] = false;

		//Set starting skills during start of battle
		SkillManager.SetStartSkills ();
	}

	public void SetSkillUI (int skillNumber, SkillModel charCard)
	{
		skillGpCost [skillNumber].text = charCard.skillGpCost.ToString() + "GP";
		skillButton [skillNumber].GetComponent<Image> ().sprite = SystemResourceController.Instance.LoadCharacterCardSprite (charCard.skillName);
	}

	private void SkillButtonInteractable (int skillNumber, Button button)
	{
		if (SkillManager.GetSkill (skillNumber).skillGpCost > ScreenBattleController.Instance.partState.PlayerGP) {
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
		if (!activateAutoSkill) {
			Debug.Log ("Starting Skill Phase");
			if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
				ButtonEnable (true);
			} else {
				SkillButtonInteractable (1, skillButton [0]);
				SkillButtonInteractable (2, skillButton [1]);
				SkillButtonInteractable (3, skillButton [2]);
			}

			attackButton.interactable = true;
			attackButton.gameObject.SetActive (true);

			GameTimeManager.StartSkillTimer (OnEndQuestionTime);

		} else {
			SystemFirebaseDBController.Instance.SkillPhase ();
		}
	}

	public void OnEndPhase ()
	{
		if (!activateAutoSkill) {
			attackButton.gameObject.SetActive (false);
			ButtonEnable (false);
		}
	}

	public void ShowAutoActivateButtons (bool isShow)
	{
		if (activateAutoSkill) {
			ButtonEnable (isShow);
		}
	}


	public void AttackButton ()
	{
		ButtonEnable (false);
		GameTimeManager.ToggleTimer (false);
		SystemFirebaseDBController.Instance.SkillPhase ();
		GameTimeManager.StopTimer ();
	}

	public void ButtonEnable (bool buttonEnable)
	{
		skillButton [0].interactable = buttonEnable;
		skillButton [1].interactable = buttonEnable;
		skillButton [2].interactable = buttonEnable;
		attackButton.interactable = buttonEnable;
	}

	public void SelectSkill (int skillNumber)
	{
		if (activateAutoSkill) {
			skillButtonToggleOn [skillNumber - 1] = !skillButtonToggleOn [skillNumber - 1];
			ActivateSkillIndicator (skillNumber - 1);
		} else {
			if (skillButton [skillNumber - 1].interactable) {
				SelectedSkill (skillNumber);
			}
		}

	}

	private void ActivateSkillIndicator (int skillNumber)
	{
		Outline skillButtonOutline = skillButton [skillNumber].GetComponent<Outline> ();
		if (skillButtonToggleOn [skillNumber]) {
			//if clicked
			skillButtonOutline.enabled = true;
			skillButtonOutline.effectColor = new Color32 (255, 96, 26, 255);

		} else {
			//if not
			skillButtonOutline.enabled = false;
		}
	}

	public void CheckSkillActivate ()
	{
		if (activateAutoSkill) {
			for (int i = 0; i < skillButtonToggleOn.Length; i++) {
				if (skillButtonToggleOn [i]) {
					SelectedSkill (i);
				}
			}
		}
	}


	public void ShowSkillDescription (int skillNumber)
	{
		GameObject skillDescription = SystemPopupController.Instance.ShowPopUp ("PopUpSkillDescription");
		skillDescription.GetComponent<PopUpSkillDescriptionController> ().SkillDescription (SkillManager.GetSkill (skillNumber).skillDescription);
	}



	private void SelectedSkill (int skillNumber)
	{
		SelectSkillActivate (delegate() {
			SkillManager.ActivateSkill (skillNumber);
		}, delegate() {
			SystemGlobalDataController.Instance.skillChosenCost = SkillManager.GetSkill (skillNumber).skillGpCost;
		});
	}

	private void SelectSkillActivate (Action activateSkill, Action skillCost)
	{
		if (activateAutoSkill) {
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
			GameTimeManager.ToggleTimer (false);
			GameTimeManager.StopTimer ();
		}
	}




}

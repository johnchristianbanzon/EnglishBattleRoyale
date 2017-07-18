using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class PhaseSkillController : BasePhase
{
	public bool activateAutoSkill;
	public Button[] skillButton;
	public Button attackButton;
	private bool[] skillButtonToggleOn = new bool[3];

	void Start(){
		skillButtonToggleOn [0] = false;
		skillButtonToggleOn [1] = false;
		skillButtonToggleOn [2] = false;
	}

	private void SkillButtonInteractable (int skillNumber, Button button)
	{
		if (SkillManager.GetSkill (skillNumber).skillGpCost > BattleController.Instance.PlayerGP) {
			button.interactable = false;
		} else {
			button.interactable = true;
		}
	}

	public override void OnStartPhase ()
	{
		if (!activateAutoSkill) {
			Debug.Log ("Starting Skill Phase");
			if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
				ButtonEnable (true);
			} else {
				SkillButtonInteractable (1, skillButton [0]);
				SkillButtonInteractable (2, skillButton [1]);
				SkillButtonInteractable (3, skillButton [2]);
			}

			attackButton.interactable = true;
			attackButton.gameObject.SetActive (true);

			timeLeft = 5;
			stoptimer = true;
			InvokeRepeating ("StartTimer", 0, 1);
		} else {
			SystemFirebaseDBController.Instance.SkillPhase ();
		}
	}

	public override void OnEndPhase ()
	{
		if (!activateAutoSkill) {
			attackButton.gameObject.SetActive (false);
			ButtonEnable (false);
			CancelInvoke ("StartTimer");
		}
	}

	public void ShowAutoActivateButtons(bool isShow){
		if (activateAutoSkill) {
			ButtonEnable (isShow);
		}
	}


	public void AttackButton ()
	{
		ButtonEnable (false);
		GameTimerView.Instance.ToggleTimer (false);
		SystemFirebaseDBController.Instance.SkillPhase ();
		stoptimer = false;
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
			skillButtonToggleOn [skillNumber -1] = !skillButtonToggleOn [skillNumber -1];
			ActivateSkillIndicator(skillNumber-1);
		} else {
			if (skillButton [skillNumber - 1].interactable) {
				TweenLogic.TweenScaleToLarge (EventSystem.current.currentSelectedGameObject.transform, Vector3.one, 0.3f);
				SelectSkillReduce (skillNumber);
			}
		}

	}

	private void ActivateSkillIndicator(int skillNumber){
		if (skillButtonToggleOn [skillNumber]) {
			//if clicked
			skillButton [skillNumber].GetComponent<Outline>() .enabled = true;
			skillButton [skillNumber].GetComponent<Outline>() .effectColor = new Color32 (255,96,26,255);

		} else {
			//if not
			skillButton [skillNumber].GetComponent<Outline>() .enabled = false;
		}
	}

	public void CheckSkillActivate ()
	{
		if (activateAutoSkill) {
			for (int i = 0; i < skillButtonToggleOn.Length; i++) {
				if (skillButtonToggleOn [i]) {
					SelectSkillReduce (i);
				}
			}
		}
	}


	public void ShowSkillDescription (int skillNumber)
	{
		SkillDescriptionReduce (SkillManager.GetSkill (skillNumber).skillDescription, true);

	}

	private void SkillDescriptionReduce (string description, bool isShow)
	{
		GameObject skillDescription = SystemPopupController.Instance.ShowPopUp ("PopUpSkillDescription");
		skillDescription.GetComponent<PopUpSkillDescriptionController> ().SkillDescription (description);
	}



	private void SelectSkillReduce (int skillNumber)
	{
		SelectSkillActivate (delegate() {
			SkillManager.ActivateSkill (skillNumber);

		}, delegate() {
			GameData.Instance.skillChosenCost = SkillManager.GetSkill (skillNumber).skillGpCost;

		});

	}

	private void SelectSkillActivate (Action activateSkill, Action skillCost)
	{
		if (activateAutoSkill) {
			activateSkill ();
		} else {
			//change to mode 2
			if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
				GameData.Instance.playerSkillChosen = delegate() {
					activateSkill ();
				};
				skillCost ();
				SystemFirebaseDBController.Instance.SkillPhase ();
				Debug.Log ("skilled!");
			} else {
				activateSkill ();
			}
			ButtonEnable (false);
			GameTimerView.Instance.ToggleTimer (false);
			stoptimer = false;
		}
	}

	private void StartTimer ()
	{
		if (stoptimer) {
			GameTimerView.Instance.ToggleTimer (true);
			if (timeLeft > 0) {
				GameTimerView.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 
			ButtonEnable (false);
			GameTimerView.Instance.ToggleTimer (false);

			SystemFirebaseDBController.Instance.SkillPhase ();
			Debug.Log ("stopped phase2 timer");
			stoptimer = false;

		}
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestionTimerController : MonoBehaviour{
	public bool stopTimer{ get; set;}
	public int timeLeft{ get; set;}

	void Start(){
		
		InvokeRepeating ("QuestionRoundTimer",0f,1f);
	}

	private void QuestionRoundTimer ()
	{
		/*
		if (stopTimer) {
			GameTimeManager.
			GameTimeManager.Instance.ToggleTimer (true);
			if (timeLeft > 0) {
				GameTimerView.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			}
			GameTimerView.Instance.ToggleTimer (false);
			stopTimer = false;
			QuestionSystemController.Instance.OnSkipQuestion ();

		}*/
	}
}

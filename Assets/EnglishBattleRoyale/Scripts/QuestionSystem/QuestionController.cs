using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class QuestionController : SingletonMonoBehaviour<QuestionController>
{
	public static int getround;
	private static int correctAnswers;
	private static bool stoptimer = false;
	private static int timeLeft;
	private static int timeDuration;
	private static GameObject[] inputButton;
	private float roundlimit = 3;
	private int totalGP;
	private static int questionsTime;
	public static Action<int,int> onResult;

	public bool onFinishQuestion {
		get;
		set;
	}

	public Action<int,int> OnResult {
		get { 
			return onResult;
		}
		set { 
			onResult = value;
		}

	}

	public bool Stoptimer {
		get { 
			return stoptimer;
		}
		set { 
			stoptimer = value;
		}
	}

	void OnEnable ()
	{
		InvokeRepeating ("StartTimer", 0, 1);
	}

	public void SetQuestion (IQuestion questiontype, int qTime, Action<int, int> Result)
	{
		for (int i = 0; i < 12; i++) {
			Destroy (GameObject.Find ("input" + i));
			Destroy (GameObject.Find ("output" + i));

		}
		timeLeft = qTime;
		questiontype.Activate (Result);
		stoptimer = true;
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
				
			GameTimerView.Instance.ToggleTimer (false);
			stoptimer = false;
			ComputeScore ();
				  
		}
	}

	public void ComputeScore ()
	{
		QuestionManager questionManagement = FindObjectOfType<QuestionManager>();
		questionManagement.QuestionHide ();

		for (int i = 0; i < 12; i++) {
			Destroy (GameObject.Find ("input" + i));
		}
		for (int i = 0; i < 12; i++) {
			Destroy (GameObject.Find ("output" + i));
		}
		onResult.Invoke (correctAnswers,timeLeft);
		correctAnswers = 0;
	}


	public void Returner (Action<bool> action, int round, int answerScore)
	{
		action (onFinishQuestion);
		getround = round;
		correctAnswers = answerScore;
		if (round > roundlimit) {
			stoptimer = false;
			ComputeScore ();
		} 

	}
}

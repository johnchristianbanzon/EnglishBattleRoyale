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

	void OnEnable ()
	{
		ScreenBattleController.Instance.partState.gameTimer.QuestionTimer (delegate() {
			ComputeScore ();
		}, timeLeft);
	}

	public void SetQuestion (IQuestion questiontype, int qTime, Action<int, int> Result)
	{
		for (int i = 0; i < 12; i++) {
			Destroy (GameObject.Find ("input" + i));
			Destroy (GameObject.Find ("output" + i));

		}
		timeLeft = qTime;
		questiontype.Activate (Result);
	}

	public void ComputeScore ()
	{
		PartQuestionController questionManagement = FindObjectOfType<PartQuestionController> ();
		questionManagement.QuestionHide ();

		for (int i = 0; i < 12; i++) {
			Destroy (GameObject.Find ("input" + i));
		}
		for (int i = 0; i < 12; i++) {
			Destroy (GameObject.Find ("output" + i));
		}
		onResult.Invoke (correctAnswers, timeLeft);
		correctAnswers = 0;
	}


	public void Returner (Action<bool> action, int round, int answerScore)
	{
		action (onFinishQuestion);
		getround = round;
		correctAnswers = answerScore;
		if (round > roundlimit) {
			ScreenBattleController.Instance.partState.gameTimer.StopTimer ();
			ComputeScore ();
		} 

	}
}

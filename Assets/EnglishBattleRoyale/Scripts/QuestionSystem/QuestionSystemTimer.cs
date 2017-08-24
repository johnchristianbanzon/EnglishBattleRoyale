﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class QuestionSystemTimer : IQuestionTimeObserver {

	public int timePassed = 0;
	public bool isTimerOn = false;
	private int timeLeft = 0;
	private QuestionSystemController questionSystemController;

	public void InitQuestionSystemTimer(bool isTimerOn){
		this.isTimerOn = isTimerOn;
		StartTimer ();
	}

	public QuestionSystemEnums.SpeedyType GetSpeedyType(double idealTime){
		QuestionSystemEnums.SpeedyType speedyType = QuestionSystemEnums.SpeedyType.Good;
		if (timePassed < idealTime) {
			speedyType = QuestionSystemEnums.SpeedyType.Awesome;
		} else if (timePassed >= idealTime) {
			if (timePassed >= (idealTime * 2)) {
				speedyType = QuestionSystemEnums.SpeedyType.Rotten;
			} else {
				speedyType = QuestionSystemEnums.SpeedyType.Good;
			}
		}
		return speedyType;
	}

	public void OnStopQuestionTimer ()
	{
		questionSystemController.isQuestionRoundOver = true;
	}
		
	public void OnStartQuestionTimer (Action<int> action, int timer)
	{
		QuestionSystemController.Instance.timerSlider.fillRect.GetComponent<Image>().color = new Color32 (159, 204, 62, 255);
		questionSystemController.StartCoroutine (StartQuestionTimer (action, timer));
	}

	public void StartTimer(){
		TimeManager.AddQuestionTimeObserver (this);
		questionSystemController = QuestionSystemController.Instance;
		double averageTime = 0;
		for (int i = 0; i < questionSystemController.questionList.Count; i++) {
			averageTime += questionSystemController.questionList [i].idealTime;
		}
		double totalTime = (averageTime / questionSystemController.questionList.Count) * 7.5;
		questionSystemController.timerSlider.maxValue = (float)totalTime;
		TimeManager.StartQuestionTimer (delegate(int timeLeft) {
			ReduceTimeLeftCallBack(timeLeft);
		}, (int)totalTime);	

	}

	public void ReduceTimeLeftCallBack(int timeLeft){
		TweenFacade.SliderTimer (questionSystemController.timerSlider, timeLeft-1);
		questionSystemController.questionHint.OnTimeInterval ();
		if (timeLeft <= 0) {
			TimerEnded ();
		}
	}

	private void TimerEnded(){
		questionSystemController.CheckAnswer (false);
		questionSystemController.onRoundResult (questionSystemController.roundResultList);
		if (questionSystemController.isDebug) {
			questionSystemController.debugUI.transform.GetChild(0).gameObject.SetActive (true);
		} else {
			questionSystemController.gameObject.SetActive (false);
		}
	}

	public IEnumerator StartQuestionTimer (Action<int> action, int timer)
	{
		timeLeft = timer;
		while (timeLeft > -1) {
			if (isTimerOn) {
				timeLeft--;
				timePassed++;
				action (timeLeft);
				if (timeLeft<=3) {
					QuestionSystemController.Instance.timerSlider.fillRect.GetComponent<Image>().color = new Color32 (255, 100, 100, 255);
					QuestionSystemController.Instance.hasNextQuestion = false;
				}
			}
			if (questionSystemController.isQuestionRoundOver) {
				yield break;
			} else {
				yield return new WaitForSeconds (1);
			}

		}

	}


}

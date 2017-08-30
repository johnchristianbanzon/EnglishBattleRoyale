using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class QuestionSystemTimer : IQuestionTimeObserver {

	public int timePassed = 0;
	public bool isTimerOn = false;
	public float timeLeft = 0;
	private QuestionSystemController questionSystemController;

	public void InitQuestionSystemTimer(bool isTimerOn){
		this.isTimerOn = isTimerOn;
		StartTimer ();
	}

	public QuestionSystemEnums.SpeedyType GetSpeedyType(double idealTime){
		hintInterval = 5;
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
		
	public void OnStartQuestionTimer (Action<float> action, float timer)
	{
		QuestionSystemController.Instance.timerSlider.fillRect.GetComponent<Image>().color = new Color32 (159, 204, 62, 255);
		questionSystemController.StartCoroutine(StartQuestionTimer (action, timer));
	}

	public void StartTimer(){
		TimeManager.AddQuestionTimeObserver (this);
		questionSystemController = QuestionSystemController.Instance;
		double averageTime = 0;
		for (int i = 0; i < questionSystemController.questionList.Count; i++) {
			averageTime += questionSystemController.questionList [i].idealTime;
		}
		double totalTime = (averageTime / questionSystemController.questionList.Count) * 7.5;
		totalTime = Mathf.Round((float)totalTime* 10f) / 10f;
		questionSystemController.timerSlider.maxValue = (float)totalTime;
		TimeManager.StartQuestionTimer (delegate(float timeLeft) {
			ReduceTimeLeftCallBack(timeLeft);
		}, (float)totalTime);	

	}

	public void ReduceTimeLeftCallBack(float timeLeft){
		
		questionSystemController.questionHint.OnTimeInterval ();
		if (timeLeft < 0.1) {
			QuestionSystemController.Instance.questionHint.disableHintButton ();
			TimerEnded ();
		}
	}

	private void TimerEnded(){
		isTimerOn = false;
		questionSystemController.CheckAnswer (false);
		questionSystemController.onRoundResult (questionSystemController.roundResultList);
		if (questionSystemController.isDebug) {
			questionSystemController.debugUI.transform.GetChild(0).gameObject.SetActive (true);
		} else {
			questionSystemController.gameObject.SetActive (false);
		}
	}

	private float hintInterval =5;
	public IEnumerator StartQuestionTimer (Action<float> action, float timer)
	{
		timeLeft = timer;
		float timeInterval = 0.1f;
		while (timeLeft > -0.1) {
			if (isTimerOn) {
				timeLeft -= timeInterval;
				timeLeft = Mathf.Round((float)timeLeft* 10f) / 10f;
				TweenFacade.SliderTimer (questionSystemController.timerSlider, timeLeft);
				if ((timeLeft % 1) == 0) {
					hintInterval--;
//					if ((timeLeft%(14 / questionSystemController.correctAnswerButtons.Count) == 0 ) && questionSystemController.questionHint.hasHintAvailable) {
					if (hintInterval == 0) {
						questionSystemController.questionHint.OnClick ();
						hintInterval = 5;
					}
					if (hintInterval <= 3) {
						GameObject hintTimer = SystemResourceController.Instance.LoadPrefab ("Input-UI",SystemPopupController.Instance.popUp);
						hintTimer.GetComponent<Image> ().enabled = false;
						hintTimer.transform.position = questionSystemController.partAnswer.transform.position;
						hintTimer.GetComponentInChildren<Text> ().text = ""+hintInterval;
						TweenFacade.TweenScaleToLarge (hintTimer.transform, Vector3.one, 0.5f);
						TweenFacade.TweenJumpTo (hintTimer.transform, Vector3.one, 120f,1,0.5f,0);
						MonoBehaviour.Destroy (hintTimer, 0.5f);
					}
					timePassed++;
					action (timeLeft);
					if (timeLeft<=1) {
						QuestionSystemController.Instance.timerSlider.fillRect.GetComponent<Image>().color = new Color32 (255, 100, 100, 255);
						QuestionSystemController.Instance.hasNextQuestion = false;
					}
				}

			}
			if (questionSystemController.isQuestionRoundOver) {
				yield break;
			} else {
				yield return new WaitForSeconds (timeInterval);
			}
		}
	}
}
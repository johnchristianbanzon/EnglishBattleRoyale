using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// - Starts The Question Round
/// - Loads Question for the round
/// - Checks Answer if it is right or wrong
/// - Goes to the next question
/// 
/// </summary>
public class QuestionSystemController : SingletonMonoBehaviour<QuestionSystemController>, IQuestionTimeObserver
{
	private int correctAnswers;
	private int currentQuestionNumber = 0;
	private bool hasSkippedQuestion = false;
	public string questionAnswer = "";
	public string questionTarget = "";
	public bool isQuestionRoundOver = false;
	public QuestionSystemEnums.QuestionType questionType;

	public ITarget targetType;
	public ISelection selectionType;
	public IAnswer answerType;

	public List<QuestionResultModel> roundResultList = new List<QuestionResultModel> ();

	public Action<List<QuestionResultModel>> onRoundResult{ get; set; }

	public Action<QuestionResultModel> onQuestionResult{ get; set; }

	public List<GameObject> correctAnswerButtons{ get; set; }
	//
	public Text targetTypeUI;
	public PartSelectionController partSelection;
	public PartAnswerController partAnswer;
	public PartTargetController partTarget;

	public QuestionHintManager questionHint;
	//DEBUG FIELDS
	public InputField timerInput;
	public GameObject debugUI;
	public bool isDebug = false;
	//DEBUG FIELDS ENDS HERE

	//
	public Slider timerSlider;
	private List<QuestionModel> questionList = new List<QuestionModel> ();
	//
	public void StartQuestionRound (QuestionTypeModel questionTypeModel, Action<List<QuestionResultModel>> onRoundResult)
	{
		isQuestionRoundOver = false;
		TimeManager.AddQuestionTimeObserver (this);
		questionList = QuestionBuilder.GetQuestionList (10,questionTypeModel);
		double averageTime = 0;
		for (int i = 0; i < questionList.Count; i++) {
			averageTime += questionList [i].idealTime;
		}
		double totalTime = (averageTime / questionList.Count) * 7;
		timerSlider.maxValue = (float)totalTime;
		TimeManager.StartQuestionTimer (delegate(int timeLeft) {
			TweenFacade.SliderTimer (timerSlider, timeLeft);
			questionHint.OnTimeInterval ();
			if (timeLeft <= 0) {
				CheckAnswer (false);
				targetType.HideTargetType ();
				selectionType.HideSelectionType ();
				answerType.ClearHint ();
				onRoundResult (roundResultList);
				Destroy (gameObject);
			}
		}, (int)totalTime);
		this.onRoundResult = onRoundResult;
		NextQuestion ();
	}

	public void OnStartQuestionTimer (Action<int> action, int timer)
	{
		StartCoroutine (StartQuestionTimer (action, timer));
	}

	private int timePassed = 0;

	public IEnumerator StartQuestionTimer (Action<int> action, int timer)
	{
		int timeLeft = timer;
		while (timeLeft > 0) {
			timeLeft--;
			timePassed++;

			action (timeLeft);
			if (isQuestionRoundOver) {
				yield break;
			} else {
				yield return new WaitForSeconds (1);
			}
		}
	}

	public void OnStopQuestionTimer ()
	{
		isQuestionRoundOver = true;
	}

	void Start ()
	{
		if (isDebug) {
			MyConst.Init ();
			QuestionBuilder.PopulateQuestion ();
			debugUI.SetActive (true);
			//			StartQuestionRound (null, null);
		}
	}

	public void OnSkipQuestion ()
	{
		if (!hasSkippedQuestion) {
			CheckAnswer (false);
			hasSkippedQuestion = true;
		}
	}

	private GameObject speedyEffect;
	public void CheckAnswer (bool isCorrect)
	{
		double idealTime = questionList [currentQuestionNumber].idealTime;
		questionHint.InitHints ();
		if (isCorrect) {
			currentQuestionNumber++;
			correctAnswers++;
			QuestionSpecialEffects spe = new QuestionSpecialEffects ();
			spe.DeployEffect (isCorrect, correctAnswerButtons, questionAnswer);
			speedyEffect = SystemResourceController.Instance.LoadPrefab ("SpeedyEffectText", SystemPopupController.Instance.popUp.gameObject);
			speedyEffect.transform.position = Vector3.zero;
			TweenFacade.TweenScaleToLarge (speedyEffect.transform, Vector3.one, 0.3f);

			if (timePassed < idealTime) {
				speedyEffect.GetComponent<Text> ().text = "Awesome";
			} else if (timePassed >= idealTime) {
				if (timePassed >= (idealTime * 2)) {
					speedyEffect.GetComponent<Text> ().text = "Rotten";				
				} else {
					speedyEffect.GetComponent<Text> ().text = "Good";			
				}
			}
			onQuestionResult.Invoke (new QuestionResultModel (00000, 13, 3, isCorrect, false));
			Invoke ("NextQuestion", 1f);
		} else {
			TweenFacade.TweenShakePosition (gameObject.transform, 1.0f, 30.0f, 50, 90f);
		}

	}

	private void HideQuestionParts ()
	{
		questionList [currentQuestionNumber].questionType.answerType.ClearHint ();
		questionList [currentQuestionNumber].questionType.selectionType.HideSelectionType ();
		questionList [currentQuestionNumber].questionType.targetType.HideTargetType ();
	}

	public void NextQuestion ()
	{
		timePassed = 0;
		questionType = questionList [currentQuestionNumber].questionType.questionCategory;
		targetType = questionList [currentQuestionNumber].questionType.targetType;
		answerType = questionList [currentQuestionNumber].questionType.answerType;
		selectionType = questionList [currentQuestionNumber].questionType.selectionType;
		Destroy (speedyEffect);
		questionHint.InitHints ();
		hasSkippedQuestion = false;
		partSelection.HideSelectionType (selectionType);
//		answerType.ClearHint ();

		GetNewQuestion (questionType, delegate(QuestionResultModel onQuestionResult) {
			roundResultList.Add (onQuestionResult);
			Invoke("HideQuestionParts",1.0f);
		});

	}
	public QuestionModel LoadQuestion ()
	{
		QuestionModel questionLoaded = questionList [currentQuestionNumber];
		if (questionLoaded.answers.Length == 2 && selectionType.Equals (partSelection.wordChoice)) {
			questionAnswer = (questionLoaded.answers [0].ToUpper () + "/" + questionLoaded.answers [1].ToUpper ());
		} else {
			questionAnswer = questionLoaded.answers [UnityEngine.Random.Range (0, questionLoaded.answers.Length)].ToUpper ();
		}
		questionTarget = questionLoaded.question;
		return questionLoaded;
	}

	public void GetNewQuestion (QuestionSystemEnums.QuestionType questionType, Action<QuestionResultModel> onQuestionResult)
	{
		LoadQuestion ();
		targetTypeUI.GetComponentInChildren<Text> ().text = questionType.ToString ();
		partTarget.DeployPartTarget (targetType, questionTarget);
		partAnswer.DeployAnswerType (answerType);
		partSelection.DeploySelectionType (selectionType, questionAnswer, delegate(List<GameObject> selectionList) {
			CheckAnswerSent (null);
		});
		this.onQuestionResult = onQuestionResult;
	}

	public void CheckAnswerSent (List<GameObject> correctAnswerButtons)
	{
		this.correctAnswerButtons = correctAnswerButtons;
	}

	public void OnDebugClick (Button button)
	{
		StartQuestionRound (QuestionBuilder.getQuestionType (button.name), delegate(List<QuestionResultModel> result) {
			debugUI.SetActive (true);
		});
		debugUI.SetActive (false);
		button.transform.parent.gameObject.SetActive (false);
	}

	public void UpdateFirebaseAnswerModel (bool isCorrect)
	{
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		string isCorrectParam;
		if (isCorrect) {
			isCorrectParam = ParamNames.AnswerCorrect.ToString ();
			param [isCorrectParam] = currentQuestionNumber;
		} else {
			isCorrectParam = ParamNames.AnswerWrong.ToString ();
		}
	}

}

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
	private int currentQuestionNumber = 1;
	private bool hasSkippedQuestion = false;
	public string questionAnswer = "";
	public string questionTarget = "";
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
	//DEBUG FIELDS ENDS HERE

	//
	public Slider timerSlider;

	//
	public void StartQuestionRound (QuestionTypeModel questionTypeModel,Action<List<QuestionResultModel>> onRoundResult)
	{
		TimeManager.AddQuestionTimeObserver (this);
		questionType = questionTypeModel.questionCategory;
		targetType = questionTypeModel.targetType;
		answerType = questionTypeModel.answerType;
		selectionType = questionTypeModel.selectionType;
		timerSlider.maxValue = 25;
		TimeManager.StartQuestionTimer (delegate(int timeLeft) {
			TweenFacade.SliderTimer(timerSlider,timeLeft);
			questionHint.OnTimeInterval();
			if(timeLeft<=0){
				CheckAnswer(false);
				targetType.HideTargetType();
				selectionType.HideSelectionType();
				answerType.ClearHint();
			}
		}, 25);
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

			yield return new WaitForSeconds (1);
		}
	}

	public void OnStopQuestionTimer(){

	}

	private IEnumerator WaitAndPrint(float waitTime)
	{
		while (true)
		{
			yield return new WaitForSeconds(waitTime);
			print("WaitAndPrint " + Time.time);
		}
	}

	public QuestionModel LoadQuestion ()
	{
		QuestionModel questionLoaded = QuestionBuilder.GetQuestion (questionType,selectionType);
		if (questionLoaded.answers.Length == 2 && selectionType.Equals (partSelection.wordChoice)) {
			questionAnswer = (questionLoaded.answers [0].ToUpper () + "/" + questionLoaded.answers [1].ToUpper ());
		} else {
			questionAnswer = questionLoaded.answers [UnityEngine.Random.Range (0, questionLoaded.answers.Length)].ToUpper ();
		}
		questionTarget = questionLoaded.question;
		return questionLoaded;
	}

	void Start ()
	{
		QuestionBuilder.PopulateQuestion ();
		/*
		string[] questionTypes = new string[6]{ "sellect", "typing", "change", "word", "slot", "letter" };
		QuestionSystemController.Instance.StartQuestionRound (
			QuestionBuilder.getQuestionType (questionTypes [UnityEngine.Random.Range (0, questionTypes.Length)])
			, delegate(List<QuestionResultModel> result) {
				//questionResultList = result;
			}
		);*/
	}

	public void OnSkipQuestion ()
	{
		if (!hasSkippedQuestion) {
			CheckAnswer (false);
			hasSkippedQuestion = true;
		}
	}

	private GameObject speedyEffect;
	private double idealTime = QuestionSystemConst.ANSWER_SPEED_BASETIME;
	public void CheckAnswer (bool isCorrect)
	{
		currentQuestionNumber ++;
		Debug.Log (timePassed);
		if (isCorrect) {
			correctAnswers++;
			QuestionSpecialEffects spe = new QuestionSpecialEffects ();
			spe.DeployEffect (isCorrect, correctAnswerButtons, questionAnswer);
			if (timePassed < idealTime) {
				speedyEffect = SystemResourceController.Instance.LoadPrefab ("SpeedyEffectText", SystemPopupController.Instance.popUp.gameObject);
				speedyEffect.transform.position = Vector3.zero;
				speedyEffect.GetComponent<Text> ().text = "Awesome";
				TweenFacade.TweenScaleToLarge (speedyEffect.transform,Vector3.one, 0.3f);
			}
			onQuestionResult.Invoke (new QuestionResultModel (00000, 13, 3, isCorrect, false));
			Invoke ("NextQuestion", 1f);
		} else {
			TweenFacade.TweenShakePosition (gameObject.transform, 1.0f, 30.0f, 50, 90f);
		}

	}

	public void NextQuestion ()
	{
		timePassed = 0;
		Destroy (speedyEffect);
		questionHint.InitHints ();
		hasSkippedQuestion = false;
		partSelection.HideSelectionType(selectionType);
		answerType.ClearHint ();
		GetNewQuestion (questionType, delegate(QuestionResultModel onQuestionResult) {

			//roundResultList.Add(onQuestionResult);
			//onRoundResult.Invoke(roundResultList);
		});

	}

	public void GetNewQuestion (QuestionSystemEnums.QuestionType questionType, Action<QuestionResultModel> onQuestionResult)
	{
		LoadQuestion ();
		targetTypeUI.GetComponentInChildren<Text> ().text = questionType.ToString ();
		partTarget.DeployPartTarget (targetType, questionTarget);
		partAnswer.DeployAnswerType (answerType);
		partSelection.DeploySelectionType (selectionType, questionAnswer,delegate(List<GameObject> selectionList) {
			CheckAnswerSent(null);
		});
		this.onQuestionResult = onQuestionResult;
	}

	public void CheckAnswerSent(List<GameObject> correctAnswerButtons){
		this.correctAnswerButtons = correctAnswerButtons;
	}

	public void OnDebugClick(Button button){
		StartQuestionRound (QuestionBuilder.getQuestionType (button.name),delegate(List<QuestionResultModel> result) {
			//questionResultList = result;
			debugUI.SetActive(true);
		});
		debugUI.SetActive(false);
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

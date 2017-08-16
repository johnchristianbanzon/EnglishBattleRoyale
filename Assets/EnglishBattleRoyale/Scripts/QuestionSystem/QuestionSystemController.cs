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
public class QuestionSystemController : SingletonMonoBehaviour<QuestionSystemController>
{
	private int currentQuestionNumber = 0;
	private bool hasSkippedQuestion = false;
	public string questionAnswer = "";
	public string questionTarget = "";
	public bool isQuestionRoundOver = false;
	public QuestionSystemEnums.TargetType questionType;

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
	public List<QuestionModel> questionList = new List<QuestionModel> ();
	public QuestionSystemTimer questionRoundTimer;
	//
	public void StartQuestionRound (QuestionTypeModel questionTypeModel, Action<List<QuestionResultModel>> onRoundResult)
	{
		currentQuestionNumber = 0;
		isQuestionRoundOver = false;
		questionList = QuestionBuilder.GetQuestionList (10, questionTypeModel);
		questionRoundTimer = new QuestionSystemTimer ();
		questionRoundTimer.InitQuestionSystemTimer (true);
		this.onRoundResult = onRoundResult;
		NextQuestion ();
	}


	void Start ()
	{
		if (isDebug) {
			MyConst.Init ();
			IQuestionProvider provider = new QuestionCSVProvider ();
			QuestionBuilder.PopulateQuestion (provider);
			debugUI.SetActive (true);

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
	private double idealTime;

	/// <summary>
	/// Increases the current QuestionNumber
	/// 
	/// </summary>
	/// <param name="isCorrect">If set to <c>true</c> is correct.</param>
	public void CheckAnswer (bool isCorrect)
	{
		idealTime = questionList [currentQuestionNumber].idealTime;
		questionHint.InitHints ();
		if (isCorrect) {
			QuestionSystemEnums.SpeedyType speedyType = questionRoundTimer.GetSpeedyType(idealTime);
			ShowSpeedyEffect (speedyType);
			onQuestionResult(new QuestionResultModel (00000, 13, 3, isCorrect, speedyType));
			QuestionFinish ();
			Invoke ("NextQuestion", 1f);
		} else {
			TweenFacade.TweenShakePosition (gameObject.transform, 1.0f, 30.0f, 50, 90f);
		}

	}

	private void QuestionFinish(){
		currentQuestionNumber++;
		questionRoundTimer.isTimerOn = false;
		questionHint.disableHintButton ();
	}

	/// <summary>
	/// Instantiates Speed Effect Prefab
	/// </summary>
	/// <param name="speedyType">Speedy type.</param>
	public void ShowSpeedyEffect(QuestionSystemEnums.SpeedyType speedyType){
		switch (speedyType) {
		case QuestionSystemEnums.SpeedyType.Awesome:
			speedyEffect = SystemResourceController.Instance.LoadPrefab ("AwesomeEffect",SystemPopupController.Instance.popUp);
			speedyEffect.GetComponent<Text>().text = "AWESOME!!";
			break;
		case QuestionSystemEnums.SpeedyType.Good:
			speedyEffect = SystemResourceController.Instance.LoadPrefab ("AwesomeEffect",SystemPopupController.Instance.popUp);
			speedyEffect.GetComponent<Text>().text = "GOOD";
			break;
		case QuestionSystemEnums.SpeedyType.Rotten:
			speedyEffect = SystemResourceController.Instance.LoadPrefab ("RottenEffect",SystemPopupController.Instance.popUp);
			speedyEffect.GetComponent<Text>().text = "ROTTEN";
			break;
		}

		speedyEffect.transform.position = Vector3.zero;
		TweenFacade.TweenScaleToLarge (speedyEffect.transform, Vector3.one, 0.3f);
	}

	private void HideQuestionParts ()
	{
		questionList [currentQuestionNumber - 1].questionType.answerType.ClearHint ();
		questionList [currentQuestionNumber - 1].questionType.selectionType.HideSelectionType ();
		targetType.HideTargetType ();
		questionRoundTimer.isTimerOn = true;
	}

	private void InitQuestionType(){
		if (questionType.Equals (QuestionSystemEnums.TargetType.Association)) {
			targetType = partTarget.association;
		} else {
			targetType = partTarget.singleQuestion;
		}
		answerType = questionList [currentQuestionNumber].questionType.answerType;
		selectionType = questionList [currentQuestionNumber].questionType.selectionType;
	}

	public void NextQuestion ()
	{
		questionRoundTimer.timePassed = 0;
		questionType = questionList [currentQuestionNumber].questionType.questionCategory;
		InitQuestionType ();
		Destroy (speedyEffect);
		questionHint.InitHints ();
		questionHint.enableHintButton ();
		hasSkippedQuestion = false;
		GetNewQuestion (questionType, delegate(QuestionResultModel onQuestionResult) {
			roundResultList.Add (onQuestionResult);

			//for every correct answer, send to firebase for answer indicator count
			if(onQuestionResult.isCorrect && !isDebug){
				SystemFirebaseDBController.Instance.SetParam(MyConst.RPC_DATA_ANSWER_INDICATOR, "isCorrect");
			}
			Invoke ("HideQuestionParts", 1.0f);
		});


	}

	public QuestionModel LoadQuestion ()
	{
		QuestionModel questionLoaded = questionList [currentQuestionNumber];
		if (questionLoaded.answers.Length >= 2 && selectionType.Equals (partSelection.wordChoice)) {
			questionAnswer = (questionLoaded.answers [0].ToUpper () + "/" + questionLoaded.answers [1].ToUpper ()
			+ "/" + questionLoaded.answers [2].ToUpper ());
		} else {
			questionAnswer = questionLoaded.answers [UnityEngine.Random.Range (0, questionLoaded.answers.Length)].ToUpper ();
		}
		questionTarget = questionLoaded.question;
		return questionLoaded;
	}

	public void GetNewQuestion (QuestionSystemEnums.TargetType questionType, Action<QuestionResultModel> onQuestionResult)
	{
		LoadQuestion ();
		targetTypeUI.GetComponentInChildren<Text> ().text = questionType.ToString ();
		partTarget.ChangeTargetColor (questionType);
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
		Debug.Log (correctAnswerButtons.Count);
	}

	public void OnDebugClick (Button button)
	{
		StartQuestionRound (QuestionBuilder.GetQuestionType (button.name), delegate(List<QuestionResultModel> result) {
			debugUI.SetActive (true);
			HideQuestionParts();
		});
		debugUI.SetActive (false);
		button.transform.parent.gameObject.SetActive (false);
	}

}

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
	public bool questionRoundHasStarted = false;
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

	public GameObject partScrollContent;
	public GameObject partScrollImage;

	public QuestionHintManager questionHint;
	//DEBUG FIELDS
	public InputField timerInput;
	public GameObject debugUI;
	public bool isDebug = false;
	public Dropdown difficultyDrop;
	//DEBUG FIELDS ENDS HERE

	public GameObject scrollBody;
	public GameObject scrollHeader;
	//
	public Slider timerSlider;
	public List<QuestionModel> questionList = new List<QuestionModel> ();
	public QuestionSystemTimer questionRoundTimer;
	public GameObject hintInterval;

	//
	public void StartQuestionRound (QuestionTypeModel questionTypeModel, Action<List<QuestionResultModel>> onRoundResult)
	{
		roundResultList.Clear ();
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
			debugUI.transform.position = Vector3.zero;

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
		questionHint.hasHintAvailable = false;
		idealTime = questionList [currentQuestionNumber].idealTime;

		if (isCorrect) {
			QuestionSystemEnums.SpeedyType speedyType = questionRoundTimer.GetSpeedyType(idealTime);
			ShowSpeedyEffect (speedyType);
			onQuestionResult(new QuestionResultModel (00000, 13, 3, isCorrect, speedyType));
			QuestionFinish ();
			Invoke ("NextQuestion", 1f);
		} else {
			SystemSoundController.Instance.PlaySFX ("SFX_mistake");
			TweenFacade.TweenShakePosition (gameObject.transform, 1.0f, 30.0f, 50, 90f);
		}

	}

	private void QuestionFinish(){
		currentQuestionNumber++;
		questionHint.disableHintButton ();
	}

	public void OnQuestionRoundFinish(){
		debugUI.SetActive (false);
		questionRoundHasStarted = false;
		isQuestionRoundOver = true;
		TweenFacade.TweenScaleYT0Zero(0.5f,partScrollImage,1);
		selectionType.ShowCorrectAnswer(false);
		Invoke("HideScrollUI",3.0f);
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
			SystemSoundController.Instance.PlaySFX ("SFX_Awesome");
			break;
		case QuestionSystemEnums.SpeedyType.Good:
			speedyEffect = SystemResourceController.Instance.LoadPrefab ("AwesomeEffect",SystemPopupController.Instance.popUp);
			speedyEffect.GetComponent<Text>().text = "GOOD";
			SystemSoundController.Instance.PlaySFX ("SFX_correct");
			break;
		case QuestionSystemEnums.SpeedyType.Rotten:
			speedyEffect = SystemResourceController.Instance.LoadPrefab ("RottenEffect",SystemPopupController.Instance.popUp);
			speedyEffect.GetComponent<Text>().text = "NOT BAD";
			SystemSoundController.Instance.PlaySFX ("SFX_rotten");
			break;
		}

		speedyEffect.transform.position = Vector3.zero;
		if (speedyType.Equals (QuestionSystemEnums.SpeedyType.Awesome)) {
			TweenFacade.TweenScaleToLarge (speedyEffect.transform, Vector3.one, 1.0f);
		}
	}

	public void HideQuestionParts ()
	{
		questionHint.InitHints ();
		int questionNumber = currentQuestionNumber;
		if (questionNumber>0) {
			questionNumber = questionNumber - 1;
		}
		questionList [questionNumber].questionType.answerType.ClearHint ();
		questionList [questionNumber].questionType.selectionType.HideSelectionType ();
		targetType.HideTargetType ();
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

	public bool hasNextQuestion = true;
	private bool hasGivenGraceTime = false;
	public void NextQuestion ()
	{
		questionRoundTimer.timePassed = 0;
		questionType = questionList [currentQuestionNumber].questionType.questionCategory;
		InitQuestionType ();
		Destroy (speedyEffect);
		questionHint.enableHintButton ();
		hasSkippedQuestion = false;
		if (hasNextQuestion) {
			GetNewQuestion (questionType, delegate(QuestionResultModel onQuestionResult) {
				roundResultList.Add (onQuestionResult);
				if (onQuestionResult.isCorrect && !isDebug) {
					SystemFirebaseDBController.Instance.SetParam (MyConst.RPC_DATA_ANSWER_INDICATOR, "isCorrect");
				}
				Invoke ("HideQuestionParts", 1.0f);
			});
		}
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
	}

	private QuestionTypeModel questionTypeModel;
	public GameObject popUpSelectionIndicator;
	public void OnDebugClick (Button button)
	{
		questionTypeModel = QuestionBuilder.GetQuestionType (button.name);
		InitQuestionSystem ();
	}


	private Vector2 scrollHeaderPos = new Vector2();
	public void InitQuestionSystem(){
		questionList = QuestionBuilder.GetQuestionList (10, questionTypeModel);
		CancelInvoke ();
		currentQuestionNumber = 0;
		isQuestionRoundOver = false;
		if (questionList.Count > 0) {
			hasNextQuestion = true;
		}
		scrollHeaderPos = new Vector2 (0, scrollHeader.transform.localPosition.y);
		scrollHeader.transform.localPosition = new Vector2 (0, 240);
		targetTypeUI.text = questionList [0].questionType.questionCategory.ToString();
		partTarget.ChangeTargetColor (questionList [0].questionType.questionCategory);
		selectionType = questionList [currentQuestionNumber].questionType.selectionType;
		popUpSelectionIndicator = selectionType.ShowSelectionPopUp ();
		TweenFacade.TweenScaleToLarge (popUpSelectionIndicator.transform, Vector3.one, 0.3f);
		popUpSelectionIndicator.transform.position = Vector3.zero;
		QuestionUIEntry ();
		StartCoroutine (DebugStartQuestionCoroutine ());

	}

	public void QuestionUIEntry(){
		
		partScrollImage.transform.localScale = new Vector2 (partScrollImage.transform.localScale.x, 0);
		partScrollContent.SetActive (false);
		debugUI.SetActive (false);
		TweenFacade.TweenMoveTo (scrollBody.transform,Vector3.zero,1.0f);
		scrollBody.SetActive(true);
		transform.localPosition = new Vector2 (- 720f,transform.localPosition.y);
		TweenFacade.TweenMoveTo (transform,Vector3.zero,0.5f);
	}

	IEnumerator DebugStartQuestionCoroutine(){

		yield return new WaitForSeconds (2f);
		yield return (StartCoroutine(TweenCoroutine()));
		partScrollContent.SetActive (true);

		if (isDebug) {
			StartQuestionRound (questionTypeModel, delegate(List<QuestionResultModel> result) {
				OnQuestionRoundFinish ();
			});
		}
		Destroy (popUpSelectionIndicator);
		debugUI.SetActive (false);
	}

	IEnumerator TweenCoroutine(){
		TweenFacade.TweenMoveTo (scrollHeader.transform, scrollHeaderPos, 0.3f);
		TweenFacade.TweenScaleYToCustom(0.5f,partScrollImage,1);
		yield return new WaitForSeconds(0.5f);
	}

	public void HideScrollUI(){
		scrollBody.SetActive(false);
		questionHint.disableHintButton ();
		TweenFacade.TweenMoveTo(scrollHeader.transform,new Vector2(800f,scrollHeader.transform.localPosition.y),0.6f);
		HideQuestionParts();
		if (isDebug) {
			debugUI.transform.localPosition = Vector3.zero;
			debugUI.SetActive (true);
		}
	}

}

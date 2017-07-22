using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// - Starts The Question Round
/// - Loads Question for the round
/// - Checks Answer if it is right or wrong
/// - Goes to the next question
/// 
/// </summary>
public class QuestionSystemController : SingletonMonoBehaviour<QuestionSystemController>
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
	public PartSelectionController partSelectionController;
	public PartAnswerController partAnswerController;
	public PartTargetController partTargetController;

	public void StartQuestionRound (int timeLeft, Action<List<QuestionResultModel>> onRoundResult)
	{
		questionType = QuestionSystemEnums.QuestionType.Definition;
		targetType = partTargetController.singleQuestionController;
		answerType = partAnswerController.fillAnswerController;
		selectionType = partSelectionController.selectLetterController;
		this.onRoundResult = onRoundResult;
		NextQuestion ();
	}

	public Question LoadQuestion ()
	{
		Question questionLoaded = QuestionBuilder.GetQuestion (questionType);
		questionAnswer = (questionLoaded.answers.Length == 2 && selectionType.Equals(partSelectionController.wordChoiceController)) ? 
			(questionLoaded.answers [0].ToUpper () + "/" + questionLoaded.answers [1].ToUpper ()) :
				questionLoaded.answers [UnityEngine.Random.Range (0, questionLoaded.answers.Length)].ToUpper ();
		questionTarget = questionLoaded.question;
		return questionLoaded;
	}

	void Start ()
	{
		QuestionBuilder.PopulateQuestion ("QuestionSystemCsv");
		StartQuestionRound (600, delegate(List<QuestionResultModel> onRoundResult) {
			
		});
	}

	public void OnSkipQuestion ()
	{
		if (!hasSkippedQuestion) {
			CheckAnswer (false);
			hasSkippedQuestion = true;
		}
	}

	public void CheckAnswer (bool isCorrect)
	{
		currentQuestionNumber += 1;
		correctAnswers = isCorrect ? correctAnswers + 1 : correctAnswers;
		QuestionSpecialEffects spe = new QuestionSpecialEffects ();
		spe.DeployEffect (isCorrect, correctAnswerButtons, questionAnswer);
		onQuestionResult.Invoke (new QuestionResultModel (00000,13,3,isCorrect,false));
		Invoke ("NextQuestion", 1f);
	}

	public void NextQuestion ()
	{
		hasSkippedQuestion = false;
		GetNewQuestion (questionType, delegate(QuestionResultModel onQuestionResult) {
			roundResultList.Add(onQuestionResult);
			Debug.Log(onQuestionResult.isCorrect);
			onRoundResult.Invoke(roundResultList);			
		});
	}

	public void GetNewQuestion (QuestionSystemEnums.QuestionType questionType, Action<QuestionResultModel> onQuestionResult)
	{
		LoadQuestion ();
		targetTypeUI.GetComponentInChildren<Text> ().text = questionType.ToString ();
		partTargetController.DeployPartTarget (targetType, questionTarget);
		partAnswerController.DeployAnswerType (answerType);
		partSelectionController.DeploySelectionType (selectionType, questionAnswer);
		this.onQuestionResult = onQuestionResult;
	}
		
	public void UpdateFirebaseAnswerModel (bool isCorrect)
	{
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		string isCorrectParam = isCorrect ? ParamNames.AnswerCorrect.ToString () : ParamNames.AnswerWrong.ToString ();
		param [isCorrectParam] = currentQuestionNumber;
		//	FDController.Instance.SetAnswerParam (new AnswerModel(JsonConverter.DicToJsonStr (param).ToString()));
	}

}

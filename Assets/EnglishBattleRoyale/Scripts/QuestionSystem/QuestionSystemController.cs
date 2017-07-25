﻿using System.Collections;
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
	public PartSelectionController partSelection;
	public PartAnswerController partAnswer;
	public PartTargetController partTarget;

	public void StartQuestionRound (QuestionTypeModel questionTypeModel,Action<List<QuestionResultModel>> onRoundResult = null)
	{
		questionType = questionTypeModel.questionCategory;
		targetType = questionTypeModel.targetType;
		answerType = questionTypeModel.answerType;
		selectionType = questionTypeModel.selectionType;
		this.onRoundResult = onRoundResult;
		NextQuestion ();
	}

	public QuestionModel LoadQuestion ()
	{
		QuestionModel questionLoaded = QuestionBuilder.GetQuestion (questionType,selectionType);
		if (questionLoaded.answers.Length == 2 && selectionType.Equals (partSelection.wordChoiceController)) {
			questionAnswer = (questionLoaded.answers [0].ToUpper () + "/" + questionLoaded.answers [1].ToUpper ());
		} else {
			questionAnswer = questionLoaded.answers [UnityEngine.Random.Range (0, questionLoaded.answers.Length)].ToUpper ();
		}
		questionTarget = questionLoaded.question;
		return questionLoaded;
	}

	void Start ()
	{
		QuestionBuilder.PopulateQuestion ("QuestionSystemCsv");
		StartQuestionRound (new QuestionTypeModel (
			QuestionSystemEnums.QuestionType.Synonym,
			partTarget.singleQuestionController,
			partAnswer.noAnswerController,
			partSelection.slotMachineController
		)
		);
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
		currentQuestionNumber ++;
		if (isCorrect) {correctAnswers++;} 
		QuestionSpecialEffects spe = new QuestionSpecialEffects ();
		spe.DeployEffect (isCorrect, correctAnswerButtons, questionAnswer);
		onQuestionResult.Invoke (new QuestionResultModel (00000,13,3,isCorrect,false));
		Invoke ("NextQuestion", 1f);
	}

	public void NextQuestion ()
	{
		hasSkippedQuestion = false;
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
		partSelection.DeploySelectionType (selectionType, questionAnswer);
		this.onQuestionResult = onQuestionResult;
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

		//	FDController.Instance.SetAnswerParam (new AnswerModel(JsonConverter.DicToJsonStr (param).ToString()));
	}

}
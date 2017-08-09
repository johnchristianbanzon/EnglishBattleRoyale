using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// QUESTION SYSTEM UTILITY CLASS, BUILDING QUESTION
/// </summary>
public static class QuestionBuilder
{
	public static QuestionSystemEnums.TargetType questionType;
	private static List<string> questionsDone = new List<string> ();
	private static List<QuestionRowModel> questionList = new List<QuestionRowModel> ();
	private static List<string> wrongChoices = new List<string> ();
	private static List<int> wrongChoicesDone = new List<int> ();

	public static int questionIndex = 1;

	public static void PopulateQuestion ()
	{
		questionList.Clear ();
		wrongChoices.Clear ();
		questionList = MyConst.GetQuestionList();
		wrongChoices = MyConst.GetWrongChoices ();
	}

	public static List<QuestionModel> GetQuestionList(int numberOfQuestions,QuestionTypeModel questionTypeModel){
		List<QuestionModel> questions =  new List<QuestionModel>();
		string[] questionTypes = new string[6]{ "select", "typing", "change", "word", "slot", "letter" };
		for (int i = 0; i < numberOfQuestions; i++) {
			questions.Add (GetQuestion (GetQuestionType(questionTypes[UnityEngine.Random.Range(0,questionTypes.Length)])));
//			questions.Add(GetQuestion(questionTypeModel));
		}
		return questions;
	}

	public static QuestionModel GetQuestion (QuestionTypeModel questionType)
	{
		int randomize = 0;
		bool questionViable = false;
		string question = "";
		List<string> answersList = new List<string> ();
		int numOfQuestions = questionList.Count;
		int whileIndex = 0;

		double idealTime = 2.5;

		while (!questionViable) {
			randomize = UnityEngine.Random.Range (0, questionList.Count);
			answersList.Clear ();
			switch (questionType.questionCategory) {
			case QuestionSystemEnums.TargetType.Antonym:
				if (questionList [randomize].hasAntonym.ToString()=="1") {
					if (questionType.selectionType.ToString().Equals("WordChoice (WordChoice)")) {
						answersList.Add (questionList [randomize].antonym1);
						answersList.Add (questionList [randomize].antonym2);
						answersList.Add (wrongChoices [randomize]);
						question = questionList [randomize].answer;
						questionViable = true;
					} else {
						answersList.Add (questionList [randomize].answer);
						question = questionList [randomize].antonym1;
						questionViable = true;
					}
				}
				break;
			case QuestionSystemEnums.TargetType.Synonym:
				if (questionList [randomize].hasSynonym.ToString()=="1") {
					if (questionType.selectionType.ToString().Equals("WordChoice (WordChoice)")) {
						answersList.Add (questionList [randomize].synonym1);
						answersList.Add (questionList [randomize].synonym2);
						answersList.Add (wrongChoices [randomize]);
						question = questionList [randomize].answer;
						questionViable = true;
					} else {
						answersList.Add (questionList [randomize].answer);
						question = questionList [randomize].synonym1;
						questionViable = true;
					}
				}
				break;

			case QuestionSystemEnums.TargetType.Definition:
				if (questionList [randomize].hasDefinition.ToString()=="1") {
					if (questionType.selectionType.ToString ().Equals("SlotMachine (SlotMachine)")) {
						if (questionList [randomize].answer.Length < 6) {
							answersList.Add (questionList [randomize].answer);
							question = questionList [randomize].definition;
							questionViable = true;
						} 

					} else {
						answersList.Add (questionList [randomize].answer);
						question = questionList [randomize].definition;
						questionViable = true;
					}
				}
				break;

			case QuestionSystemEnums.TargetType.Association:
				if (questionList [randomize].hasClues.ToString()=="1") {
					answersList.Add (questionList [randomize].answer);
					question = questionList [randomize].clues1 + "/"+questionList [randomize].clues2
						+"/"+questionList [randomize].clues3+"/"+questionList [randomize].clues4;
					questionViable = true;
				}
				break;
			}
			if (questionsDone.Contains (question)) {
				questionViable = false;
				if (whileIndex >= numOfQuestions) {
					questionsDone.Clear ();
				}
			}
			whileIndex ++;
		}

		switch (questionType.questionCategory) {
		case QuestionSystemEnums.TargetType.Definition:
			idealTime += 0.5;
			break;
		case QuestionSystemEnums.TargetType.Association:
			idealTime += 1;
			break;
		}

		switch (questionType.selectionType.GetType ().Name) {
		case "Typing":
			idealTime += 1.5;
			break;
		case "SelectLetter":
			idealTime += 1;
			break;
		case "SlotMachine":
			idealTime += 1;
			break;
		}

		QuestionModel questionGot = new QuestionModel (questionType,question, answersList.ToArray (),idealTime);
		questionsDone.Add (question);

		//Debug.Log (questionGot.answers[0] + "/" + questionGot.question);

		return questionGot;
	}

	/// <summary>
	/// Returns a randomized unique choices from Choices List
	/// </summary>
	/// <returns>The random choices.</returns>
	public static string GetRandomChoices ()
	{
		int randomnum = UnityEngine.Random.Range (0, wrongChoices.Count);
		while (wrongChoicesDone.Contains (randomnum)) {
			randomnum = UnityEngine.Random.Range (0, wrongChoices.Count);
		}
		string wrongChoice = wrongChoices [randomnum];
		Debug.Log (wrongChoice);
		return wrongChoice;
	}

	public static QuestionTypeModel GetQuestionType(string selection){

		QuestionTypeModel typeModel = null;
		switch(selection){
		case "select":
			typeModel = new QuestionTypeModel (
				QuestionSystemEnums.TargetType.Definition,
				QuestionSystemController.Instance.partAnswer.fillAnswer,
				QuestionSystemController.Instance.partSelection.selectLetter
			);
			break;
		case "typing":
			typeModel = new QuestionTypeModel (
				QuestionSystemEnums.TargetType.Definition,
				QuestionSystemController.Instance.partAnswer.fillAnswer,
				QuestionSystemController.Instance.partSelection.typing
			);
			break;
		case "change":
			typeModel = new QuestionTypeModel (
				QuestionSystemEnums.TargetType.Synonym,
				QuestionSystemController.Instance.partAnswer.showAnswer,
				QuestionSystemController.Instance.partSelection.changeOrder
			);
			break;
		case "word":
			typeModel = new QuestionTypeModel (
				QuestionSystemEnums.TargetType.Synonym,
				QuestionSystemController.Instance.partAnswer.noAnswer,
				QuestionSystemController.Instance.partSelection.wordChoice
			);
			break;
		case "slot":
			typeModel = new QuestionTypeModel (
				QuestionSystemEnums.TargetType.Definition,
				QuestionSystemController.Instance.partAnswer.showAnswer,
				QuestionSystemController.Instance.partSelection.slotMachine
			);
			break;
		case "letter":
			typeModel = new QuestionTypeModel (
				QuestionSystemEnums.TargetType.Association,
				QuestionSystemController.Instance.partAnswer.showAnswer,
				QuestionSystemController.Instance.partSelection.letterLink
			);
			break;
		}
		return typeModel;
	}
}


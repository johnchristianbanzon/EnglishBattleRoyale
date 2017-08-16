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

	public static void PopulateQuestion (IQuestionProvider provider)
	{
		questionList.Clear ();
		wrongChoices.Clear ();
		questionList = provider.GetQuestionList();
		wrongChoices = MyConst.GetWrongChoices();
	}

	public static List<QuestionModel> GetQuestionList(int numberOfQuestions,QuestionTypeModel questionTypeModel){
		List<QuestionModel> questions =  new List<QuestionModel>();

		Dictionary<string,int> dictionary = new Dictionary<string,int> ();
		dictionary.Add ("SelectLetter", 1);
		dictionary.Add ("Typing", 1);
		dictionary.Add ("ChangeOrderController", 1);
		dictionary.Add ("WordChoice", 1);
		dictionary.Add ("SlotMachine", 1);
		dictionary.Add ("LetterLink", 1);
		string selectionFromRandom = QuestionGenerator.GetPseudoRandomValue (dictionary);
		for (int i = 0; i < numberOfQuestions; i++) {
			if (QuestionSystemController.Instance.isDebug) {
//				questions.Add (GetQuestion (questionTypeModel));
				string questionType = questionTypeModel.selectionType.GetType().Name;
				questions.Add (GetQuestion (GetQuestionType(questionType)));
			} else {
				questions.Add (GetQuestion (GetQuestionType (selectionFromRandom)));

			}
			selectionFromRandom = QuestionGenerator.GetPseudoRandomValue (dictionary);
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
					if (questionType.selectionType.GetType().Name.Equals("WordChoice")) {
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
					if (questionType.selectionType.GetType().Name.Equals("WordChoice")) {
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
					 
						answersList.Add (questionList [randomize].answer);
						question = questionList [randomize].definition;
						questionViable = true;

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

			if (questionType.selectionType.GetType().Name.Equals("SlotMachine")) {
				if (questionList [randomize].answer.Length > 6) {
					questionViable = false;
				} 
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
		return wrongChoice;
	}

	public static QuestionTypeModel GetQuestionType(string selection){

		Dictionary<QuestionSystemEnums.TargetType,int> targetDictionary = new Dictionary<QuestionSystemEnums.TargetType,int> ();
		targetDictionary.Add (QuestionSystemEnums.TargetType.Definition, 1);
		targetDictionary.Add (QuestionSystemEnums.TargetType.Synonym, 1);
		targetDictionary.Add (QuestionSystemEnums.TargetType.Antonym, 1);
		targetDictionary.Add (QuestionSystemEnums.TargetType.Association, 1);
		QuestionTypeModel typeModel = null;
		switch(selection){
		case "SelectLetter":
			typeModel = new QuestionTypeModel (
//				QuestionSystemEnums.TargetType.Definition,
				QuestionGenerator.GetTargetWay(targetDictionary),
				QuestionSystemController.Instance.partAnswer.fillAnswer,
				QuestionSystemController.Instance.partSelection.selectLetter
			);
			break;
		case "Typing":
			typeModel = new QuestionTypeModel (
//				QuestionSystemEnums.TargetType.Definition,
				QuestionGenerator.GetTargetWay(targetDictionary),
				QuestionSystemController.Instance.partAnswer.fillAnswer,
				QuestionSystemController.Instance.partSelection.typing
			);
			break;
		case "ChangeOrderController":
			typeModel = new QuestionTypeModel (
//				QuestionSystemEnums.TargetType.Synonym,
				QuestionGenerator.GetTargetWay(targetDictionary),
				QuestionSystemController.Instance.partAnswer.showAnswer,
				QuestionSystemController.Instance.partSelection.changeOrder
			);
			break;
		case "WordChoice":
			targetDictionary.Remove (QuestionSystemEnums.TargetType.Definition);
			targetDictionary.Remove (QuestionSystemEnums.TargetType.Association);
			typeModel = new QuestionTypeModel (
//				QuestionSystemEnums.TargetType.Synonym,
				QuestionGenerator.GetTargetWay(targetDictionary),
				QuestionSystemController.Instance.partAnswer.noAnswer,
				QuestionSystemController.Instance.partSelection.wordChoice
			);
			break;
		case "SlotMachine":
			typeModel = new QuestionTypeModel (
//				QuestionSystemEnums.TargetType.Definition,
				QuestionGenerator.GetTargetWay(targetDictionary),
				QuestionSystemController.Instance.partAnswer.showAnswer,
				QuestionSystemController.Instance.partSelection.slotMachine
			);
			break;
		case "LetterLink":
			typeModel = new QuestionTypeModel (
//				QuestionSystemEnums.TargetType.Association,
				QuestionGenerator.GetTargetWay(targetDictionary),
				QuestionSystemController.Instance.partAnswer.showAnswer,
				QuestionSystemController.Instance.partSelection.letterLink
			);
			break;
		}
		return typeModel;
	}

}


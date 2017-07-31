using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// QUESTION SYSTEM UTILITY CLASS, BUILDING QUESTION
/// </summary>
public static class QuestionBuilder
{
	public static QuestionSystemEnums.QuestionType questionType;
	private static List<string> questionsDone = new List<string> ();
	private static List<QuestionRowModel> questionList = new List<QuestionRowModel> ();
	private static List<string> wrongChoices = new List<string> ();
	private static List<int> wrongChoicesDone = new List<int> ();
	public static int questionIndex = 1;

	public static void PopulateQuestion ()
	{
		
		questionList.Clear ();
		TextAsset csvData = SystemResourceController.Instance.LoadCSV ("QuestionSystemCsv");
		List<List<string>> csvQuestionList = CSVParser.Parse (csvData.ToString());

		for (int i = 1; i < csvQuestionList.Count; i++) {
			//0 FOR LEVELID, QUESTIONID FOR TESTING : LACKING VALUES
			questionList.Add (new QuestionRowModel (
				0,
				CSVParser.GetValueArrayFromKey(csvQuestionList, "answer")[i].ToString (),
				0,
				CSVParser.GetValueArrayFromKey(csvQuestionList, "definition")[i].ToString (),
				CSVParser.GetValueArrayFromKey(csvQuestionList, "synonym1")[i].ToString (),
				CSVParser.GetValueArrayFromKey(csvQuestionList, "synonym2")[i].ToString (),
				CSVParser.GetValueArrayFromKey(csvQuestionList, "antonym1")[i].ToString (),
				CSVParser.GetValueArrayFromKey(csvQuestionList, "antonym2")[i].ToString (),
				CSVParser.GetValueArrayFromKey(csvQuestionList, "clue1")[i].ToString (),
				CSVParser.GetValueArrayFromKey(csvQuestionList, "clue2")[i].ToString (),
				CSVParser.GetValueArrayFromKey(csvQuestionList, "clue3")[i].ToString (),
				CSVParser.GetValueArrayFromKey(csvQuestionList, "clue4")[i].ToString (),
				int.Parse(CSVParser.GetValueArrayFromKey(csvQuestionList, "de")[i].ToString()),
				int.Parse(CSVParser.GetValueArrayFromKey(csvQuestionList, "sy")[i].ToString()),
				int.Parse(CSVParser.GetValueArrayFromKey(csvQuestionList, "an")[i].ToString()),
				int.Parse(CSVParser.GetValueArrayFromKey(csvQuestionList, "cl")[i].ToString())
			));
			wrongChoices.Add (CSVParser.GetValueArrayFromKey(csvQuestionList, "answer")[i].ToString ()
			);

		
		}
	}

	public static QuestionModel GetQuestion (QuestionSystemEnums.QuestionType questiontype, ISelection selectionType)
	{
		
		int randomize = 0;
		bool questionViable = false;
		string question = "";
		List<string> answersList = new List<string> ();
		questionType = questiontype;
		int numOfQuestions = questionList.Count;
		int whileIndex = 0;
		while (!questionViable) {
			randomize = UnityEngine.Random.Range (0, questionList.Count);
			answersList.Clear ();

			switch (questionType) {
			case QuestionSystemEnums.QuestionType.Antonym:
				if (questionList [randomize].hasAntonym.ToString()=="1") {
					answersList.Add (questionList[randomize].antonym1);
					answersList.Add (questionList[randomize].antonym2);
					question = questionList [randomize].answer;
					questionViable = true;
				}
				break;
			case QuestionSystemEnums.QuestionType.Synonym:
				if (questionList [randomize].hasSynonym.ToString()=="1") {
					answersList.Add (questionList[randomize].synonym1);
					answersList.Add (questionList[randomize].synonym2);
					question = questionList [randomize].answer;
					questionViable = true;
				}
				break;

			case QuestionSystemEnums.QuestionType.Definition:
				if (questionList [randomize].hasDefinition.ToString()=="1") {
					if (selectionType.ToString ().Equals("SlotMachine (SlotMachine)")) {
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

			case QuestionSystemEnums.QuestionType.Association:
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
		QuestionModel questionGot = new QuestionModel (question, answersList.ToArray ());
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
		return wrongChoice;
	}

	public static QuestionTypeModel getQuestionType(string selection){
//		
		float[] selectionTypePercentage = new float[6];
		float randomizedPercentage = Random.Range(0.00f,1.0f);
		float percentageLeastDifference = 1.0f / selectionTypePercentage.Length;
		for (int i = 0; i < selectionTypePercentage.Length; i++) {
			selectionTypePercentage [i] = (1.0f / selectionTypePercentage.Length)* (i + 1) ;
			if ((randomizedPercentage - selectionTypePercentage [i] < percentageLeastDifference)) {
				percentageLeastDifference = randomizedPercentage - selectionTypePercentage [i];
			}
		}
		QuestionTypeModel typeModel = null;
		switch(selection){
		case "sellect":
			 typeModel = new QuestionTypeModel (
				QuestionSystemEnums.QuestionType.Definition,
				QuestionSystemController.Instance.partTarget.singleQuestion,
				QuestionSystemController.Instance.partAnswer.fillAnswer,
				QuestionSystemController.Instance.partSelection.selectLetter
			);
			break;
		case "typing":
			typeModel = new QuestionTypeModel (
				QuestionSystemEnums.QuestionType.Definition,
				QuestionSystemController.Instance.partTarget.singleQuestion,
				QuestionSystemController.Instance.partAnswer.fillAnswer,
				QuestionSystemController.Instance.partSelection.typing
			);
			break;
		case "change":
			typeModel = new QuestionTypeModel (
				QuestionSystemEnums.QuestionType.Synonym,
				QuestionSystemController.Instance.partTarget.singleQuestion,
				QuestionSystemController.Instance.partAnswer.showAnswer,
				QuestionSystemController.Instance.partSelection.changeOrder
			);
			break;
		case "word":
			typeModel = new QuestionTypeModel (
				QuestionSystemEnums.QuestionType.Synonym,
				QuestionSystemController.Instance.partTarget.singleQuestion,
				QuestionSystemController.Instance.partAnswer.noAnswer,
				QuestionSystemController.Instance.partSelection.wordChoice
			);
			break;
		case "slot":
			typeModel = new QuestionTypeModel (
				QuestionSystemEnums.QuestionType.Definition,
				QuestionSystemController.Instance.partTarget.singleQuestion,
				QuestionSystemController.Instance.partAnswer.noAnswer,
				QuestionSystemController.Instance.partSelection.slotMachine
			);
			break;
		case "letter":
			typeModel = new QuestionTypeModel (
				QuestionSystemEnums.QuestionType.Association,
				QuestionSystemController.Instance.partTarget.association,
				QuestionSystemController.Instance.partAnswer.showAnswer,
				QuestionSystemController.Instance.partSelection.letterLink
			);
			break;
		}




			
		return typeModel;
	}
}


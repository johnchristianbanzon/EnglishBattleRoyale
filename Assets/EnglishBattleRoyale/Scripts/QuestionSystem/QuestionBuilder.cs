using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// QUESTION SYSTEM UTILITY CLASS, BUILDING QUESTION
/// </summary>
public static class QuestionBuilder
{
	public static QuestionSystemEnums.QuestionType questionType;
	private static List<string> questionsDone = new List<string> ();
	private static List<QuestionListModel> questionList = new List<QuestionListModel> ();
	private static List<string> wrongChoices = new List<string> ();
	private static List<int> wrongChoicesDone = new List<int> ();
	public static List<Dictionary<string,System.Object>> parsedData = new List<Dictionary<string,System.Object>> ();
	public static int questionIndex = 1;

	public static void PopulateQuestion (string questionName)
	{
		questionList.Clear ();
		parsedData = CSVToDic.ConvertCSV (questionName);
		for (int listIndex = 0; listIndex < parsedData.Count - 1; listIndex++) {
			questionList.Add (new QuestionListModel (
				parsedData [listIndex] ["definition"].ToString (),
				parsedData [listIndex] ["answer"].ToString (),
				(parsedData [listIndex] ["synonym1"].ToString () + "/" + parsedData [listIndex] ["synonym2"]),
				(parsedData [listIndex] ["antonym1"].ToString () + "/" + parsedData [listIndex] ["antonym2"]),
				(parsedData [listIndex] ["clue1"].ToString () + "/" + parsedData [listIndex] ["clue2"].ToString () + "/" +
				parsedData [listIndex] ["clue3"].ToString () + "/" + parsedData [listIndex] ["clue4"].ToString ()),
				parsedData [listIndex] ["de"],  parsedData [listIndex] ["sy"], parsedData [listIndex] ["an"], parsedData [listIndex] ["cl"]
			));
			wrongChoices.Add (parsedData [listIndex] ["answer"].ToString ());

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
					string[] antonym = questionList [randomize].antonym.Split ('/');
					answersList.Add (antonym [0]);
					answersList.Add (antonym [1]);
					question = questionList [randomize].answer;
					questionViable = true;
				}
				break;
			case QuestionSystemEnums.QuestionType.Synonym:
				if (questionList [randomize].hasSynonym.ToString()=="1") {
					string[] synonym = questionList [randomize].synonym.Split ('/');
					answersList.Add (synonym [0]);
					answersList.Add (synonym [1]);
					question = questionList [randomize].answer;
					questionViable = true;
				}
				break;
			case QuestionSystemEnums.QuestionType.Definition:
				if (questionList [randomize].hasDefinition.ToString()=="1") {
					answersList.Add (questionList [randomize].answer);
					question = questionList [randomize].definition;
					questionViable = true;
				}
				break;
			case QuestionSystemEnums.QuestionType.Association:
				if (questionList [randomize].hasClues.ToString()=="1") {
					answersList.Add (questionList [randomize].answer);
					question = questionList [randomize].clues;
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
}


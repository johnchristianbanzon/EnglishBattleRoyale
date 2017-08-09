using System.Collections.Generic;
using System.Linq;
using System;

public static class QuestionGenerator
{
	public static FilteredQuestionModel[] GenerateQuestions (QuestionRowModel[] questionRowArray)
	{
		FilteredQuestionModel[] filteredQuestion = new FilteredQuestionModel[questionRowArray.Length];
		for (int i = 0; i < questionRowArray.Length; i++) {
//			filteredQuestion[i] = (new FilteredQuestionModel (questionRowArray [i], GetSelectWay (), GetTargetWay ()));
		}

		return filteredQuestion;
	}


	public static string GetPseudoRandomValue(Dictionary<string, int> ratios)
	{
		List<KeyValuePair<string, int>> numbers = new List<KeyValuePair<string, int>> ();
		int sum = 0;
		foreach (KeyValuePair<string, int> pair in ratios) {
			int ratio = pair.Value;
			sum = sum + ratio;
			numbers.Add (pair);
		}
		int randomNumber = new Random ().Next (0, sum);
		int total = 0;
		for (int i = 0; i < numbers.Count; i++) {
			total = total + numbers [i].Value;
			if (randomNumber < total) {
				return numbers [i].Key; 
			}
		}
		return "";
	}

	public static QuestionSystemEnums.QuestionType GetTargetWay (Dictionary<QuestionSystemEnums.QuestionType, int> ratios)
	{
		List<KeyValuePair<QuestionSystemEnums.QuestionType, int>> numbers = new List<KeyValuePair<QuestionSystemEnums.QuestionType, int>> ();
		int sum = 0;
		foreach (KeyValuePair<QuestionSystemEnums.QuestionType, int> pair in ratios) {
			int ratio = pair.Value;
			sum = sum + ratio;
			numbers.Add (pair);
		}
		int randomNumber = new Random ().Next (0, sum);
		int total = 0;
		for (int i = 0; i < numbers.Count; i++) {
			total = total + numbers [i].Value;
			if (randomNumber < total) {
				return numbers [i].Key; 
			}
		}
		return QuestionSystemEnums.QuestionType.Synonym;
	}

	private static QuestionSystemEnums.SelectionType GetSelectWay (Dictionary<QuestionSystemEnums.SelectionType, int> ratios)
	{
		List<KeyValuePair<QuestionSystemEnums.SelectionType, int>> numbers = new List<KeyValuePair<QuestionSystemEnums.SelectionType, int>> ();
		int sum = 0;
		foreach (KeyValuePair<QuestionSystemEnums.SelectionType, int> pair in ratios) {
			int ratio = pair.Value;
			sum = sum + ratio;
			numbers.Add (pair);
		}
		int randomNumber = new Random ().Next (0, sum);
		int total = 0;
		for (int i = 0; i < numbers.Count; i++) {
			total = total + numbers [i].Value;
			if (randomNumber < total) {
				return numbers [i].Key; 
			}
		}
		return QuestionSystemEnums.SelectionType.SelectLetter;
	}
}

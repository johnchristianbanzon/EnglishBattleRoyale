using System.Collections.Generic;
using System.Linq;

public static class QuestionGenerator
{
	public static FilteredQuestionModel[] GenerateQuestions (QuestionRowModel[] questionRowArray)
	{
		FilteredQuestionModel[] filteredQuestion = new FilteredQuestionModel[questionRowArray.Length];
		for (int i = 0; i < questionRowArray.Length; i++) {
			filteredQuestion[i] = (new FilteredQuestionModel (questionRowArray [i], GetSelectWay (), GetTargetWay ()));
		}

		return filteredQuestion;
	}

	private static QuestionSystemEnums.SelectionType GetSelectWay ()
	{
		QuestionSystemEnums.SelectionType questionSystemEnum = QuestionSystemEnums.SelectionType.ChangeOrder;
//		int[] weight;


//		weight[0] = MyConst.GetQuestionConstKeyValue ("WeightSelectLetter");
//		weight[1] = MyConst.GetQuestionConstKeyValue ("WeightWordChoice");
//		weight[2] = MyConst.GetQuestionConstKeyValue ("WeightChangeOrder");
//		weight[3] = MyConst.GetQuestionConstKeyValue ("WeighTyping");
//		weight[4] = MyConst.GetQuestionConstKeyValue ("WeightLetterLink");
//		weight[5] = MyConst.GetQuestionConstKeyValue ("WeightSlotMachine");

//		int sum = weight.Sum();



		return questionSystemEnum;
	}

	private static  QuestionSystemEnums.QuestionType GetTargetWay ()
	{
		QuestionSystemEnums.QuestionType questionSystemEnum = QuestionSystemEnums.QuestionType.Antonym;

		return questionSystemEnum;
	}
}

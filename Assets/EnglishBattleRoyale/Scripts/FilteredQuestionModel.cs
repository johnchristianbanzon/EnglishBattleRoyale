public class FilteredQuestionModel
{
	QuestionRowModel questionRow;
	QuestionSystemEnums.SelectionType selectWay;
	QuestionSystemEnums.QuestionType targetWay;

	public FilteredQuestionModel (
		QuestionRowModel questionRow,
		QuestionSystemEnums.SelectionType selectWay,
		QuestionSystemEnums.QuestionType targetWay)
	{
		this.questionRow = questionRow;
		this.selectWay = selectWay;
		this.targetWay = targetWay;
	
	}
}

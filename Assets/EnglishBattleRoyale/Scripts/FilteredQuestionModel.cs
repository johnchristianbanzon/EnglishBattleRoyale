public class FilteredQuestionModel
{
	QuestionRowModel questionRow;
	QuestionSystemEnums.SelectionType selectWay;
	QuestionSystemEnums.TargetType targetWay;

	public FilteredQuestionModel (
		QuestionRowModel questionRow,
		QuestionSystemEnums.SelectionType selectWay,
		QuestionSystemEnums.TargetType targetWay)
	{
		this.questionRow = questionRow;
		this.selectWay = selectWay;
		this.targetWay = targetWay;
	
	}
}

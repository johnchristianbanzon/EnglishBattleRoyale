public class FilteredQuestionModel
{
	public QuestionRowModel questionRow;
	public QuestionSystemEnums.SelectionType selectWay;
	public QuestionSystemEnums.TargetType targetWay;

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

public class QuestionTypeModel
{
	public QuestionSystemEnums.TargetType questionCategory;
	public IAnswer answerType;
	public ISelection selectionType;


	public QuestionTypeModel (
		QuestionSystemEnums.TargetType questionCategory,
		IAnswer answerType,
		ISelection selectionType)
	{
		this.questionCategory = questionCategory;
		this.answerType = answerType;
		this.selectionType = selectionType;
	}

}

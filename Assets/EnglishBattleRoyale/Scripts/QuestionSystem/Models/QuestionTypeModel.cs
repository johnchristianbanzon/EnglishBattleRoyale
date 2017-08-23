public class QuestionTypeModel
{
	public QuestionSystemEnums.TargetType questionCategory;
	public QuestionSystemEnums.ContentLevel contentLevel;
	public IAnswer answerType;
	public ISelection selectionType;


	public QuestionTypeModel (
		QuestionSystemEnums.TargetType questionCategory,
		QuestionSystemEnums.ContentLevel contentLevel,
		IAnswer answerType,
		ISelection selectionType)
	{
		this.questionCategory = questionCategory;
		this.contentLevel = contentLevel;
		this.answerType = answerType;
		this.selectionType = selectionType;
	}

}

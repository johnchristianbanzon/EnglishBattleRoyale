public class QuestionModel{
	public QuestionTypeModel questionType;
	public string question;
	public string[] answers;
	public double idealTime;
	public QuestionModel(QuestionTypeModel questionType,string question, string[] answers, double idealTime){
		this.questionType = questionType;
		this.answers = answers;
		this.question = question;
		this.idealTime = idealTime;
	}

}

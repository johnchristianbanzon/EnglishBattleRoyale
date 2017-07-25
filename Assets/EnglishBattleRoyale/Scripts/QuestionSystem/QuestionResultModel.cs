public class QuestionResultModel {

	public int questionId;
	public int timePassed;
	public int numberHints;
	public bool isCorrect;
	public bool isSpeedy;

	public QuestionResultModel(int questionId, int timePassed, int numberHints, bool isCorrect, bool isSpeedy){
		this.questionId = questionId;
		this.timePassed = timePassed;
		this.numberHints = numberHints;
		this.isCorrect = isCorrect;
		this.isSpeedy = isSpeedy;
	}
}

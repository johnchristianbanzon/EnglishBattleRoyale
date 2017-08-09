public class QuestionResultModel {

	public int questionId;
	public int timePassed;
	public int numberHints;
	public bool isCorrect;
	public QuestionSystemEnums.SpeedyType speedyType;

	public QuestionResultModel(int questionId, int timePassed, int numberHints, bool isCorrect, QuestionSystemEnums.SpeedyType speedyType){
		this.questionId = questionId;
		this.timePassed = timePassed;
		this.numberHints = numberHints;
		this.isCorrect = isCorrect;
		this.speedyType = speedyType;
	}
}

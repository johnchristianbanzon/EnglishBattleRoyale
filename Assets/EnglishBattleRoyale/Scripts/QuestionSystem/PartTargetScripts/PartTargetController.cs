using UnityEngine;
using UnityEngine.UI;
public class PartTargetController : MonoBehaviour {
	public SingleQuestion singleQuestion;
	public Association association;
	public Text targetTypeText;
	public void DeployPartTarget(ITarget targetType, string targetString){
		targetType.ShowTargetType (targetString);
	}

	public void ChangeTargetColor(QuestionSystemEnums.TargetType targetType){
		switch(targetType){
		case QuestionSystemEnums.TargetType.Antonym:
			targetTypeText.color = new Color32 (239,118,122,255);
			break;
		case QuestionSystemEnums.TargetType.Association:
			targetTypeText.color = new Color32 (69,105,144,255);
			break;
		case QuestionSystemEnums.TargetType.Definition:
			targetTypeText.color = new Color32 (255,196,61,255);
			break;
		case QuestionSystemEnums.TargetType.Synonym:
			targetTypeText.color = new Color32 (73,220,177,255);
			break;
		}
	}
}

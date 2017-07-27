using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartQuestionController: MonoBehaviour
{
	public GameObject questionSelect;
	public ISelection[] selectionTypes = new ISelection[6];

	private void OnEndQuestion (int gp, int qtimeLeft)
	{
		QuestionStart (gp, qtimeLeft);
	}

	private void OnEndSelectQuestionTime ()
	{
		HideUI ();
		QuestionSystemController.Instance.StartQuestionRound(GetQuestionType(selectionTypes[0]), delegate(List<QuestionResultModel> onRoundResult) {
			// RETURNS LIST OF QUESTIONRESULTS
		});
	}

	public QuestionTypeModel GetQuestionType (ISelection partSelection)
	{
		QuestionTypeModel questionType = null;

		questionType.questionCategory = QuestionSystemEnums.QuestionType.Definition;
		questionType.targetType = QuestionSystemController.Instance.partTarget.singleQuestion;
		questionType.answerType = QuestionSystemController.Instance.partAnswer.fillAnswer;
		questionType.selectionType = partSelection;

		return questionType;

	}

	public void OnStartPhase ()
	{
		ScreenBattleController.Instance.partSkill.ShowAutoActivateButtons (true);
		Debug.Log ("Starting Answer Phase");
		RPCDicObserver.AddObserver (PartAnswerIndicatorController.Instance);
		GameTimeManager.HasAnswered (false);

		GameTimeManager.StartSelectQuestionTimer (OnEndSelectQuestionTime);
		questionSelect.SetActive (true);

	}

	public void OnEndPhase ()
	{
		RPCDicObserver.RemoveObserver (PartAnswerIndicatorController.Instance);
		if (questionSelect.activeInHierarchy) {
			questionSelect.SetActive (false);
		}
	}



	public void OnQuestionSelect (int questionNumber)
	{
		GameTimeManager.StopTimer ();
		GameTimeManager.HasAnswered (true);
		questionSelect.SetActive (false);
		//call question callback here
//		QuestionSystemController.Instance.StartQuestionRound(questionNumber,OnEndQuestion);

	}

	private void QuestionStart (int gp, int qtimeLeft)
	{
		Debug.Log (gp);
		Debug.Log (SystemGlobalDataController.Instance.gpEarned);

		SystemGlobalDataController.Instance.gpEarned = gp;
		ScreenBattleController.Instance.partState.PlayerGP += gp;
		SystemFirebaseDBController.Instance.AnswerPhase (qtimeLeft, gp);

		//for mode 3
		ScreenBattleController.Instance.partSkill.CheckSkillActivate ();

		if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode2) {
			if (SystemGlobalDataController.Instance.skillChosenCost <= ScreenBattleController.Instance.partState.PlayerGP) {
				if (SystemGlobalDataController.Instance.playerSkillChosen != null) {
					SystemGlobalDataController.Instance.playerSkillChosen ();
				}
			} else {
				Debug.Log ("LESS GP CANNOT ACTIVATE SKILL");
			}
		} 
		HideUI ();
	}

	private void HideUI ()
	{
		questionSelect.SetActive (false);
		GameTimeManager.ToggleTimer (false);

	}

}


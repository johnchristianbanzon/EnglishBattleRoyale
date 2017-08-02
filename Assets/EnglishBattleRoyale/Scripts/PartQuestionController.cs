using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartQuestionController: MonoBehaviour
{
	private List<QuestionResultModel> questionResultList;
	private GameObject questionSystem;


	public void OnStartPhase ()
	{
		RPCDicObserver.AddObserver (PartAnswerIndicatorController.Instance);
		QuestionBuilder.PopulateQuestion ();

		string[] questionTypes = new string[6]{ "sellect", "typing", "change", "word", "slot", "letter" };
		questionSystem = SystemResourceController.Instance.LoadPrefab ("QuestionSystemController", this.gameObject);
		QuestionSystemController.Instance.StartQuestionRound (
			QuestionBuilder.getQuestionType (questionTypes [UnityEngine.Random.Range (0, questionTypes.Length)])
		, delegate(List<QuestionResultModel> result) {
			questionResultList = result;
		}
		);
	}

	public void OnEndPhase ()
	{
		Destroy (questionSystem);
		RPCDicObserver.RemoveObserver (PartAnswerIndicatorController.Instance);
	}

}


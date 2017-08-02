using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class PartQuestionController: MonoBehaviour
{
	private List<QuestionResultModel> questionResultList;
	private GameObject questionSystem;


	public void OnStartPhase ()
	{
		Debug.Log ("Starting Answer Phase");
		ScreenBattleController.Instance.partCharacter.ShowAutoActivateButtons (true);
		RPCDicObserver.AddObserver (PartAnswerIndicatorController.Instance);
		QuestionBuilder.PopulateQuestion ();

		string[] questionTypes = new string[6]{ "sellect", "typing", "change", "word", "slot", "letter" };

		questionSystem = SystemResourceController.Instance.LoadPrefab ("QuestionSystem", this.gameObject);
		QuestionSystemController.Instance.StartQuestionRound (
			QuestionBuilder.getQuestionType (questionTypes [UnityEngine.Random.Range (0, questionTypes.Length)])
			, delegate(List<QuestionResultModel> resultList) {

			//callback here
			questionResultList = resultList;

			int correctCount = questionResultList.Count (p => p.isCorrect == true);
			int speedyCount = questionResultList.Count (p => p.isSpeedy == true);
			QuestionResultCountModel questionResultCount = new QuestionResultCountModel (correctCount, speedyCount);
			string param = JsonUtility.ToJson (questionResultCount);
			SystemFirebaseDBController.Instance.AnswerPhase (param);

			Debug.Log("hello " +param);
		}
		);
	}

	public void OnEndPhase ()
	{
		RPCDicObserver.RemoveObserver (PartAnswerIndicatorController.Instance);
	}

}


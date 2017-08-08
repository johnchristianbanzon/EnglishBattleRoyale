using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class PartQuestionController: MonoBehaviour
{
	private List<QuestionResultModel> questionResultList;
	private GameObject questionSystem;

	void Start ()
	{
		QuestionBuilder.PopulateQuestion ();
	}

	public void OnStartPhase ()
	{
		ScreenBattleController.Instance.partCharacter.ShowAutoActivateButtons (true);
		RPCDicObserver.AddObserver (PartAnswerIndicatorController.Instance);


		string[] questionTypes = new string[6]{ "select", "typing", "change", "word", "slot", "letter" };

		questionSystem = SystemResourceController.Instance.LoadPrefab ("QuestionSystem", this.gameObject);
		QuestionSystemController.Instance.StartQuestionRound (
			QuestionBuilder.getQuestionType (questionTypes [UnityEngine.Random.Range (0, questionTypes.Length)])
			, delegate(List<QuestionResultModel> resultList) {

			//callback here
			questionResultList = resultList;

			int correctCount = questionResultList.Count (p => p.isCorrect == true);
			int speedyCount = questionResultList.Count (p => p.isSpeedy == true);

			//bonus get from answers
			float gpGainBonus = correctCount * 2;
			float gpDamageBonus = correctCount;
			float speedygpGainBonus = correctCount * 2;
			float speedyDamageBonus = correctCount * 2;

			ScreenBattleController.Instance.partState.player.playerGP += gpGainBonus + speedygpGainBonus;
			Debug.Log ("GP GAINED A TOTAL OF " + (gpGainBonus + speedygpGainBonus));

			ScreenBattleController.Instance.partState.player.playerBaseDamage += gpDamageBonus + speedyDamageBonus;
			Debug.Log ("BONUS PLAYER DAMAGE NOW INCREASED TO " + ScreenBattleController.Instance.partState.player.playerBaseDamage);
			//

			
			QuestionResultCountModel questionResultCount = new QuestionResultCountModel (correctCount, speedyCount);
			string param = JsonUtility.ToJson (questionResultCount);
			SystemFirebaseDBController.Instance.AnswerPhase (param);

		}
		);
	}

	public void OnEndPhase ()
	{
		RPCDicObserver.RemoveObserver (PartAnswerIndicatorController.Instance);
	}

}


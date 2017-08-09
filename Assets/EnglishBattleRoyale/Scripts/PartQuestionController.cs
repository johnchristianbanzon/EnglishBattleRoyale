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
			int awesomeSpeedyCount = questionResultList.Count (p => p.speedyType == QuestionSystemEnums.SpeedyType.Awesome);
				int goodSpeedyCount = questionResultList.Count (p => p.speedyType == QuestionSystemEnums.SpeedyType.Good);
				int rottenSpeedyCount = questionResultList.Count (p => p.speedyType == QuestionSystemEnums.SpeedyType.Rotten);
			//:TO-DO count speedyawesome and speedygood and include in computation
			//bonus get from answers
			float correctGPBonus = correctCount * GameManager.gameSettings.correctGPBonus;
			float correctDamageBonus = correctCount * GameManager.gameSettings.correctDamageBonus;
				float speedyAwesomeGPBonus = awesomeSpeedyCount * GameManager.gameSettings.speedyAwesomeGPBonus;
				float speedyAwesomeDamageBonus = awesomeSpeedyCount * GameManager.gameSettings.speedyAwesomeDamageBonus;
//				float speedyGoodGPBonus = correctCount * GameManager.gameSettings.speedyGoodGPBonus;
//				float speedyGoodDamageBonus = correctCount * GameManager.gameSettings.speedyGoodDamageBonus;

			ScreenBattleController.Instance.partState.player.playerGP += correctGPBonus + speedyAwesomeGPBonus;
			Debug.Log ("GP GAINED A TOTAL OF " + (correctGPBonus + speedyAwesomeGPBonus));

			ScreenBattleController.Instance.partState.player.playerBaseDamage += correctDamageBonus + speedyAwesomeDamageBonus;
			Debug.Log ("BONUS PLAYER DAMAGE NOW INCREASED TO " + ScreenBattleController.Instance.partState.player.playerBaseDamage);
			//

			
				QuestionResultCountModel questionResultCount = new QuestionResultCountModel (correctCount,awesomeSpeedyCount,goodSpeedyCount,rottenSpeedyCount);
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


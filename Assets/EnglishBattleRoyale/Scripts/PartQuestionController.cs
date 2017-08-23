\using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class PartQuestionController: MonoBehaviour
{
	private List<QuestionResultModel> questionResultList;

	void Start ()
	{
		IQuestionProvider provider = new QuestionCSVProvider ();
		QuestionBuilder.PopulateQuestion (provider);
	}

	public void OnStartPhase ()
	{
		ScreenBattleController.Instance.partCharacter.ShowAutoActivateButtons (true);
		RPCDicObserver.AddObserver (PartAnswerIndicatorController.Instance);


		string[] questionTypes = new string[6]{ "select", "typing", "change", "word", "slot", "letter" };

		SystemResourceController.Instance.LoadPrefab ("QuestionSystem", this.gameObject);
		QuestionSystemController.Instance.StartQuestionRound (
			QuestionBuilder.GetQuestionType (questionTypes [UnityEngine.Random.Range (0, questionTypes.Length)])
			, delegate(List<QuestionResultModel> resultList) {

			//callback here
			questionResultList = resultList;

			int correctCount = questionResultList.Count (p => p.isCorrect == true);
			int awesomeSpeedyCount = questionResultList.Count (p => p.speedyType == QuestionSystemEnums.SpeedyType.Awesome);
			int goodSpeedyCount = questionResultList.Count (p => p.speedyType == QuestionSystemEnums.SpeedyType.Good);
			int rottenSpeedyCount = questionResultList.Count (p => p.speedyType == QuestionSystemEnums.SpeedyType.Rotten);
			//:TO-DO count speedyawesome and speedygood and include in computation
			//bonus get from answers
				float correctGPBonus = correctCount *  MyConst.gameSettings.correctGPBonus;
				float correctDamageBonus = correctCount * MyConst.gameSettings.correctDamageBonus;
				float speedyAwesomeGPBonus = awesomeSpeedyCount * MyConst.gameSettings.speedyAwesomeGPBonus;
				float speedyAwesomeDamageBonus = awesomeSpeedyCount * MyConst.gameSettings.speedyAwesomeDamageBonus;
				float speedyGoodGPBonus = correctCount * MyConst.gameSettings.speedyGoodGPBonus;
				float speedyGoodDamageBonus = correctCount * MyConst.gameSettings.speedyGoodDamageBonus;

			ScreenBattleController.Instance.partState.player.playerGP += correctGPBonus + speedyAwesomeGPBonus + speedyGoodGPBonus;
			Debug.Log ("GP GAINED A TOTAL OF " + (correctGPBonus + speedyAwesomeGPBonus));

			ScreenBattleController.Instance.partState.player.playerTD = ScreenBattleController.Instance.partState.player.playerBD + correctDamageBonus + speedyAwesomeDamageBonus + speedyGoodDamageBonus;
			Debug.Log ("BONUS PLAYER DAMAGE ADDED " + (correctDamageBonus + speedyAwesomeDamageBonus + speedyGoodDamageBonus));
			//

			
			//send answer results to firebase
			QuestionResultCountModel questionResultCount = new QuestionResultCountModel (correctCount, awesomeSpeedyCount, goodSpeedyCount, rottenSpeedyCount);
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


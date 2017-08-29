using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class PartQuestionController: MonoBehaviour
{
	public GameObject questionSystem = null;
	public GameObject questionTypePopUp;
	public QuestionTypeModel questionType;
	QuestionSystemController questionSystemController = null;
	void Start ()
	{
		IQuestionProvider provider = new QuestionCSVProvider ();
		QuestionBuilder.PopulateQuestion (provider);
	}

	public void DebugStartQuestion(){
		StartQuestion ();
		Destroy (questionSystemController.popUpSelectionIndicator);
	}

	private void ReturnQuestionSystemPosition(){
		transform.localPosition = new Vector3 (-720,0,0);
	}

	public void hideScrollUI(){
		TweenFacade.TweenMoveTo(transform,new Vector2(transform.position.x+800f,transform.position.y),0.6f);
		Invoke ("ReturnQuestionSystemPosition", 0.7f);
	}
		
	public void OnStartPhase ()
	{
		ScreenBattleController.Instance.partCharacter.ShowAutoActivateButtons (true);
		RPCDicObserver.AddObserver (PartAnswerIndicatorController.Instance);
		if (questionSystem == null) {
			questionSystem = SystemResourceController.Instance.LoadPrefab ("QuestionSystem", this.gameObject);
			Debug.Log(questionSystem.name);
		} else {
			questionSystem.SetActive (true);
		}
		string[] questionTypes = new string[6]{ "SelectLetter", "Typing", "ChangeOrderController", "WordChoice", "SlotMachine", "LetterLink" };
//		QuestionSystemController.Instance.ShowPopUP (questionTypes [UnityEngine.Random.Range (0, questionTypes.Length)]);

		Invoke("StartQuestion",2.0f);
	}

	private void StartQuestion(){
		QuestionSystemController.Instance.StartQuestionRound (
			questionType
			, delegate(List<QuestionResultModel> resultList) {
				
				int correctCount = resultList.Count (p => p.isCorrect == true);
				int awesomeSpeedyCount = resultList.Count (p => p.speedyType == QuestionSystemEnums.SpeedyType.Awesome);
				int goodSpeedyCount = resultList.Count (p => p.speedyType == QuestionSystemEnums.SpeedyType.Good);
				int rottenSpeedyCount = resultList.Count (p => p.speedyType == QuestionSystemEnums.SpeedyType.Rotten);

				//:TO-DO count speedyawesome and speedygood and include in computation
				//bonus get from answers
				float correctGPBonus = correctCount * MyConst.gameSettings.correctGPBonus;
				float correctDamageBonus = correctCount * MyConst.gameSettings.correctDamageBonus;
				float speedyAwesomeGPBonus = awesomeSpeedyCount * MyConst.gameSettings.speedyAwesomeGPBonus;
				float speedyAwesomeDamageBonus = awesomeSpeedyCount * MyConst.gameSettings.speedyAwesomeDamageBonus;
				float speedyGoodGPBonus = correctCount * MyConst.gameSettings.speedyGoodGPBonus;
				float speedyGoodDamageBonus = correctCount * MyConst.gameSettings.speedyGoodDamageBonus;

				ScreenBattleController.Instance.partState.player.playerGP += correctGPBonus + speedyAwesomeGPBonus + speedyGoodGPBonus;
				Debug.Log ("GP GAINED A TOTAL OF " + (correctGPBonus + speedyAwesomeGPBonus));

				ScreenBattleController.Instance.partState.player.playerTD = ScreenBattleController.Instance.partState.player.playerBD + correctDamageBonus + speedyAwesomeDamageBonus + speedyGoodDamageBonus;
				Debug.Log ("BONUS PLAYER DAMAGE ADDED " + (correctDamageBonus + speedyAwesomeDamageBonus + speedyGoodDamageBonus));


				//send answer results to firebase
				QuestionResultCountModel questionResultCount = new QuestionResultCountModel (correctCount, awesomeSpeedyCount, goodSpeedyCount, rottenSpeedyCount);
				string param = JsonUtility.ToJson (questionResultCount);
				SystemFirebaseDBController.Instance.AnswerPhase (param);
				QuestionSystemController.Instance.HideQuestionParts();
				QuestionSystemReturnCallback();
			}
		);
	}

	public void QuestionSystemReturnCallback(){
		/*
		TweenFacade.TweenMoveTo (questionSystemController.scrollBody.transform,new Vector2(questionSystemController.scrollBody.transform.position.x,questionSystemController.scrollBody.transform.position.y + 924f),0.5f);
		questionSystemController.HideQuestionParts();
		Invoke("hideScrollUI",0.5f);*/
	}
	public void OnEndPhase ()
	{
		RPCDicObserver.RemoveObserver (PartAnswerIndicatorController.Instance);
	}

}


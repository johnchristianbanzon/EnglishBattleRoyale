using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class PartQuestionController: MonoBehaviour
{
	private List<QuestionResultModel> questionResultList;
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
		if (questionSystem.Equals(null)) {
			questionSystem = SystemResourceController.Instance.LoadPrefab ("QuestionSystem", this.gameObject);
		} else {
			questionSystem.SetActive (true);
		}


		string[] questionTypes = new string[6]{ "select", "typing", "change", "word", "slot", "letter" };
		questionType = QuestionBuilder.GetQuestionType (questionTypes [UnityEngine.Random.Range (0, questionTypes.Length)]);

		string popUpName = "";
		switch (questionType.selectionType.GetType().Name) {
		case "WordChoice":
			popUpName = "PopUpWordChoice";
			break;
		case "SelectLetter":
			popUpName = "PopUpSelectLetter";
			break;
		case "ChangeOrderController":
			popUpName = "PopUpChangeOrder";
			break;
		case "Typing":
			popUpName = "PopUpTyping";
			break;
		case "SlotMachine":
			popUpName = "PopUpSlotMachine";
			break;
		case "LetterLink":
			popUpName = "PopUpLetterLink";
			break;
		}
		questionSystemController = QuestionSystemController.Instance;
		questionSystemController.popUpSelectionIndicator = SystemResourceController.Instance.LoadPrefab (popUpName, SystemPopupController.Instance.popUp);
		TweenFacade.TweenScaleToLarge (questionSystemController.popUpSelectionIndicator.transform, Vector3.one, 0.3f);
		questionSystemController.popUpSelectionIndicator.transform.position = Vector3.zero;
//		questionSystemController.scrollBody.transform.position = new Vector2 (transform.position.x,transform.position.y + 920f);

		TweenFacade.TweenMoveTo (transform, Vector3.zero, 0.5f);
		TweenFacade.TweenMoveTo (questionSystemController.scrollBody.transform,Vector3.zero,1.0f);

		Invoke("StartQuestion",2.0f);



	}

	private void StartQuestion(){
		
		QuestionSystemController.Instance.StartQuestionRound (
			questionType
			, delegate(List<QuestionResultModel> resultList) {
				//
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
				float speedyGoodGPBonus = correctCount * GameManager.gameSettings.speedyGoodGPBonus;
				float speedyGoodDamageBonus = correctCount * GameManager.gameSettings.speedyGoodDamageBonus;

				ScreenBattleController.Instance.partState.player.playerGP += correctGPBonus + speedyAwesomeGPBonus + speedyGoodGPBonus;
				Debug.Log ("GP GAINED A TOTAL OF " + (correctGPBonus + speedyAwesomeGPBonus));

				ScreenBattleController.Instance.partState.player.playerBaseDamage += correctDamageBonus + speedyAwesomeDamageBonus + speedyGoodDamageBonus;
				Debug.Log ("BONUS PLAYER DAMAGE ADDED " + (correctDamageBonus + speedyAwesomeDamageBonus + speedyGoodDamageBonus));
				//


				//send answer results to firebase
				QuestionResultCountModel questionResultCount = new QuestionResultCountModel (correctCount, awesomeSpeedyCount, goodSpeedyCount, rottenSpeedyCount);
				string param = JsonUtility.ToJson (questionResultCount);
				SystemFirebaseDBController.Instance.AnswerPhase (param);
				QuestionSystemReturnCallback();
			}
		);
	}

	public void QuestionSystemReturnCallback(){
		TweenFacade.TweenMoveTo (questionSystemController.scrollBody.transform,new Vector2(questionSystemController.scrollBody.transform.position.x,questionSystemController.scrollBody.transform.position.y + 924f),0.5f);
		questionSystemController.HideQuestionParts();
		Invoke("hideScrollUI",0.5f);
	}
	public void OnEndPhase ()
	{
		RPCDicObserver.RemoveObserver (PartAnswerIndicatorController.Instance);
	}

}


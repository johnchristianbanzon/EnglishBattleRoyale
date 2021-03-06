﻿using System.Collections.Generic;
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
		RPCDicObserver.AddObserver (PartAnswerIndicatorController.Instance);

		Invoke("StartQuestion",2.0f);
	}

	private void StartQuestion(){
		if (questionSystem == null) {
			questionSystem = SystemResourceController.Instance.LoadPrefab ("QuestionSystem", this.gameObject);

		} else {
			questionSystem.SetActive (true);
		}
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

			
				PlayerManager.SetIsPlayer(true);
				PlayerManager.Player.gp += correctGPBonus + speedyAwesomeGPBonus + speedyGoodGPBonus;
				PlayerManager.Player.td = PlayerManager.Player.bd + correctDamageBonus + speedyAwesomeDamageBonus + speedyGoodDamageBonus;
				PlayerManager.UpdateStateUI(true);


				//send answer results to firebase
				QuestionResultCountModel questionResultCount = new QuestionResultCountModel (correctCount, awesomeSpeedyCount, goodSpeedyCount, rottenSpeedyCount);
				string param = JsonUtility.ToJson (questionResultCount);
				SystemFirebaseDBController.Instance.AnswerPhase (param);
				QuestionSystemController.Instance.HideQuestionParts();
				QuestionSystemReturnCallback();

				ScreenBattleController.Instance.partCharacter.ShowCharacters(true);
		
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
		//show character selection
	}

}


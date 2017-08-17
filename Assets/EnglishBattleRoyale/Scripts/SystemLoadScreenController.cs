using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SystemLoadScreenController : SingletonMonoBehaviour<SystemLoadScreenController> {
	public GameObject loadScreen;

	private const string LOADING_SCREEN = "LoadScreenLoading";
	public float loadingScreenTweenTime = 0.2f;
	private GameObject loadingScreen;

	private const string WAIT_OPPONENT = "LoadScreenWaitOpponent";
	public float waitOpponentTweenTime = 0.2f;
	private GameObject waitOpponent;

	public void StartLoadingScreen(Action action){
		ClearAllLoadingScreen ();

		loadingScreen = SystemResourceController.Instance.LoadPrefab (LOADING_SCREEN,loadScreen);
		action ();
	}
		
	public void StopLoadingScreen(){
		Destroy (loadingScreen, loadingScreenTweenTime);
	}

	public void StartWaitOpponentScreen(){
		ClearAllLoadingScreen ();

		waitOpponent = SystemResourceController.Instance.LoadPrefab (WAIT_OPPONENT,loadScreen);
		TweenFacade.TweenStartWaitOpponent (waitOpponentTweenTime, waitOpponent);

	}

	public void StopWaitOpponentScreen(){
		Destroy (waitOpponent, waitOpponentTweenTime);
		TweenFacade.TweenStopWaitOpponent (waitOpponentTweenTime, waitOpponent);
	}



	public void ClearAllLoadingScreen(){
		for (int i = 0; i < loadScreen.transform.childCount; i++) {
			Destroy (loadScreen.transform.GetChild(i).gameObject);
		}
	}

}

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
		loadingScreen = SystemResourceController.Instance.LoadPrefab (LOADING_SCREEN,loadScreen);
		StartCoroutine (ActivateActionCoroutine(action));
	}

	IEnumerator ActivateActionCoroutine(Action action){
		yield return new WaitForSeconds (1);
		action ();
	}
		
	public void StopLoadingScreen(){
		Destroy (loadingScreen, loadingScreenTweenTime);
	}





}

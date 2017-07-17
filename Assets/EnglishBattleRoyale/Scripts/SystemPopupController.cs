﻿using UnityEngine;
using System.Collections.Generic;

public class SystemPopupController : SingletonMonoBehaviour<SystemPopupController> {
	public GameObject popUp;
	private GameObject popUpObject;
	public GameObject backUnclickable;
//	private List<GameObject> nowShownPopUp = new List<GameObject> ();

	public void ShowPopUp(string screenName){
		ClearAllPopUp ();
		BackUnclickable (true);
		popUpObject = SystemPrefabController.Instance.LoadPrefab (screenName,popUp);
	}

	public void ClearPopUp(string popUpName){
		Destroy (popUp.transform.Find(popUpName).gameObject);
	}

	public void ClearAllPopUp(){
		BackUnclickable (false);
		for (int i = 0; i < popUp.transform.childCount; i++) {
			Destroy (popUp.transform.GetChild(i).gameObject);
		}
	}

	private void BackUnclickable(bool isUnClickable){
		backUnclickable.SetActive (isUnClickable);
	}

}

using UnityEngine;
using System.Collections.Generic;

public class SystemPopupController : SingletonMonoBehaviour<SystemPopupController> {
	public GameObject popUp;
	private GameObject popUpObject;
//	private List<GameObject> nowShownPopUp = new List<GameObject> ();

	public GameObject ShowPopUp(string screenName){
		ClearAllPopUp ();
		popUpObject = SystemResourceController.Instance.LoadPrefab (screenName,popUp);
		return popUpObject;
	}

	public void ClearPopUp(string popUpName){
		Destroy (popUp.transform.Find(popUpName).gameObject);
	}

	public void ClearAllPopUp(){
		for (int i = 1; i < popUp.transform.childCount; i++) {
			Destroy (popUp.transform.GetChild(i).gameObject);
		}
	}

}

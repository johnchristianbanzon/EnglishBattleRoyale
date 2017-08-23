using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour {
	public GameObject debugButton;
	public GameObject debugContainer;
	public GameObject playerSettings;
	public GameObject gameSettings;

	public void ShowContainer(){
		if (debugContainer.activeInHierarchy) {
			debugContainer.SetActive (false);
		} else {
			debugContainer.SetActive (true);
		}
	}

	public void ShowPlayerSettings(){
		playerSettings.SetActive (true);
	}

	public void ShowGameSettings(){
		gameSettings.SetActive (true);
	}

	public void OnDrag ()
	{
		Vector2 pos = new Vector2(0,0);
		Canvas myCanvas = SystemGlobalDataController.Instance.gameCanvas; 
		RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);

		debugButton.transform.position = myCanvas.transform.TransformPoint(pos);
	}
}

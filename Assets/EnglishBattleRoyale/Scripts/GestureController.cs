using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GestureController : SingletonMonoBehaviour<GestureController>, IRPCDicObserver
{
	public GameObject gestureButtonContainer;
	public Sprite closeImage;
	public Sprite gestureImage;
	public GameObject gestureButton;
	private Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();

	public void ShowGestureButtons (Transform button)
	{
		if (!gestureButtonContainer.activeInHierarchy) {
			gestureButtonContainer.SetActive (true);
			TweenLogic.TweenScaleToLarge (gestureButtonContainer.transform, new Vector3(1,1,1), 0.4f);

			button.gameObject.GetComponent<Image> ().sprite = closeImage;
		} else {
//			TweenController.TweenScaleToLarge (gestureButtonContainer.transform, new Vector3(1,1,1), 0.2f);
			button.gameObject.GetComponent<Image> ().sprite = gestureImage;
			Invoke ("ScaleToSmall", 0.05f);

		}
	}

	public void ScaleToSmall(){
		TweenLogic.TweenScaleToSmall (gestureButtonContainer.transform, new Vector3(0.7f,0.7f,0.7f), 0.1f);
		Invoke ("HideGestureButton", 0.05f);
	}
	public void HideGestureButton ()
	{
		gestureButtonContainer.SetActive (false);
	}


	public void ShowPlayerGesture1 ()
	{
		ShowGesture (true, "Gesture1");

		SendGesture (1);
	}

	public void ShowPlayerGesture2 ()
	{
		ShowGesture (true, "Gesture2");
		SendGesture (2);
	}

	public void ShowPlayerGesture3 ()
	{
		ShowGesture (true, "Gesture3");
		SendGesture (3);
	}

	public void ShowPlayerGesture4 ()
	{
		ShowGesture (true, "Gesture4");
		SendGesture (4);
	}

	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		try{
		Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
		if (rpcReceive.ContainsKey ("param")) {
			bool userHome = (bool)rpcReceive ["userHome"];
			GameData.Instance.attackerBool = userHome;

			Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive ["param"];
			if (param.ContainsKey ("Gesture")) {
				string stringParam = param ["Gesture"].ToString ();
				if (GameData.Instance.attackerBool.Equals (!GameData.Instance.isHost))
					SetEnemyGesture (stringParam);
			}
		}
		}
		catch(System.Exception e){
			//do something with exception in future
		}

	}

	public void SetEnemyGesture (string enemyGesture)
	{
		Dictionary<string, System.Object> gestureParam = JsonConverter.JsonStrToDic (enemyGesture);
		foreach (KeyValuePair<string, System.Object> gesture in gestureParam) {

			switch (int.Parse (gesture.Value.ToString ())) {
			case 1:
				ShowGesture (false, "Gesture1");
				break;
			case 2:
				ShowGesture (false, "Gesture2");
				break;
			case 3:
				ShowGesture (false, "Gesture3");
				break;
			case 4:
				ShowGesture (false, "Gesture4");
				break;
			}

		}
	}

	private void SendGesture (int gestureNumber)
	{
		gestureButton.GetComponent<Image> ().sprite = gestureImage;
		param [ParamNames.Gesture.ToString ()] = gestureNumber;
		SystemFirebaseDBController.Instance.SetGestureParam (new GestureModel (JsonConverter.DicToJsonStr (param).ToString ()));
	}

	//Hide gesture camera after displaying
	IEnumerator StartTimer (bool isPlayer)
	{
		yield return new WaitForSeconds (1.5f);
		if (!isPlayer) {
			CameraWorksController.Instance.HideGestureCamera ();
		}
	}

	private void ShowGesture (bool isPlayer, string param)
	{
		StartCoroutine (StartTimer (isPlayer));
		CharacterAnimationController.Instance.SetTriggerAnim (isPlayer, param);
		if (!isPlayer) {
			CameraWorksController.Instance.ShowGestureCamera ();
		}
	}


}

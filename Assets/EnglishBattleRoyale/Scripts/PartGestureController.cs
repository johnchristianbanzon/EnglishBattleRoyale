using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PartGestureController : MonoBehaviour, IRPCDicObserver
{
	public GameObject gestureButtonContainer;
	public Sprite closeImage;
	public Sprite gestureImage;
	public GameObject gestureButton;
	private Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();

	void Start ()
	{
		Init ();
	}

	private void Init(){
		RPCDicObserver.AddObserver (this);
	}

	public void ShowGestureButtons (Transform button)
	{
		if (!gestureButtonContainer.activeInHierarchy) {
			gestureButtonContainer.SetActive (true);
			TweenFacade.TweenScaleToLarge (gestureButtonContainer.transform, new Vector3 (1, 1, 1), 0.4f);

			button.gameObject.GetComponent<Image> ().sprite = closeImage;
		} else {
//			TweenController.TweenScaleToLarge (gestureButtonContainer.transform, new Vector3(1,1,1), 0.2f);
			button.gameObject.GetComponent<Image> ().sprite = gestureImage;
			Invoke ("ScaleToSmall", 0.05f);

		}
	}

	public void ScaleToSmall ()
	{
		TweenFacade.TweenScaleToSmall (gestureButtonContainer.transform, new Vector3 (0.7f, 0.7f, 0.7f), 0.1f);
		Invoke ("HideGestureButton", 0.05f);
	}

	public void HideGestureButton ()
	{
		gestureButtonContainer.SetActive (false);
	}


	public void ShowPlayerGesture (int gestureNum)
	{
		ShowGesture (true, "Gesture" + gestureNum);

		SendGesture (gestureNum);
	}


	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		try {
			Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;

			bool userHome = (bool)rpcReceive ["userHome"];
			SystemGlobalDataController.Instance.isSender = userHome;

			Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive ["param"];

			if (param.ContainsKey ("GestureRPC")) {
				
				GestureModel gesture =  JsonUtility.FromJson<GestureModel> (param ["GestureRPC"].ToString ());
				if (SystemGlobalDataController.Instance.isSender.Equals (!SystemGlobalDataController.Instance.isHost)) {
					SetEnemyGesture (gesture.param);
				}
			}

		} catch (System.Exception e) {
			//do something with exception in future
		}

	}

	public void SetEnemyGesture (string enemyGesture)
	{
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object> ();
		result ["GestureRPC"] = JsonUtility.ToJson (enemyGesture);

		Dictionary<string, System.Object> gestureParam = result;
		foreach (KeyValuePair<string, System.Object> gesture in gestureParam) {
			ShowGesture (false, "Gesture" + int.Parse (gesture.Value.ToString ()));
		}
	}

	private void SendGesture (int gestureNumber)
	{
		gestureButton.GetComponent<Image> ().sprite = gestureImage;
		param [ParamNames.Gesture.ToString ()] = gestureNumber;
		SystemFirebaseDBController.Instance.SetParam ("GestureRPC", (new GestureModel (JsonUtility.ToJson(param))));
	}

	//Hide gesture camera after displaying
	IEnumerator StartTimer (bool isPlayer)
	{
		yield return new WaitForSeconds (1.5f);
		if (!isPlayer) {
			ScreenBattleController.Instance.partCameraWorks.HideGestureCamera ();
		}
	}

	private void ShowGesture (bool isPlayer, string param)
	{
		StartCoroutine (StartTimer (isPlayer));
		ScreenBattleController.Instance.partAvatars.SetTriggerAnim (isPlayer, param);
		if (!isPlayer) {
			ScreenBattleController.Instance.partCameraWorks.ShowGestureCamera ();
		}
	}


}

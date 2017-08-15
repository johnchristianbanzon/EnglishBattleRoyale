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


	void Start ()
	{
		Init ();
	}

	private void Init ()
	{
		RPCDicObserver.AddObserver (this);
	}

	public void ShowGestureButtons (Transform button)
	{
		if (!gestureButtonContainer.activeInHierarchy) {
			gestureButtonContainer.SetActive (true);
			TweenFacade.TweenScaleToLarge (gestureButtonContainer.transform, new Vector3 (1, 1, 1), 0.4f);
			button.gameObject.GetComponent<Image> ().sprite = closeImage;
		} else {
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
		Invoke ("ScaleToSmall", 0.05f);
		ShowGesture (true, "gesture" + gestureNum);

		SendGesture (gestureNum);
	}


	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		try {
			Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;

			bool userHome = (bool)rpcReceive [MyConst.RPC_DATA_USERHOME];

			Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive [MyConst.RPC_DATA_PARAM];

			if (param.ContainsKey (MyConst.RPC_DATA_GESTURE)) {
				
				GestureModel gesture = JsonUtility.FromJson<GestureModel> (param [MyConst.RPC_DATA_GESTURE].ToString ());
				if (userHome.Equals (!GameManager.isHost)) {
					SetEnemyGesture (gesture.gestureNumber);
				}
			}

		} catch (System.Exception e) {
			//do something with exception in future
		}

	}

	public void SetEnemyGesture (int gestureNumber)
	{
		ShowGesture (false, "gesture" + gestureNumber);
	}

	private void SendGesture (int gestureNumber)
	{
		gestureButton.GetComponent<Image> ().sprite = gestureImage;
		GestureModel gesture = new GestureModel (gestureNumber);
		SystemFirebaseDBController.Instance.SetParam (MyConst.RPC_DATA_GESTURE, gesture);
	}

	//Hide gesture camera after displaying
	private void HideGesture ()
	{
		ScreenBattleController.Instance.partCameraWorks.HideGestureCamera ();
	}

	private void ShowGesture (bool isPlayer, string param)
	{
		ScreenBattleController.Instance.partAvatars.SetTriggerAnim (isPlayer, param);
		if (!isPlayer) {
			ScreenBattleController.Instance.partCameraWorks.ShowGestureCamera ();
			Invoke ("HideGesture", 1.5f);
		}
	}


}

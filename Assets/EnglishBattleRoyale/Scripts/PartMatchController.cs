using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
//using PapaParse.Net;
/* UI For searching matches */
public class PartMatchController : SingletonMonoBehaviour<PartMatchController>
{
	public GameObject gameRoomUI;
	public GameObject lobbyRoom;
	public GameObject gameRoomAssets;
	public ToggleGroup toggleGroup;
	public GameObject roomViews;
	private int timeLeft = 3;
	private bool stoptimer = true;
	public GameObject matchSword;
	public Text matchingText;
	public GameObject menu;
	public Button searchRoomButton;
	public GameObject matchingScreen;

	public void StartMatchingScreen ()
	{
		TweenLogic.TweenScaleToNormal (0.2f, matchingScreen);
	}

	public void StopMatchingScreen ()
	{
		TweenLogic.TweenScaleToZero (0.2f, matchingScreen);
	}

	public void SearchRoom ()
	{
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
		//matchSword.GetComponentInChildren<Animation> ().Play ("FindMatchAnimation");
//		matchSword.GetComponentInChildren<Animation> ().PlayQueued("FindMatchAnimation", QueueMode.PlayNow);
//		matchSword.GetComponentInChildren<Animation> ().PlayQueued("MatchingLoop", QueueMode.CompleteOthers).wrapMode =WrapMode.Loop;
		matchingText.text = "Matching...";
		TweenLogic.TweenMoveTo (matchingText.transform, new Vector2 (matchingText.transform.localPosition.x, matchingText.transform.localPosition.y + 160f), 0.5f);
		TweenLogic.TweenMoveTo (menu.transform, new Vector2 (menu.transform.localPosition.x, menu.transform.localPosition.y - 160f), 0.5f);
		StartMatchingScreen ();
		searchRoomButton.interactable = false;
		SystemFirebaseDBController.Instance.SearchRoom (delegate(bool result) {
			if (result) {
				GoToGameRoom ();	
			} else {
				Debug.Log ("Cancelled Search");
				searchRoomButton.interactable = true;
				matchSword.GetComponentInChildren<Animation> ().Play ("MatchIdle");
//				AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
				matchingText.text = "Find Match";
				TweenLogic.TweenMoveTo (matchingText.transform, new Vector2 (matchingText.transform.localPosition.x, matchingText.transform.localPosition.y - 160f), 0.5f);
				TweenLogic.TweenMoveTo (menu.transform, new Vector2 (menu.transform.localPosition.x, menu.transform.localPosition.y + 160f), 0.5f);
			}
			StopMatchingScreen ();
		});
	}

	public void OnModeChange ()
	{
		foreach (Toggle tg in toggleGroup.ActiveToggles()) {

			ModeEnum modeChosen = ModeEnum.Mode1;
			switch (tg.name) {

			case "Mode1":
				modeChosen = ModeEnum.Mode1;
				break;
			case "Mode2":
				modeChosen = ModeEnum.Mode2;
				break;
			}
			GameData.Instance.modePrototype = modeChosen;
		}
	}

	public void CancelRoomSearch ()
	{
//		FDController.Instance.CancelRoomSearch ();
	}


	private void GoToGameRoom ()
	{
		AudioController.Instance.PlayAudio (AudioEnum.Bgm);
		lobbyRoom.SetActive (false);
		roomViews.SetActive (false);
		gameRoomUI.SetActive (true);
		gameRoomAssets.SetActive (true);
		SystemLoadScreenController.Instance.StopLoadingScreen ();
		StartPreTimer ();
		CameraWorksController.Instance.StartIntroCamera ();
		RPCDicObserver.AddObserver (GestureController.Instance);
		RPCDicObserver.AddObserver (BattleStatusManager.Instance);
		RPCDicObserver.AddObserver(SkillActivator.Instance);
	}

	/// <summary>
	/// Delay before start of battle
	/// </summary>
	public void StartPreTimer ()
	{
		timeLeft = 3;
		stoptimer = true;
		InvokeRepeating ("StartTimer", 0, 1);
	}

	private void StartTimer ()
	{
		if (stoptimer) {
			GameTimerView.Instance.ToggleTimer (true);
			if (timeLeft > 0) {
				GameTimerView.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 
			PhaseManager.Instance.StartPhase1 ();
			GameTimerView.Instance.ToggleTimer (false);
			stoptimer = false;
			CancelInvoke ("StartTimer");
		}
	}

}

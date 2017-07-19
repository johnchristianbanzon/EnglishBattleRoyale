using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMenuController : MonoBehaviour {
	public ScreenLobbyController screenLobby;
	public GameObject menuIndicator;
	public GameObject deckButton;
	public GameObject matchButton;
	public GameObject profileButton;

	public void NavigateProfile(){
		TweenLogic.TweenMoveTo(menuIndicator.transform,profileButton.transform.localPosition,0.4f);
		screenLobby.NavigateProfile ();
	}

	public void NavigateMatch(){
		TweenLogic.TweenMoveTo(menuIndicator.transform,matchButton.transform.localPosition,0.4f);
		screenLobby.NavigateMatch ();
	}

	public void NavigateDeck(){
		TweenLogic.TweenMoveTo(menuIndicator.transform,deckButton.transform.localPosition,0.4f);
		screenLobby.NavigateDeck ();
	}
}

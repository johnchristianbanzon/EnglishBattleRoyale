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
		TweenFacade.TweenMoveTo(menuIndicator.transform,profileButton.transform.localPosition,0.4f);
		screenLobby.NavigateProfile ();
	}

	public void NavigateMatch(){
		TweenFacade.TweenMoveTo(menuIndicator.transform,matchButton.transform.localPosition,0.4f);
		screenLobby.NavigateMatch ();
	}

	public void NavigateDeck(){
		TweenFacade.TweenMoveTo(menuIndicator.transform,deckButton.transform.localPosition,0.4f);
		screenLobby.NavigateDeck ();
	}
}

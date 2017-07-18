using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMenuController : MonoBehaviour {
	public ScreenLobbyController screenLobby;


	public void NavigateProfile(){
		screenLobby.NavigateProfile ();
	}

	public void NavigateMatch(){
		screenLobby.NavigateMatch ();
	}

	public void NavigateDeck(){
		screenLobby.NavigateDeck ();
	}
}

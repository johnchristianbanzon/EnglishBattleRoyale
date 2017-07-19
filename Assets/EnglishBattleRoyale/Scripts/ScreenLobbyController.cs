﻿using UnityEngine;

public class ScreenLobbyController : MonoBehaviour {

	public GameObject partSkill;
	public GameObject partProfile;
	public GameObject partMatch;
	public GameObject menuIndicator;
	public GameObject deckButton;
	public GameObject matchButton;
	public GameObject profileButton;

	public void NavigateProfile(){
		//activate gameObject / Tween here
		TweenLogic.TweenMoveTo(menuIndicator.transform,profileButton.transform.localPosition,0.4f);
		TweenLogic.TweenMoveTo(partMatch.transform,new Vector2(-720f,0),0.4f);
		TweenLogic.TweenMoveTo(partSkill.transform,new Vector2(720f,0),0.4f);
		TweenLogic.TweenMoveTo(partProfile.transform,new Vector2(0,0),0.4f);
	}

	public void NavigateMatch(){
		//activate gameObject / Tween here
		TweenLogic.TweenMoveTo(menuIndicator.transform,matchButton.transform.localPosition,0.4f);
		TweenLogic.TweenMoveTo(partMatch.transform,Vector3.zero,0.4f);
		TweenLogic.TweenMoveTo(partSkill.transform,new Vector2(720f,0),0.4f);
		TweenLogic.TweenMoveTo(partProfile.transform,new Vector2(720f,0),0.4f);
	}

	public void NavigateDeck(){
		//activate gameObject / Tween here
		TweenLogic.TweenMoveTo(menuIndicator.transform,deckButton.transform.localPosition,0.4f);
		TweenLogic.TweenMoveTo(partSkill.transform,Vector3.zero,0.4f);

		TweenLogic.TweenMoveTo(partMatch.transform,new Vector2(-720f,0),0.4f);
		TweenLogic.TweenMoveTo(partProfile.transform,new Vector2(-720f,0),0.4f);
	}

}

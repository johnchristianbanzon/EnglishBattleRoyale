using UnityEngine;

public class ScreenLobbyController : MonoBehaviour {

	public GameObject partSkill;
	public GameObject partProfile;
	public GameObject partMatch;

	void Start(){
		//initialize constants from csv
		SystemLoadScreenController.Instance.StopLoadingScreen();
	}

	public void NavigateProfile(){
		//activate gameObject / Tween here

		TweenFacade.TweenMoveTo(partMatch.transform,new Vector2(-720f,0),0.4f);
		TweenFacade.TweenMoveTo(partSkill.transform,new Vector2(720f,0),0.4f);
		TweenFacade.TweenMoveTo(partProfile.transform,new Vector2(0,0),0.4f);
	}

	public void NavigateMatch(){
		//activate gameObject / Tween here

		TweenFacade.TweenMoveTo(partMatch.transform,Vector3.zero,0.4f);
		TweenFacade.TweenMoveTo(partSkill.transform,new Vector2(720f,0),0.4f);
		TweenFacade.TweenMoveTo(partProfile.transform,new Vector2(720f,0),0.4f);
	}

	public void NavigateDeck(){
		//activate gameObject / Tween here

		TweenFacade.TweenMoveTo(partSkill.transform,Vector3.zero,0.4f);
		TweenFacade.TweenMoveTo(partMatch.transform,new Vector2(-720f,0),0.4f);
		TweenFacade.TweenMoveTo(partProfile.transform,new Vector2(-720f,0),0.4f);
	}

}

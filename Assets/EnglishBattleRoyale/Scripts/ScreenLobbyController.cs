using UnityEngine;
using UnityEngine.UI;

public class ScreenLobbyController : SingletonMonoBehaviour<ScreenLobbyController> {
	public GameObject partSkill;
	public GameObject homeGroup;
	public GameObject partMatch;
	public GameObject menu;
	public GameObject partProfile;
	public GameObject skillButton;
	public GameObject matchButton;
	public GameObject homeButton;
	public GameObject lobbyView;
	public GameObject menuIndicator;
	public InputField playerName;

	public string GetPlayerName(){
		return playerName.text;
	}


	public void LerpProfileView(Button start){
		TweenLogic.TweenMoveTo(homeGroup.transform, -partProfile.transform.localPosition, 0.3f);
		menu.SetActive (true);
		lobbyView.SetActive (false);
	}

	public void LerpSkillView(Button skill){
		TweenLogic.TweenMoveTo (homeGroup.transform, -partSkill.transform.localPosition, 0.5f);
		TweenLogic.TweenMoveTo (menuIndicator.transform, skillButton.transform.localPosition, 0.3f);
		colorToDefault ();
		TweenLogic.TweenScaleToLarge(skill.transform.GetChild(0).transform, new Vector3(1.1f,1.1f,1.1f), 0.2f);
	}

	public void LerpBackToHome(Button home){
		TweenLogic.TweenMoveTo(homeGroup.transform, new Vector2(0,0), 0.5f);
		TweenLogic.TweenMoveTo (menuIndicator.transform, homeButton.transform.localPosition, 0.3f);
		colorToDefault ();
		TweenLogic.TweenScaleToLarge(home.transform.GetChild(0).transform, new Vector3(1.1f,1.1f,1.1f), 0.2f);
	}

	public void LerpToMatch(Button match){
		TweenLogic.TweenMoveTo(homeGroup.transform, -partMatch.transform.localPosition, 0.5f);
		TweenLogic.TweenMoveTo (menuIndicator.transform, matchButton.transform.localPosition, 0.3f);
		colorToDefault ();
		TweenLogic.TweenScaleToLarge(match.transform.GetChild(0).transform, new Vector3(1.1f,1.1f,1.1f), 0.2f);
	}

	public void colorToDefault(){
		TweenLogic.TweenTextScale(skillButton.transform.GetChild(0).transform, Vector3.one, 0.2f);
		TweenLogic.TweenTextScale(matchButton.transform.GetChild(0).transform, Vector3.one, 0.2f);
		TweenLogic.TweenTextScale(homeButton.transform.GetChild(0).transform, Vector3.one, 0.2f);
	}
}

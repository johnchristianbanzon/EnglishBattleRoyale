using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBattleController: SingletonMonoBehaviour<ScreenBattleController>
{
	public PartStateController partState;
	public PartCharacterController partCharacter;
	public PartQuestionController partQuestion;
	public PartGestureController partGesture;
	public PartCameraWorksController partCameraWorks;
	public PartAvatarsController partAvatars;
	private bool isPhase1 = false;

	void Start(){
		Init ();
	}

	private void Init(){
		TimeManager.AddGameTimeObserver (partState);
		SystemSoundController.Instance.PlayBGM ("BGM_Battle");

		SystemLoadScreenController.Instance.StopLoadingScreen ();
		partCameraWorks.StartIntroCamera ();
		TimeManager.StartPreBattleTimer (3);

		//get initial state for enemy and player stored in GameManager
		foreach (KeyValuePair<Firebase.Database.DataSnapshot, bool> initialState in GameManager.initialState) {
			partState.SetStateParam (initialState.Key, initialState.Value);
		}
	}

	public void StartPhase1 ()
	{
		PhaseActivate (true, false);
		isPhase1 = true;
	}

	public void StartPhase2 ()
	{
		PhaseActivate (false, true);
		isPhase1 = false;
	}

	public bool GetIsPhase1(){
		return isPhase1;
	}

	private void PhaseActivate (bool answer, bool attack)
	{
		if (answer) {
			partQuestion.OnStartPhase ();
		} else {
			partQuestion.OnEndPhase ();
		}

		if (attack) {
			partCharacter.OnStartPhase ();
		} else {
			partCharacter.OnEndPhase ();
		}
	}
}

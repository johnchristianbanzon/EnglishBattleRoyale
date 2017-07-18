using UnityEngine;
using UnityEngine.UI;

/* UI For searching matches */
public class PartMatchController : MonoBehaviour
{
	public Button searchRoomButton;
	public Text searchRoomButtonText;
	public Text searchRoomText;
	private bool isSearchingRoom = false;

	public void SearchRoom ()
	{
		if (isSearchingRoom == false) {
			searchRoomText.text = "Searching Opponent";
			searchRoomButtonText.text = "Cancel";
			AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
			SystemFirebaseDBController.Instance.SearchRoom (delegate(bool result) {
				if (result) {
					GoToGameRoom ();	
				} else {
					searchRoomButtonText.text = "Battle";
					searchRoomText.text = "Find Match";
					Debug.Log ("Cancelled Search");
					searchRoomButton.interactable = true;
					AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
				}
			});
		}
	}



	public void CancelRoomSearch ()
	{
		if (isSearchingRoom) {
			searchRoomButton.interactable = false;
			SystemFirebaseDBController.Instance.CancelRoomSearch ();
		}
	}


	private void GoToGameRoom ()
	{
		RPCDicObserver.AddObserver (GestureController.Instance);
		RPCDicObserver.AddObserver (BattleStatusManager.Instance);
		RPCDicObserver.AddObserver (SkillActivator.Instance);
		SystemScreenController.Instance.ShowScreen ("ScreenBattle");
	}



}

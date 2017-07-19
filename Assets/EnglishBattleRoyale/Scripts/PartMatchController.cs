using UnityEngine;
using UnityEngine.UI;

/* UI For searching matches */
public class PartMatchController : MonoBehaviour
{
	public Button searchRoomButton;
	public Image searchRoomImage;
	public Sprite searchRoomBattle;
	public Sprite searchRoomCancel;
	public Text searchRoomText;
	private bool isSearchingRoom = false;

	public void SearchRoom ()
	{
		if (isSearchingRoom == false) {
			searchRoomText.text = "Searching Opponent";
			searchRoomImage.sprite = searchRoomCancel;
			AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
			SystemFirebaseDBController.Instance.SearchRoom (delegate(bool result) {
				if (result) {
					GoToGameRoom ();	
				} else {
					searchRoomImage.sprite = searchRoomBattle;
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
		SystemScreenController.Instance.ShowScreen ("ScreenBattle");
	}



}

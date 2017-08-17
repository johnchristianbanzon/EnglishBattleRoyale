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
			isSearchingRoom = true;
			searchRoomText.text = "Searching Opponent";
			searchRoomImage.sprite = searchRoomCancel;
			SystemSoundController.Instance.PlaySFX ("SFX_ClickButton");
			SystemFirebaseDBController.Instance.SearchRoom (delegate(bool result) {
				if (result) {
					GoToGameRoom ();	
				} else {
					searchRoomImage.sprite = searchRoomBattle;
					searchRoomText.text = "Find Match";
					Debug.Log ("Cancelled Search");
					searchRoomButton.interactable = true;
					SystemSoundController.Instance.PlaySFX ("SFX_ClickButton");
				}
			});

		} else {
			CancelRoomSearch ();
		}
	}



	private void CancelRoomSearch ()
	{
		if (isSearchingRoom) {
			searchRoomButton.interactable = false;
			SystemFirebaseDBController.Instance.CancelRoomSearch ();
			isSearchingRoom = false;
		}
	}

	//initialize and go to battle
	private void GoToGameRoom ()
	{
		SystemLoadScreenController.Instance.StartLoadingScreen (delegate() {
			BattleStatusManager battleStatusManager = new BattleStatusManager ();
			BattleManager battleManager = new BattleManager ();
			CharacterManager characterManager = new CharacterManager ();

			battleManager.Init ();
			characterManager.Init ();
			battleStatusManager.Init ();

			SystemScreenController.Instance.ShowScreen ("ScreenBattle");
		});
	}



}

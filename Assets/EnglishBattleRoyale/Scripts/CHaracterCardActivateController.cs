using UnityEngine;
using UnityEngine.UI;

public class CHaracterCardActivateController : MonoBehaviour {

	public void ShowCard (int charID)
	{
		this.GetComponent<Image>().sprite = SystemResourceController.Instance.LoadCharacterCardSprite (charID);
		Destroy (this.gameObject, 1);
	}

}

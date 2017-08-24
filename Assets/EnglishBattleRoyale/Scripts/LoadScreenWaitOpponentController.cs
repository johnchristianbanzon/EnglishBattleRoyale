using UnityEngine;
using UnityEngine.UI;

public class LoadScreenWaitOpponentController : MonoBehaviour {
	public Text waitOpponentText;

	// Use this for initialization
	void Start () {
		TweenFacade.TweenWaitOpponentText(waitOpponentText.GetComponent<RectTransform>());
	}
	

}

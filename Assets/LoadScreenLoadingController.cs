using UnityEngine;
using UnityEngine.UI;

public class LoadScreenLoadingController : MonoBehaviour {

	public Text loadingText;

	// Use this for initialization
	void Start () {
		TweenFacade.TweenWaitOpponentText(loadingText.GetComponent<RectTransform>());
	}
}

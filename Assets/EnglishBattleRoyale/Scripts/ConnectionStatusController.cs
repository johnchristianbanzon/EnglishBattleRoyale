using UnityEngine;
using UnityEngine.UI;

public class ConnectionStatusController : MonoBehaviour, IRPCBoolObserver {

	public Image connectionIndicator;

	void Start(){
		RPCBoolObserver.AddObserver (this);
		connectionIndicator.enabled = false;
	}

	public void OnNotify (bool isConnectedDB)
	{
		if (!isConnectedDB) {
			connectionIndicator.enabled = true;
			TweenFacade.TweenImageFadeInFadeOut (connectionIndicator);
		} else {
			connectionIndicator.enabled = false;
		}

	}

}

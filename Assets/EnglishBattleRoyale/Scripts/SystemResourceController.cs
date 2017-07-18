using UnityEngine;

public class SystemResourceController : SingletonMonoBehaviour<SystemResourceController>
{
	public GameObject LoadPrefab (string prefabName, GameObject parentObject)
	{
		GameObject prefabObject = Instantiate (Resources.Load ("Prefabs/" + prefabName)) as GameObject;
		prefabObject.transform.SetParent (parentObject.transform, false);
		return prefabObject;
	}

	public TextAsset LoadCSV (string csvName){
		TextAsset csvObject = Resources.Load ("CSV/" + csvName) as TextAsset;
		return csvObject;
	}




}

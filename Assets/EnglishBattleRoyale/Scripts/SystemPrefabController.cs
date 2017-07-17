using UnityEngine;

public class SystemPrefabController : SingletonMonoBehaviour<SystemPrefabController>
{
	public GameObject LoadPrefab (string prefabName, GameObject parentObject)
	{
		GameObject prefabObject = Instantiate (Resources.Load ("Prefabs/" + prefabName)) as GameObject;
		prefabObject.transform.SetParent (parentObject.transform, false);
		return prefabObject;
	}
}

using System.Collections.Generic;
using UnityEngine;
public static class JsonConverter{

	public static string DicToJsonStr (Dictionary<string, System.Object> param)
	{
		string jsonStr = MiniJSON.Json.Serialize (param);
		return jsonStr;
	}

	public static Dictionary<string, System.Object> JsonStrToDic (string param)
	{
		Dictionary<string, System.Object> Dic = (Dictionary<string, System.Object>)MiniJSON.Json.Deserialize (param);
		return Dic;

	}

	public static Dictionary<string, System.Object> ObjToDic (string objectName, System.Object myObject)
	{
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object> ();
		result [objectName] = JsonUtility.ToJson (myObject);

		return result;
	}

	public static System.Object StringToObject (string jsonString)
	{
		System.Object result = JsonUtility.FromJson<System.Object> (jsonString);
		return result;
	}


}

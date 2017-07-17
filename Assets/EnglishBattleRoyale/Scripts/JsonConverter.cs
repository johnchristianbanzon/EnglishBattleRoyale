using System.Collections.Generic;
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
}

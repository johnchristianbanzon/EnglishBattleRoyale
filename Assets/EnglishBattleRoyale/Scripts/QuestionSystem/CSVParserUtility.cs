using System.Collections.Generic;
using UnityEngine;
using PapaParse.Net;

public static class CSVParserUtility
{
	public static List<List<string>> Parse (string csvName)
	{
		Result parsed = Papa.parse (csvName);
		List<List<string>> rows = parsed.data;
		return rows;
	}

	public static object GetValueFromKey (List<List<string>> list, string constName)
	{
		object questionConstValue = null;
		for (int i = 1; i < list.Count; i++) {
			if (list [i] [0].Equals (constName)) {
				questionConstValue = list [i] [1];
				break;
			}
		}
		return questionConstValue;
	}

	public static object[] GetValueArrayFromKey (List<List<string>> list, string constName)
	{
		object[] questionConstValue = new object[list.Count];
		int index = 0;
		for (int i = 0; i < list.Count; i++) {
			for (int j = 0; j < list [i].Count; j++) {
				if (list [0] [j].Equals (constName)) {
					index = j;
					questionConstValue [i] = list [i] [index];
				}
			}
		}
		return questionConstValue;
	}



}
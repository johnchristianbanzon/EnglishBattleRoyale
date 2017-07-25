using System.Collections.Generic;
using UnityEngine;
using PapaParse.Net;

public static class CSVParser
{
	public static List<List<string>> Parse(string csvName){
		Result parsed = Papa.parse (csvName);
		List<List<string>> rows = parsed.data;
		return rows;
	} 

}
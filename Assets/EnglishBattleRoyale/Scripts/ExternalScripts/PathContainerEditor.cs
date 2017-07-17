#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PathContainer))]
	public class PathContainerEditor : Editor
	{
	
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			PathContainer myScript = (PathContainer)target;
			if(GUILayout.Button("Add"))
			{
				myScript.AddNewWaypoint();
			}
		}
	}
#endif


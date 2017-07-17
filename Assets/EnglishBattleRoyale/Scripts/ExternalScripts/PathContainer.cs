using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using DG.Tweening;

[Serializable]
public class PathContainer : MonoBehaviour {
public List<GameObject> waypoints;
public Vector3[] waypointVector;
public float duration;
public GameObject waypointPrefab;
public Transform moveObject;
public float moveDelay;
public bool isLoop = false;
	public bool isLookRotator = false;
	public bool startCameraInWaypoint = false;
public SmoothLookAt smoothCamera;
public GameObject cameraRotator;

	#if UNITY_EDITOR
	void OnDrawGizmos(){
		

		if (waypoints == null) {
			return;
		}
		if (waypoints.Count != 0) {
			for (int i = 1; i < waypoints.Count; i++) {
				Gizmos.color = Color.green;
				Gizmos.DrawLine (waypoints [i].transform.position, waypoints [i - 1].transform.position);

			}
		}
		waypointVector = new Vector3[waypoints.Count];
		for (int i = 0; i < waypoints.Count; i++) {
			waypointVector [i] = waypoints [i].transform.position;
		}

	}

	public void AddNewWaypoint(){
		GameObject newWaypoint = Instantiate (waypointPrefab, this.transform);
		waypoints.Add (newWaypoint);
		Selection.activeGameObject = newWaypoint;

	}
	#endif
	void OnEnable(){
		if (isLookRotator) {
			smoothCamera.target = cameraRotator.transform;
		}
		if (startCameraInWaypoint) {
			moveObject.transform.position = waypoints [0].transform.position;
		}
		StartCoroutine (StartDelay());
	}

	IEnumerator StartDelay(){
		yield return new WaitForSeconds (moveDelay);
		Sequence mySequence = DOTween.Sequence ();
		mySequence.Append (moveObject.DOPath (waypointVector, duration, PathType.CatmullRom,PathMode.Full3D,10).SetEase(Ease.Linear).SetSpeedBased(true));
		if (isLoop) {
			mySequence.SetLoops (-1);
		}
	
	}

}


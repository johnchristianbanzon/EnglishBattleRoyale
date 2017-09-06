using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillEffectController : MonoBehaviour {
	public float skillDuration = 2;
	public string skillAudioName = "";
	public float audioTimeDelay = 0;


	void OnEnable(){
		if (!String.IsNullOrEmpty (skillAudioName)) {
			StartCoroutine (SkillAudioDelay ());
		}
		StartCoroutine (SkillDuration());
	}

	public float GetSkillDuration(){
		return skillDuration;
	}

	IEnumerator SkillAudioDelay(){
		yield return new WaitForSeconds (audioTimeDelay);
		SystemSoundController.Instance.PlaySFX (skillAudioName);

	}

	IEnumerator SkillDuration(){
		yield return new WaitForSeconds (skillDuration);
		Destroy (this.gameObject);
	}
}

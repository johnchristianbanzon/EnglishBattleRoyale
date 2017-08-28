using UnityEngine;
using System.Collections;
using System;

public class SystemSoundController: SingletonMonoBehaviour<SystemSoundController>
{
	private AudioSource bgmPlayer;
	private bool isBGMMute;
	private bool isSFXMute;

	public void MuteBGMToggle (bool toggle)
	{
		isBGMMute = toggle;
	}

	public void MuteSFXToggle (bool toggle)
	{
		isSFXMute = toggle;
	}

	public void PlayBGM (string bgmName)
	{
		if (bgmPlayer != null) {
			Destroy (bgmPlayer);
		}
		bgmPlayer = this.gameObject.AddComponent<AudioSource> ();	
		bgmPlayer.clip = SystemResourceController.Instance.LoadAudio (bgmName);
		bgmPlayer.loop = true;
		bgmPlayer.mute = isBGMMute;

		TweenFacade.TweenVolume (bgmPlayer);
		bgmPlayer.Play ();
	}

	public void PlaySFX (string sfxName)
	{

		AudioSource sfxPlayer = this.gameObject.AddComponent<AudioSource> ();	
		sfxPlayer.clip = SystemResourceController.Instance.LoadAudio (sfxName);
		sfxPlayer.mute = isSFXMute;

		sfxPlayer.Play ();

		StartCoroutine (StartDestroyAfterPlay (sfxPlayer.clip.length, delegate() {
			Destroy (sfxPlayer);
		}));
	}

	private IEnumerator StartDestroyAfterPlay (float time, Action action)
	{
		yield return new WaitForSeconds (time);
		action ();
	}

}

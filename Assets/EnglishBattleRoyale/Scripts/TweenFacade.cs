using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class TweenFacade{

	//HP GP Sliders
	public static void TweenPlayerGPSlider(float endValue, float duration, bool snapping, Slider playerGpSlider){
		TweenSlider (playerGpSlider, endValue, duration, snapping);
	}

	public static void TweenPlayerHPSlider(float endValue, float duration, bool snapping, Slider playerHpSlider){
		TweenSlider (playerHpSlider, endValue, duration, snapping);
	}

	public static void TweenEnemyHPSlider(float endValue, float duration, bool snapping, Slider enemyHpSlider){
		TweenSlider (enemyHpSlider, endValue, duration, snapping);
	}

	private static void TweenSlider(Slider slider,float endValue, float duration, bool snapping){
		slider.DOValue (endValue, duration, snapping).SetEase(Ease.InOutCirc);
	}
		

	//WaitOpponent

	public static void TweenScaleYToCustom(float duration, GameObject scaleObject, float customScale){
		scaleObject.transform.localScale = new Vector3 (customScale, 0, customScale);
		scaleObject.transform.DOScale(Vector3.one, duration);
	}

	public static void TweenScaleYT0Zero(float duration, GameObject scaleObject, float customScale){
		scaleObject.transform.DOScale(new Vector3(customScale,0,customScale), duration);
	}

	public static void TweenWaitOpponentText(RectTransform obj){
		Sequence mySequence = DOTween.Sequence ();
		mySequence.SetLoops (-1);
		mySequence.Append(obj.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.5f).SetEase(Ease.Linear));
		mySequence.Append(obj.DOScale(Vector3.one,0.5f).SetEase(Ease.Linear));
	}

	public static void TweenVolume(AudioSource obj){
		obj.volume = 0;
		obj.DOFade (1,10);
	}
		

	public static void TweenTextScale(Transform text, Vector3 endValue, float duration){
		text.transform.DOScale (endValue,duration).SetEase(Ease.InElastic,30,1);
	}

	public static void TweenScaleToSmall(Transform obj,Vector3 endValue, float duration){
		obj.transform.DOScale (endValue,0.05f);
	}
	public static void TweenScaleToLarge(Transform text, Vector3 endValue, float duration){
		text.transform.DOScale (new Vector3(0.1f,0.1f,0.1f),0.05f);
		text.transform.DOScale (endValue, duration).SetEase (Ease.OutElastic, 10, 1);
	}

	public static void TweenShakePosition(Transform obj,float duration, float strength, int vibrato, float randomness){
		obj.DOShakePosition(duration, strength, vibrato,randomness, true);
	}

	public static void TweenDoShakeRotation(Transform obj,float duration, Vector3 strength, int vibrato, float randomness){
		obj.DOShakeRotation(duration, strength, vibrato,randomness, true);
	}

	public static void TweenDoPunchRotation(Transform obj,float duration, Vector3 strength, int vibrato, float elasticity){
		obj.DOPunchRotation(strength, duration,vibrato, elasticity).SetLoops(-1).SetEase(Ease.Linear);
	}
	public static void StopTweens(){
		DOTween.Clear ();
	}
	public static void TweenJumpTo(Transform obj, Vector3 endValue, float jumpPower,int numJumps,float duration,float delay){
		obj.transform.DOLocalJump (endValue, jumpPower, numJumps, 1, true).SetDelay (delay);
	}

	public static void TweenMoveTo(Transform obj, Vector3 endValue,float duration){
		obj.transform.DOLocalMove (endValue,duration,true);
	}

	public static void TweenImageFadeInFadeOut(Image image){
		Sequence mySequence = DOTween.Sequence();
		mySequence.SetLoops (-1);
		mySequence.Append (image.DOFade(0,1));
		mySequence.Append (image.DOFade(1,1));
	}

	public static void SliderTimer(Slider slider, float value){
		slider.DOValue (value, 1).SetEase (Ease.Linear);
	}


	public static void TweenNewCharacterCard(Transform obj){
		obj.DOScale (Vector3.one, 1).SetEase (Ease.InOutElastic);
	}

	public static void TweenActivateCharacterCard(Transform obj){
		obj.DOScale (Vector3.zero, 1).SetEase (Ease.InOutElastic);
	}

	public static void RotateObject(GameObject obj, Vector3 rotationValue, float duration){
		obj.transform.DORotate (rotationValue,duration);
	}
}

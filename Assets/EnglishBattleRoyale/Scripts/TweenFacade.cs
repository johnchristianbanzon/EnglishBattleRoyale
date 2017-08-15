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

	public static void TweenScaleToNormal(float duration, GameObject scaleObject,GameObject loadingIndicator = null){
		scaleObject.transform.DOScale(new Vector3(1,1,1), duration);
		if (loadingIndicator != null) {
			TweenRotateForever (loadingIndicator.GetComponent<RectTransform>());
		}
	}

	public static void TweenScaleToZero(float duration, GameObject scaleObject){
//		scaleObject.transform.DOScale(new Vector3(1,0,0), duration);
	}

	public static void TweenRotateForever(RectTransform rotateObject){
		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(rotateObject.DOLocalRotate(new Vector3(0, 0, -360), 5, RotateMode.FastBeyond360).SetEase(Ease.Linear)).SetLoops(-1);
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
	public static void TweenJumpTo(Transform obj, Vector3 endValue, float jumpPower,int numJumps,float duration){
		obj.transform.DOLocalJump (endValue,jumpPower,numJumps,1,true);
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

		float originalX = obj.position.x;
		obj.transform.localScale = Vector3.one;
		obj.transform.position = new Vector3 (obj.position.x - 20,obj.position.y);
		obj.DOMoveX (originalX, 1).SetEase(Ease.OutCirc);
	}

	public static void TweenActivateCharacterCard(Transform obj){
		obj.DOScale (Vector3.zero, 1).SetEase (Ease.InOutElastic);
	}


}

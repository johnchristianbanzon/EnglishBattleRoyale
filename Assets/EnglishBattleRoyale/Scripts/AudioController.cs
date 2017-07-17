using UnityEngine;
public class AudioController: SingletonMonoBehaviour<AudioController>
{
	public AudioSource bgmBattle;
	public AudioSource clickButton;
	public AudioSource attack;
	public AudioSource lose;
	public AudioSource skill;
	public AudioSource win;
	public AudioSource hit;
	public AudioSource mistake;
	public AudioSource correct;

	//create and destroy sfx after play to avoid conflict
	public void PlayAudio (AudioEnum audioName)
	{
		switch (audioName) {
		case AudioEnum.Bgm:
			bgmBattle.Play ();
			break;
		case AudioEnum.ClickButton:
			clickButton.Play ();
			break;
		case AudioEnum.Attack:
			attack.Play ();
			break;
		case AudioEnum.Lose:
			lose.Play ();
			break;
		case AudioEnum.Skill:
			skill.Play ();
			break;
		case AudioEnum.Win:
			win.Play ();
			break;
		case AudioEnum.Hit:
			hit.Play ();
			break;
		case AudioEnum.Correct:
			correct.Play ();
			break;
		case AudioEnum.Mistake:
			mistake.Play ();
			break;
		}

	}
}

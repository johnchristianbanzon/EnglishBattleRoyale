using UnityEngine;
using UnityEngine.UI;

public class CharCardController : MonoBehaviour
{
	public Text gpCost;
	public Image skillImage;

	public void SetCardParameter (SkillModel cardParam)
	{
		this.gpCost.text = cardParam.skillGpCost.ToString();
		skillImage.sprite = SystemResourceController.Instance.LoadCharacterCardSprite (cardParam.skillName);
	}
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SkillViewController : MonoBehaviour {
	public GameObject skillPlaceHolder;
	public GameObject skillOverViewScreen;
	public GameObject skillOverViewDesc;
	public GameObject skillDamage;
	public GameObject skillHeal;
	public GameObject skillName;
	public GameObject skillGp;
	public GameObject skillSprite;
	private GameObject previousSelectedButton;
	private GameObject currentSelectedButton;
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickSkillItem(){	
		
		if (previousSelectedButton != null) {
			Destroy (GameObject.Find("SkillNav"));
		}
		currentSelectedButton = EventSystem.current.currentSelectedGameObject;
		previousSelectedButton = previousSelectedButton == null ? EventSystem.current.currentSelectedGameObject:
			currentSelectedButton ;
		GameObject answerPrefab = Instantiate (Resources.Load("Prefabs/SkillNav")) as GameObject; 
		answerPrefab.transform.SetParent (EventSystem.current.currentSelectedGameObject.transform, false);
		TweenLogic.TweenScaleToSmall (previousSelectedButton.transform,
			new Vector3(1.0f,1.0f,1.0f),0.5f);
		TweenLogic.TweenScaleToLarge (EventSystem.current.currentSelectedGameObject.transform,
			new Vector3(1.10f,1.10f,1.10f),0.5f);
		answerPrefab.name = "SkillNav";
		answerPrefab.transform.localPosition = new Vector3(53,
			EventSystem.current.currentSelectedGameObject.transform.localPosition.y,0);
		answerPrefab.transform.SetAsFirstSibling ();
		answerPrefab.transform.GetChild(1).GetComponent<Button> ().onClick.AddListener (() => {
			OnClickInfo ();
		});
		answerPrefab.transform.GetChild(2).GetComponent<Button> ().onClick.AddListener (() => {
			OnClickUse ();
		});
		//TweenController.TweenMoveTo (answerPrefab.transform,),0.5f);
		//_scrollRect.content.GetChild(0).transform.SetAsLastSibling();
	}

	public void OnClickUse(){
	//	Debug.Log (GameObject.Find ("EquippedSkill1").transform.localPosition);
		/*
		currentSelectedButton.transform.SetParent( GameObject.Find("EquippedSkill1").transform);
		TweenController.TweenMoveTo (currentSelectedButton.transform, GameObject.Find("EquippedSkill1").transform.parent.localPosition,0.5f);*/
	}

	public void OnClickInfo(){

		skillOverViewScreen.SetActive (true);
		TweenLogic.TweenScaleToLarge (skillOverViewScreen.transform,new Vector3(1,1,1),0.2f);
		skillSprite.GetComponentInChildren<Image>().sprite = currentSelectedButton.transform.GetChild(2).GetComponentInChildren<Image> ().sprite;
		switch (currentSelectedButton.name) {
		case "BicPunch":
			skillOverViewDesc.GetComponentInChildren<Text> ().text = SkillManager.skillList[0].skillDescription;
			skillDamage.GetComponentInChildren<Text> ().text = "9";
			skillHeal.GetComponentInChildren<Text> ().text = "0";
			skillGp.GetComponentInChildren<Text> ().text = "3";
			skillName.GetComponentInChildren<Text> ().text = SkillManager.skillList[0].skillName.ToString();
			break;
		case "Sunder":
			skillOverViewDesc.GetComponentInChildren<Text> ().text = SkillManager.skillList[1].skillDescription;
			skillDamage.GetComponentInChildren<Text> ().text = "10";
			skillHeal.GetComponentInChildren<Text> ().text = "5";
			skillGp.GetComponentInChildren<Text> ().text = "9";
			skillName.GetComponentInChildren<Text> ().text = SkillManager.skillList[1].skillName.ToString();
			break;
		case "Rejuvenation":
			skillOverViewDesc.GetComponentInChildren<Text> ().text = SkillManager.skillList[2].skillDescription;
			skillDamage.GetComponentInChildren<Text> ().text = "0";
			skillHeal.GetComponentInChildren<Text> ().text = "8";
			skillGp.GetComponentInChildren<Text> ().text = "4";
			skillName.GetComponentInChildren<Text> ().text = SkillManager.skillList[2].skillName.ToString();
			break;
		}
		Destroy (EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
	}
	public void OnCloseSkillOverView(){
		TweenLogic.TweenTextScale (skillOverViewScreen.transform,new Vector3(1,1,1),0.5f);
		skillOverViewScreen.SetActive (false);
	}


}

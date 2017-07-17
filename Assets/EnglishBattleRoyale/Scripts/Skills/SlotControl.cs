using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SlotControl : MonoBehaviour , IDropHandler{
	public int position;
	private SkillModel[] skill = new SkillModel[3];//VARIABLE NEVER USED
	void start(){
		
	}
	public GameObject item
	{
		get
		{
			if (transform.childCount > 0)
			{
				return transform.GetChild(0).gameObject;
			}
			return null;
		}
	}

	#region IdropHandler implementation
	public void OnDrop(PointerEventData eventData)
	{
		if (!item)
		{
			DragHandler dh = new DragHandler ();//VARIABLE NEVER USED
			DragHandler.item.transform.SetParent(transform);
			int skillIndex = 0;
			switch(item.name){
			case "BicPunch":
				skillIndex= 0;
				break;
			case "Sunder":
				 skillIndex = 1;
				break;
			case "Rejuvenation":
				skillIndex = 2;
				break;
			}
			SkillManager.Instance.SetSkill (position, 
				SkillManager.Instance.skillList[skillIndex]);
		}
	}
	#endregion

}﻿
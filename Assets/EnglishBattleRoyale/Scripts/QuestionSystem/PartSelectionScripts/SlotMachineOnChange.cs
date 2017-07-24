using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotMachineOnChange : MonoBehaviour {

	private ScrollRect myScrollRect;
	private RectTransform slotContent;
	private float positionCounter = 0.5f;
	private GameObject itemGot;
	public bool InitByUser = false; 
	private ScrollRect _scrollRect;
	private ContentSizeFitter _contentSizeFitter; 
	private VerticalLayoutGroup _verticalLayoutGroup;
	private HorizontalLayoutGroup _horizontalLayoutGroup;
	private GridLayoutGroup _gridLayoutGroup;
	private bool _isVertical = false; 
	private bool _isHorizontal = false; 
	private float _disableMarginY = 0;
	private bool _hasDisabledGridComponents = false; 
	private List <RectTransform> items = new List<RectTransform>();

	//private bool stopScrolling = false;

	public void getContentPosition(){
		myScrollRect = GetComponent<ScrollRect>();
		slotContent = myScrollRect.content;
		positionCounter = 1.0f;

	
	}
	void Start(){
		getContentPosition ();
		GetSlots ();

	}

	public GameObject GetSlots(){
		GameObject selectedObject = null;
		switch(myScrollRect.transform.GetChild (0).GetChild (0).GetChild (0).gameObject.name){
		case "SlotText1":
			selectedObject = myScrollRect.transform.GetChild (0).GetChild (0).GetChild (1).gameObject;
			break;
		case "SlotText2":
			selectedObject = myScrollRect.transform.GetChild (0).GetChild (0).GetChild (2).gameObject;
			break;
		case "SlotText3":
			selectedObject = myScrollRect.transform.GetChild (0).GetChild (0).GetChild (0).gameObject;
			break;
		}
		itemGot = selectedObject;
		return selectedObject;
	}

	public void GetAnswer(GameObject g){
		itemGot = g;
		Debug.Log (g.transform.GetChild (0).GetComponent<Text> ().text);

	}
	public void OnButtonDown(){
		getContentPosition ();

		switch (EventSystem.current.currentSelectedGameObject.name) {
		case "UpIcon":	
			myScrollRect.content.GetChild (_itemCount - 1).transform.SetAsFirstSibling ();
			break;
		case "DownIcon":
			_scrollRect.content.GetChild(0).transform.SetAsLastSibling();
			break;
		}
		GetAnswer (myScrollRect.transform.GetChild (0).GetChild (0).GetChild (1).gameObject);

	}
		
	void Awake ()
	{
		if(!InitByUser)
			Init();
	}
	public void Init()
	{
		if(GetComponent<ScrollRect>() != null)
		{
			_scrollRect = GetComponent<ScrollRect>();
			_scrollRect.onValueChanged.AddListener(OnScroll);
			_scrollRect.movementType = ScrollRect.MovementType.Unrestricted;

			for(int i=0;i<_scrollRect.content.childCount;i++)
			{
				items.Add(_scrollRect.content.GetChild(i).GetComponent<RectTransform>());
			}
			if(_scrollRect.content.GetComponent<VerticalLayoutGroup>() != null)
			{
				_verticalLayoutGroup = _scrollRect.content.GetComponent<VerticalLayoutGroup>();
			}
			if(_scrollRect.content.GetComponent<HorizontalLayoutGroup>() != null)
			{
				_horizontalLayoutGroup = _scrollRect.content.GetComponent<HorizontalLayoutGroup>();
			}
			if(_scrollRect.content.GetComponent<GridLayoutGroup>() != null)
			{
				_gridLayoutGroup = _scrollRect.content.GetComponent<GridLayoutGroup>();
			}
			if(_scrollRect.content.GetComponent<ContentSizeFitter>() != null)
			{
				_contentSizeFitter = _scrollRect.content.GetComponent<ContentSizeFitter>();
			}

			_isHorizontal = _scrollRect.horizontal;
			_isVertical = _scrollRect.vertical;

			if(_isHorizontal && _isVertical)
			{
				Debug.LogError("UI_InfiniteScroll doesn't support scrolling in both directions, plase choose one direction (horizontal or vertical)");
			}

			_itemCount = _scrollRect.content.childCount;
		}
		else
		{
			Debug.LogError("UI_InfiniteScroll => No ScrollRect component found");
		}
	}

	void DisableGridComponents()
	{
		if(_isVertical)
		{
			_recordOffsetY = items[0].GetComponent<RectTransform>().anchoredPosition.y - items[1].GetComponent<RectTransform>().anchoredPosition.y;
			_disableMarginY = _recordOffsetY * _itemCount /2;// _scrollRect.GetComponent<RectTransform>().rect.height/2 + items[0].sizeDelta.y;
		}
		if(_verticalLayoutGroup)
		{
			_verticalLayoutGroup.enabled = false; 
		}
		if(_horizontalLayoutGroup)
		{
			_horizontalLayoutGroup.enabled = false; 
		}
		if(_contentSizeFitter)
		{
			_contentSizeFitter.enabled = false; 
		}
		if(_gridLayoutGroup)
		{
			_gridLayoutGroup.enabled = false; 
		}
		_hasDisabledGridComponents = true; 
	}
	private Vector2 _newAnchoredPosition = Vector2.zero;

	private float _treshold = 100f; 
	private int _itemCount = 0;
	private float _recordOffsetY = 0;

	public void OnScroll(Vector2 pos)
	{
		
		positionCounter = Mathf.Round (slotContent.transform.localPosition.y * 100f) / 100f;
		Canvas.ForceUpdateCanvases();
		string distanceDiff = (positionCounter + Mathf.Round (itemGot.transform.localPosition.y * 100f) / 100f).ToString("f0");
		if (distanceDiff=="-180" || distanceDiff=="-40" || distanceDiff=="-41" || distanceDiff=="-39" || distanceDiff=="-42"
			|| distanceDiff=="-43" || distanceDiff=="-38" || distanceDiff=="-37" || distanceDiff=="-181" || distanceDiff=="-182"
			|| distanceDiff=="-179" || distanceDiff=="-178"
		
		) {
			myScrollRect.enabled = false;
			myScrollRect.enabled = true;
		}

		if (pos.y <= 0f) {
			myScrollRect.enabled = false;
			myScrollRect.enabled = true;
		}

		if(!_hasDisabledGridComponents)
			DisableGridComponents();
		for(int i=0;i<items.Count;i++)
		{
			if(_isVertical)
			{
				if (_scrollRect.transform.InverseTransformPoint(items[i].gameObject.transform.position).y > _disableMarginY + _treshold)
				{
					_newAnchoredPosition = items[i].anchoredPosition;
					_newAnchoredPosition.y -= _itemCount * _recordOffsetY;
					items[i].anchoredPosition = _newAnchoredPosition;
					_scrollRect.content.GetChild(_itemCount-1).transform.SetAsFirstSibling();
				}
				else if (_scrollRect.transform.InverseTransformPoint(items[i].gameObject.transform.position).y <= -_disableMarginY)
				{
					_newAnchoredPosition = items[i].anchoredPosition;
					_newAnchoredPosition.y += _itemCount * _recordOffsetY;
					items[i].anchoredPosition = _newAnchoredPosition;
					_scrollRect.content.GetChild(0).transform.SetAsLastSibling();
				}
			}

		}
		//getAnswer ();
//		GetSlots ();
//
	}
}

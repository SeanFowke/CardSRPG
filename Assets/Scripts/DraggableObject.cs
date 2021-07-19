using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	// Thanks to Quill18 for the awesome tutorial on draggable card UI

	Vector2 offset;
	Transform returnPoint = null;
	Transform canvas = null;
	CanvasGroup canvasGroup = null;
	public GameObject placeHolder { get; private set; } = null;

	public void OnBeginDrag(PointerEventData data_)
	{
		//just keeps track of the offset between where you click and where the mouse was pressed so we can move from where 
		offset = new Vector2(transform.position.x, transform.position.y) - data_.position;
		//we're setting the parent to the canvas when we drag 
		returnPoint = transform.parent;
		canvas = transform.parent.parent;
		//this whole section is dedicated to how the player can organise their hand and drag cards to different spots
		// basically it works by creating an empty object with the same layout element as our card and moving it around to different 
		// spots realtive to the card location
		placeHolder = new GameObject();
		placeHolder.transform.SetParent(returnPoint);

		LayoutElement elem = placeHolder.AddComponent<LayoutElement>();
		LayoutElement ourElem = GetComponent<LayoutElement>();
		if (ourElem != null)
		{
			elem.preferredWidth = ourElem.preferredWidth;
			elem.preferredHeight = ourElem.preferredHeight;
			elem.flexibleHeight = 0;
			elem.flexibleWidth = 0;
		}

		placeHolder.transform.SetSiblingIndex(transform.GetSiblingIndex());
		// parent it to the canvas so the object can then be parented back to the hand area. This will be the card back and snap it back
		// in line
		transform.SetParent(canvas);
		canvasGroup = GetComponent<CanvasGroup>();

		if (canvasGroup != null)
		{
			canvasGroup.blocksRaycasts = false;
		}
	}

	public void OnDrag(PointerEventData data_)
	{
		transform.position = data_.position + offset;

		int newSiblingIndex = returnPoint.childCount;
		// if the 
		for (int i = 0; i < returnPoint.childCount; i++)
		{
			if (transform.position.x < returnPoint.GetChild(i).position.x)
			{
				newSiblingIndex = i;
				// ignore the placeholder
				if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
				{
					newSiblingIndex--;
				}
				break;
			}
		}

		placeHolder.transform.SetSiblingIndex(newSiblingIndex);
	}

	public void OnEndDrag(PointerEventData data_)
	{

		transform.SetParent(returnPoint);
		transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
		if (canvasGroup != null)
		{
			canvasGroup.blocksRaycasts = true;
		}
	}

	public void SetReturnPoint(Transform point_)
	{
		returnPoint = point_;
	}
}

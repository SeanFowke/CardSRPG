using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropCard : MonoBehaviour, IDropHandler
{
	public void OnDrop(PointerEventData data_)
	{
		DraggableObject d = data_.pointerDrag.GetComponent<DraggableObject>();
		Card c = data_.pointerDrag.GetComponent<Card>();


		if (d != null)
		{
			d.SetReturnPoint(transform);
		}
		// okay why are we checking name, I could instead check for a tag of some sort but creating a whole tag for one object seems
		//wasteful
		if (c != null && d!= null && gameObject.name == "CardDrop")
		{
			c.PlayCard();
			Destroy(c.gameObject);
		}

		Destroy(d.placeHolder);

	}
}

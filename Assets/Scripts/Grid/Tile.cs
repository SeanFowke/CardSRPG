using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
	bool isOccupied;
	int row;
	int column;

	private void Start()
	{

	}

	void Update()
	{
		// I really hate to do this but it's just a prototype
		SelectTile();
	}

	public void SetIsOccupied(bool occupied_)
	{
		isOccupied = occupied_;
	}

	public bool GetIsOccuiped()
	{
		return isOccupied;
	}

	public void Initialise(bool occupied_, int row_, int column_)
	{
		isOccupied = occupied_;
		row = row_;
		column = column_;
	}

	public bool IsOccupied()
	{
		return isOccupied;
	}

	public int GetRow()
	{
		return row;
	}

	public int GetColumn()
	{
		return column;
	}

	public void OnPointerClick(PointerEventData data_)
	{


	}

	public void SelectTile()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
			RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
			if (hit)
			{
				//Debug.Log("Hello");
				if (hit.transform.gameObject.GetComponent<Tile>() == this)
				{
					Debug.Log("Clicked on a tile");
					// if there is a selected unit go and move to this tile if it's not occupied and it's our turn
					if (!isOccupied)
					{
						// reset the previous target to it's original colour
						if (GridManager.instance.GetTargetTile() != null)
						{
							GridManager.instance.GetTargetTile().GetComponent<SpriteRenderer>().color = Color.white;
						}

						// set this as the new target
						GridManager.instance.SetTargetTile(this);
						GetComponent<SpriteRenderer>().color = Color.yellow;

						Debug.Log("New target tile selected");
					}
				}
			}
		}
	}
}

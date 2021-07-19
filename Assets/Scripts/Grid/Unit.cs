using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour
{
	int id;
	[SerializeField] int health;
	int movementRange;
	int row;
	int column;
	[SerializeField] bool isPlayerControlled;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		SelectUnit();
	}

	public void Move(Tile tile_)
	{
		transform.position = tile_.transform.position;
	}

	public void SetId(int id_)
	{
		id = id_;
	}

	public int GetId()
	{
		return id;
	}

	// what happens when this unit is clicked
	public void SelectUnit()
	{
		// tell the gridmanager that this is the unit we want to move
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
			RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
			if (hit)
			{
				//Debug.Log("Hello");
				if (hit.transform.gameObject.GetComponent<Unit>() == this)
				{
					Debug.Log("New Unit Selected");
					if (isPlayerControlled)
					{
						GridManager.instance.SetActiveUnit(this);
						Debug.Log("Set Active Unit");
					}
				}
			}
		}

		
	}

	public void Initialise(int id_, int row_, int column_)
	{
		id = id_;
		row = row_;
		column = column_;
	}

	public void SetRow(int row_)
	{
		row = row_;
	}

	public void SetColumn(int column_)
	{
		column = column_;
	}

	public int GetRow()
	{
		return row;
	}

	public int GetColumn()
	{
		return column;
	}

	public void DamageUnit(int damage_)
	{
		if (health - damage_ > 0)
		{
			health -= damage_;
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

}

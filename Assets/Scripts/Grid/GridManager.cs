using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
	[SerializeField] Transform gridStartLocation;
	[SerializeField] GameObject playerUnitPrefab;
	[SerializeField] GameObject enemyUnitPrefab;
	[SerializeField] int rows;
	[SerializeField] int columns;
	[SerializeField] int tileSize = 1;
	[SerializeField] GameObject tile;
	List<Unit> units = new List<Unit>();
	// 2D array
	Tile[,] tileList;
	Unit activeUnit = null;
	Unit activeAttackTarget = null;
	Tile targetTile = null;


	#region Singleton Notation
	public static GridManager instance { get; private set; }
	private void Awake()
	{
		if (instance == null)
		{
			// create the tile list based off the rows and columns input
			tileList = new Tile[rows, columns];
			instance = this;
			DontDestroyOnLoad(instance);
		}
		else
		{
			Destroy(this.gameObject);
		}

	}
	#endregion



	// Start is called before the first frame update
	void Start()
    {
		CreateGrid();
		SpawnUnits();
		// TEST SUCCESSFUl, counts from 0
		//Tile test;
		//test = GetTile(2, 4);
		//test = GetTile(1, 1);
		//test = GetTile(0, 0);
		//test = GetTile(4, 4);
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			activeUnit = null;
			targetTile = null;
			activeAttackTarget = null;
		}
    }

	void CreateGrid()
	{
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				// spawn the game object and move it over to it's desired location based on the size of the tile
				// we negate the tilesize on the y to move the grid down as we genrate it
				GameObject g = Instantiate(tile, new Vector2(i, j), Quaternion.identity);
				g.transform.position = new Vector2((i * tileSize) + gridStartLocation.position.x, (j * -tileSize) + gridStartLocation.position.y);

				// initialise the tile
				Tile t = g.GetComponent<Tile>();

				if (t != null)
				{
					t.Initialise(false, i, j);
					// add the tile to the tile list
					tileList[i, j] = t;
				}
			}
		}
	}

	void SpawnUnits()
	{
		// Player Units
		Tile SpawnLoc1 = GetTile(0, 3);
		Unit u;
		GameObject g = Instantiate(playerUnitPrefab, new Vector3(SpawnLoc1.transform.position.x, SpawnLoc1.transform.position.y, -1), Quaternion.identity);
		SpawnLoc1.SetIsOccupied(true);
		u = g.GetComponent<Unit>();
		units.Add(u);
		u.Initialise(0, 0, 3);
		// add the the unit to the AI Manager
		AIManager.instance.playerUnits.Add(u);

		Tile SpawnLoc2 = GetTile(0, 2);
		g = Instantiate(playerUnitPrefab, new Vector3(SpawnLoc2.transform.position.x, SpawnLoc2.transform.position.y, -1), Quaternion.identity);
		SpawnLoc2.SetIsOccupied(true);
		units.Add(g.GetComponent<Unit>());
		u = g.GetComponent<Unit>();
		u.Initialise(1, 0, 2);
		AIManager.instance.playerUnits.Add(u);

		// Enemy Units
		Tile SpawnLoc3 = GetTile(4, 2);
		g = Instantiate(enemyUnitPrefab, new Vector3(SpawnLoc3.transform.position.x, SpawnLoc3.transform.position.y, -1), Quaternion.identity);
		SpawnLoc3.SetIsOccupied(true);
		units.Add(g.GetComponent<Unit>());
		u = g.GetComponent<Unit>();
		u.Initialise(2, 4, 2);
		AIManager.instance.enemies.Add(u);

		Tile SpawnLoc4 = GetTile(4, 3);
		g = Instantiate(enemyUnitPrefab, new Vector3(SpawnLoc4.transform.position.x, SpawnLoc4.transform.position.y, -1), Quaternion.identity);
		SpawnLoc4.SetIsOccupied(true);
		units.Add(g.GetComponent<Unit>());
		u = g.GetComponent<Unit>();
		u.Initialise(3, 4, 3);
		AIManager.instance.enemies.Add(u);


	}

	public Tile GetTile(int row_,int column_)
	{
		// check if requested Tile is valid, if not go and give a specific error code and leave
		if (row_ <= rows && column_ <= columns && tileList[row_, column_] != null)
		{
			//Debug.Log("Valid Tile Found");
			return tileList[row_, column_];
		}
		else if (tileList[row_, column_] == null)
		{
			Debug.LogWarning("Tile Requested is Null");
			return null;
		}

		Debug.Log("Invalid Grid Coordinates");
		return null;
	}

	public Unit GetUnit(int id_)
	{
		if (units[id_] != null)
		{
			return units[id_];
		}
		else
		{
			Debug.LogError("GetUnit was found to be null");
			return null;
		}
	}

	// these vectors don't represent world space but grid coords 
	public void MoveUnit(Unit unit_, Tile targetTile_, int movementRange_)
	{
		// very simple manhattan distance formula
		// basically gets the distance in between two different cells and we wanna check if its less than the movement range
		if (unit_ && targetTile_)
		{
			Vector2 startPos = new Vector2(unit_.GetRow(), unit_.GetColumn());
			Vector2 endPos = new Vector2(targetTile_.GetRow(), targetTile_.GetColumn());
			if ((Mathf.Abs(endPos.x - startPos.x) + Mathf.Abs(endPos.y - startPos.y)) <= movementRange_)
			{
				//set the tile the unit is currently at to not occupied
				GetTile((int)startPos.x, (int)startPos.y).SetIsOccupied(false);
				// move to the new tile
				unit_.Move(targetTile_);
				// set the new tile to be occupied
				targetTile_.SetIsOccupied(true);
			}
			else
			{
				Debug.Log("Movement is not within distance");
			}

			targetTile = null;
			activeUnit = null;
		}
		
	}

	public void SetActiveUnit(Unit unit_)
	{
		activeUnit = unit_;
	}

	public Unit GetActiveUnit()
	{
		return activeUnit;
	}

	public void SetAttackTarget(Unit unit_)
	{
		activeAttackTarget = unit_;
	}

	public Unit GetActiveAttackTarget()
	{
		return activeAttackTarget;
	}

	public void SetTargetTile(Tile target_)
	{
		targetTile = target_;
	}

	public Tile GetTargetTile()
	{
		return targetTile;
	}

	public void DamageUnit(Unit attackingUnit_, Unit targetUnit_, int range_, int damage_)
	{
		Vector2 startPos = new Vector2(attackingUnit_.GetRow(), attackingUnit_.GetColumn());
		Vector2 endPos = new Vector2(targetUnit_.GetRow(), targetUnit_.GetColumn());
		// check manhattan distance again
		if ((Mathf.Abs(endPos.x - startPos.x) + Mathf.Abs(endPos.y - startPos.y)) >= range_)
		{
			targetUnit_.DamageUnit(damage_);
		}
		else
		{
			Debug.Log("Not within range to attack");
		}

		activeAttackTarget = null;
		activeUnit = null;
	}

	public int GetRows()
	{
		return rows;
	}
	public int GetColumns()
	{
		return columns;
	}
}

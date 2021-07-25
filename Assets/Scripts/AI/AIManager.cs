using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
	#region Singleton Notation
	public static AIManager instance { get; private set; }
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(instance);
		}
		else
		{
			Destroy(this.gameObject);
		}

	}
	#endregion

	public List<Unit> enemies;
	public List<Unit> playerUnits;
	[SerializeField] int damage;

	public void EnemyTurn()
	{
		foreach (Unit u in enemies)
		{
			// if 0 move if 1 attack
			int r = Random.Range(0, 100);

			if (r >= 50)
			{
				r = 1;
			}
			else
			{
				r = 0;
			}

			switch (r)
			{
				case 0:
					MoveAI(u);
					break;
				case 1:
					AttackEnemy(u);
					break;
			}
		}
	}


	void MoveAI(Unit unit_)
	{
		Debug.Log("AI Moved Unit");
		// go through each enemy
		// find a random target location
		GridManager.instance.MoveUnit(unit_, FindRandomTargetTile(unit_), 4);
	}

	void AttackEnemy(Unit unit_)
	{
		Debug.Log("AI Attacked Unit");
		int rando = Random.Range(0, playerUnits.Count - 1);
		Unit target = playerUnits[rando];
		GridManager.instance.DamageUnit(unit_, target, 4, damage);
	}

	Vector2Int FindRandomLocationOnGrid(Unit unit_)
	{
		Vector2Int targetTile = new Vector2Int(Random.Range(-3, 3) + unit_.GetRow(), Random.Range(-3, 3) + unit_.GetColumn());
		return targetTile;
	}



	Tile FindRandomTargetTile(Unit unit_)
	{
		bool foundLocation = false;
		Vector2Int targetTile = new Vector2Int();

		while (!foundLocation)
		{
			// pick a target tile making sure to take into account the position of the unit
			targetTile = FindRandomLocationOnGrid(unit_);
			// check to make sure target tile is within the boundaries 
			if (targetTile.x > GridManager.instance.GetRows() - 1 || targetTile.x < 0)
			{
				targetTile = FindRandomLocationOnGrid(unit_);
				continue;
			}
			else if (targetTile.y > GridManager.instance.GetColumns() - 1 || targetTile.y < 0)
			{
				targetTile = FindRandomLocationOnGrid(unit_);
				continue;
			}
			else
			{
				foundLocation = true;
			}
		}
		

		Tile t = GridManager.instance.GetTile(targetTile.x, targetTile.y);
		if (t != null && !t.GetIsOccuiped())
		{
			// if it's valid send the data off
			return t;
		}
		else
		{
			// if the tile is invalid go find another tile
			return FindRandomTargetTile(unit_);
		}
	}
}

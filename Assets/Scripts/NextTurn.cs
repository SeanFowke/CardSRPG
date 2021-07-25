using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTurn : MonoBehaviour
{
	// Calls the next turn when the button is pressed
	public void CallNextTurn()
	{
		//for now just call draw a new hand on the Card manager
		CardManager.instance.DrawNewHand();
		// give the enemy their turn
		AIManager.instance.EnemyTurn();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card Stat", menuName = "Card Stat")]
public class CardStats : ScriptableObject
{
	// required in all cases
	public string cardName;
	public string rulesText;
	public int cost;
	public Sprite cardImage;
	public Color innerColour;
	// ID should match the index of the list in the cardmanager
	public int id;
	// 0 if not movement card
	public int movement;
	// if damage is assigned then range must be assigned as well
	public int damage;
	public int range;
	// 

}

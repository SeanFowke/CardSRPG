using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
	#region Singleton Notation
	public static CardManager instance { get; private set; }
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

	// what card was most recently played, added for more functionality down the line
	Card playedCard = null;
	[SerializeField] int energyTotal;
	int energyTotalInit;
	// all of the cards the player has in their deck
	[SerializeField] List<CardStats> deck = new List<CardStats>();
	// has the card already been drawn?
	List<bool> deckDrawnYet = new List<bool>();
	// list of cards the player has in their hand
	List<Card> hand = new List<Card>();
	int handSize = 5;
	List<GameObject> cardsOnTheUI = new List<GameObject>();

	[SerializeField] GameObject cardPrefab;
	[SerializeField] DropCard handArea;

	[SerializeField] Text energyText;

	

	void Start()
	{
		// since we populate the deck list from the editor we can initliase the list with the following 
		for (int i = 0; i < deck.Count; i++)
		{
			deckDrawnYet.Add(false);
			deck[i].id = i;
		}

		energyTotalInit = energyTotal;
		energyText.text = "Energy Total: " + energyTotal.ToString();
		DrawNewHand();
	}

	void Update()
	{

	}

	public void PlayCard(Card card_)
	{
		if (energyTotal - card_.GetStats().cost >= 0)
		{
			// pass all of the info of the played card along to the grid manager including what to actually do

			playedCard = card_;
			energyTotal -= card_.GetStats().cost;

			if (card_.GetStats().damage > 0)
			{
				Debug.Log("Played Card for Damage");
				GridManager.instance.DamageUnit(GridManager.instance.GetActiveUnit(), GridManager.instance.GetActiveAttackTarget(), card_.GetStats().range, card_.GetStats().damage);
			}
			else if (card_.GetStats().movement > 0)
			{
				Debug.Log("Played Card for Movement");
				GridManager.instance.MoveUnit(GridManager.instance.GetActiveUnit(), GridManager.instance.GetTargetTile(), card_.GetStats().movement);
			}

			cardsOnTheUI.Remove(card_.gameObject);
			hand.Remove(card_);

			energyText.text = "Energy Total: " + energyTotal.ToString();
		}
		else
		{
			Debug.Log("Cost is too high to play this card");

		}
	}

	// draws a number up to the maximum hand size
	public void DrawNewHand()
	{
		energyTotal = energyTotalInit;
		// create a new list in indices that haven't been drawn yet
		List<int> cardsThatCanBeDrawn = new List<int>();

		foreach (GameObject g in cardsOnTheUI)
		{
			if (g != null)
			{
				Destroy(g);
			}
		}

		cardsOnTheUI.Clear();
		hand.Clear();

		// draw five new cards
		for (int i = 0; i < handSize; i++)
		{
			// go and find any cards that can be left 
			for (int j = 0; j < deckDrawnYet.Count; j++)
			{
				if (!deckDrawnYet[j])
				{
					// if it hasn't been drawn yet add it to the indices of possible cards
					cardsThatCanBeDrawn.Add(j);
				}	
			}

			// if there are no cards that can be drawn, reshuffle the deck taking into the account the cards we've already added to the hand
			if (cardsThatCanBeDrawn.Count == 0)
			{
				ReshuffleDeck(cardsThatCanBeDrawn);
				i--;
				continue;
			}

			// pick a random number and then load up the associated card
			int r = (int)Random.Range(0, cardsThatCanBeDrawn.Count);
			int rando = cardsThatCanBeDrawn[r];

			if (rando < deck.Count)
			{
				// spawn in the Card game object and fill it with information
				GameObject g = Instantiate(cardPrefab, new Vector2(0, 0), Quaternion.identity);
				cardsOnTheUI.Add(g);
				g.transform.SetParent(handArea.gameObject.transform);

				Card c = g.GetComponent<Card>();
				if (c != null)
				{
					c.SetStats(deck[rando]);
					hand.Add(c);
				}

				deckDrawnYet[rando] = true;
				cardsThatCanBeDrawn.Clear();
			}
		}

		PopulateUI();
	}

	// fill in the UI for the Card Objects
	void PopulateUI()
	{
		for (int i = 0; i < handSize; i++)
		{
			GameObject g = cardsOnTheUI[i];
			if (g != null)
			{
				Card c = g.GetComponent<Card>();

				if (c != null)
				{
					// go find all the UI elements and update them
					c.SetInnerBorderColour(c.GetStats().innerColour);
					c.SetCardNameText(c.GetStats().cardName);
					c.SetCardImage(c.GetStats().cardImage);
					c.SetRulesText(c.GetStats().rulesText);
					c.SetCostText(c.GetStats().cost.ToString());
				}
				else
				{
					Debug.Log("GetComponent was found to be null");
				}
			}
			else
			{
				Debug.Log("Gameobject at index was found to be null");
			}
		}
	}

	// Reshuffle the deck, give it a list of the cards that have already been deemed eligible for 
	public void ReshuffleDeck(List<int> cardsAlreadyChosen_)
	{
		Debug.Log("Reshuffle Deck Called!");
		// reset all the bools back to 0
		for (int i = 0; i < deckDrawnYet.Count; i++)
		{
			deckDrawnYet[i] = false;
		}
		// go and reset the cards already chosen
		for (int j = 0; j < cardsAlreadyChosen_.Count; j++)
		{
			deckDrawnYet[cardsAlreadyChosen_[j]] = true;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
	// scriptable object where we go and 
	[SerializeField] CardStats cardStat;

	[SerializeField] Image innerBorderColour;
	[SerializeField] Image cardImage;
	[SerializeField] Text cardNameText;
	[SerializeField] Text rulesText;
	[SerializeField] Text costText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void PlayCard()
	{
		Debug.Log("Card has been played!");
		CardManager.instance.PlayCard(this);
	}

	public void SetStats(CardStats stats_)
	{
		cardStat = stats_;
	}

	public CardStats GetStats()
	{
		return cardStat;
	}

	public void SetCostText(string text_)
	{
		costText.text = text_;
	}
	public void SetRulesText(string text_)
	{
		rulesText.text = text_;
	}
	public void SetCardNameText(string text_)
	{
		cardNameText.text = text_;
	}
	public void SetInnerBorderColour(Color colour_)
	{
		innerBorderColour.color = colour_;
	}
	public void SetCardImage(Sprite image_)
	{
		cardImage.sprite = image_;
	}
}

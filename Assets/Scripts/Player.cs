using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int PlayerID { get; private set; }
    public int CurrentHP { get; private set; }
    public List<Card> Hand { get; private set; }

    public Player(int playerID, int startingHP)
    {
        PlayerID = playerID;
        CurrentHP = startingHP;
        Hand = new List<Card>();
    }

    public void AddCardToHand(Card card)
    {
        Hand.Add(card);
    }

    public void DisplayPlayerInfo()
    {
        Debug.Log($"Player ID: {PlayerID}, HP: {CurrentHP}");
        Debug.Log("Cards in hand:");
        foreach (Card card in Hand)
        {
            card.DisplayCardInfo();
        }
    }
}

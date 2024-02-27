using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    // Deck attributes
    private List<Card> cards;

    // Constructor to initialize the deck with 10 cards
    public Deck()
    {
        cards = new List<Card>();

        for (int i = 1; i <= 18; i++)
        {
            Card newCard = new Card(i);
            cards.Add(newCard);
        }
    }

    // Method to shuffle the deck
    public void Shuffle()
    {
        System.Random rng = new System.Random();
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
    }

    // Method to deal a card to a player
    public Card DealCardToPlayer(Player player)
    {
        if (cards.Count == 0)
        {
            Debug.Log("The deck is empty. Cannot deal more cards.");
            return null;
        }

        Card dealtCard = cards[0];
        player.AddCardToHand(dealtCard);
        cards.RemoveAt(0);

        return dealtCard;
    }

    // Method to display the current state of the deck
    public void DisplayDeck()
    {
        Debug.Log("Current state of the deck:");
        foreach (Card card in cards)
        {
            card.DisplayCardInfo();
        }
    }
}

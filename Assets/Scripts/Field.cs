using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Field
{
    // Field attributes
    private List<Card>[,] factionAreas;
    private int[,] playerTotalPower;
    private Player[] winningPlayer;
    private Image[,,] images;

    // Constructor to initialize the field with three faction areas
    public Field()
    {
        factionAreas = new List<Card>[3, 2];
        playerTotalPower = new int[3, 2];
        winningPlayer = new Player[3];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                factionAreas[i, j] = new List<Card>();
            }
            winningPlayer[i] = null;
        }
    }

    // Method to play a card onto the field
    public void PlayCard(Player player, Card card, int areaIndex)
    {
        int playerID = player.PlayerID;
        int faction = card.Faction;

        if ((faction != areaIndex) && (card.IsFlipped == false))
        {
            Debug.Log($"Cannot play {card.ToString()} in Area {areaIndex} with faction {faction}. Faction mismatch.");
            return;
        }

        player.Hand.Remove(card);
        factionAreas[areaIndex, playerID].Add(card);
        playerTotalPower[areaIndex, playerID] += card.Power;

        
        UpdateWinningPlayer(areaIndex);
    }

    // Method to determine the winning player in each area
    private void UpdateWinningPlayer(int areaIndex)
    {
        int maxPower = 0;
        Player currentWinningPlayer = null;

        for (int i = 0; i < 2; i++)
        {
            int power = playerTotalPower[areaIndex, i];
            if (power > maxPower)
            {
                maxPower = power;
                currentWinningPlayer = new Player(i, 20); // Assuming starting HP is 20
            }
        }

        winningPlayer[areaIndex] = currentWinningPlayer;
    }

    // Method to display the current state of the field
    public void DisplayFieldState()
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.Log($"Area {i + 1}:");

            if (winningPlayer[i] != null)
            {
                Debug.Log($"Winning Player: {winningPlayer[i].PlayerID}");
            }

            for (int j = 0; j < 2; j++)
            {
                Debug.Log($"Player {j + 1}:");

                foreach (Card card in factionAreas[i, j])
                {
                    card.DisplayCardInfo();
                }

                Debug.Log($"Total Power: {playerTotalPower[i, j]}");
            }

            Debug.Log("--------------");
        }
    }
}

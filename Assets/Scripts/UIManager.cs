using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI player1InfoText;
    public TextMeshProUGUI player2InfoText;
    public GameEngine gameEngine; // Reference to your GameEngine script
    public Dropdown player1Dropdown;

    void Start()
    {
        // Assign the Text elements in the Unity editor
        player1InfoText.text = "Player 1: ";
        player2InfoText.text = "Player 2: ";

        // Access the GameEngine script to display player information
        if (gameEngine != null)
        {
            UpdatePlayerInfo();
            
        }
        else
        {
            Debug.LogError("GameEngine script not assigned to UIManager.");
        }
    }

    void UpdatePlayerInfo()
    {
        // Update the UI with player information from the GameEngine
        
        player1InfoText.text += $"{gameEngine.player1.CurrentHP} HP\n\n";
        player2InfoText.text += $"{gameEngine.player2.CurrentHP} HP\n\n";

        // Add card information to the UI
        player1InfoText.text += "Cards:\n";
        foreach (Card card in gameEngine.player1.Hand)
        {
            player1InfoText.text += $"ID: {card.Id}, Power: {card.Power}, Faction: {card.Faction}\n";
        }

        player2InfoText.text += "Cards:\n";
        foreach (Card card in gameEngine.player2.Hand)
        {
            player2InfoText.text += $"ID: {card.Id}, Power: {card.Power}, Faction: {card.Faction}\n";
        }

        UpdateDropdown(gameEngine.player1, player1Dropdown);
    }

    void UpdateDropdown(Player player, Dropdown dropdown)
    {
        dropdown.ClearOptions();

        // Create a list of options based on cards in hand
        List<string> cardOptions = new List<string>();
        foreach (Card card in player.Hand)
        {
            cardOptions.Add($"ID: {card.Id}, Power: {card.Power}, Faction: {card.Faction}");
        }

        // Populate the Dropdown with card options
        dropdown.AddOptions(cardOptions);

    }
}
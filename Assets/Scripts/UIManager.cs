using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI player1InfoText;
    public TextMeshProUGUI player2InfoText;
    public TextMeshProUGUI gameText;
    public GameEngine gameEngine;
    public Dropdown player1Dropdown;
    public Button odigrajButton;
    public Image cardPreview;
    public Sprite[] cardImages;

    private int selectedCardHandId = 0;


    void Start()
    {
        // Assign the Text elements in the Unity editor
        player1InfoText.text = "Player 1: ";
        player2InfoText.text = "Player 2: ";

        player1Dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        odigrajButton.onClick.AddListener(OnUpdatePowerButtonClick);

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

    void OnDropdownValueChanged(int index)
    {
        selectedCardHandId = index;
        cardPreview.sprite = cardImages[gameEngine.player1.Hand[selectedCardHandId].Id];
    }

    void OnUpdatePowerButtonClick()
    {

        gameText.text = selectedCardHandId + "\n" + gameEngine.player1.Hand[selectedCardHandId].Name;
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

        List<string> cardOptions = new List<string>();
        foreach (Card card in player.Hand)
        {
            cardOptions.Add($"ID: {card.Id}, Power: {card.Power}, Faction: {card.Faction}");
        }

        dropdown.AddOptions(cardOptions);
        cardPreview.sprite = cardImages[gameEngine.player1.Hand[selectedCardHandId].Id];
    }
}
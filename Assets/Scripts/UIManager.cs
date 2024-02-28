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

    public int selectedCardHandId = 0;
    public int selectedArea = 0;


    void Start()
    {
        player1Dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        odigrajButton.onClick.AddListener(OnUpdatePowerButtonClick);

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
        gameEngine.field.PlayCard(gameEngine.player1, gameEngine.player1.Hand[selectedCardHandId], selectedArea);
        selectedCardHandId = 0;
        if (gameEngine.player1.Hand.Count > 0)
            cardPreview.sprite = cardImages[gameEngine.player1.Hand[selectedCardHandId].Id];
        else
        {
            odigrajButton.enabled = false;
            cardPreview.sprite = cardImages[0];
        }

        UpdatePlayerInfo();
    }
    public void OnAreaClick(int index)
    {
        selectedArea = index;
    }

    public void UpdatePlayerInfo()
    {
        
        player1InfoText.text = $"{gameEngine.player1.CurrentHP} HP\n\n";
        player2InfoText.text = $"{gameEngine.player2.CurrentHP} HP\n\n";

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
            cardOptions.Add($"{card.ToString()}");
        }

        dropdown.AddOptions(cardOptions);
        if(gameEngine.player1.Hand.Count> 0)
            cardPreview.sprite = cardImages[gameEngine.player1.Hand[selectedCardHandId].Id];
    }
}
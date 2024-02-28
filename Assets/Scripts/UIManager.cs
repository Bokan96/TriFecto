using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public Image[] P1A0CardImages;
    public Image[] P1A1CardImages;
    public Image[] P1A2CardImages;
    public Image[] P2A0CardImages;
    public Image[] P2A1CardImages;
    public Image[] P2A2CardImages;
    public Sprite[] cardImages;

    public int selectedCardHandId = 0;
    public int selectedCardId;
    public int selectedArea = 0;


    void Start()
    {
        selectedCardId = gameEngine.player1.Hand[selectedCardHandId].Id;
        player1Dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        odigrajButton.onClick.AddListener(OnUpdatePowerButtonClick);
        UpdatePlayerInfo();
    }

    void OnDropdownValueChanged(int index)
    {
        selectedCardHandId = index;
        selectedCardId = gameEngine.player1.Hand[selectedCardHandId].Id;
        cardPreview.sprite = cardImages[selectedCardId];
    }

    void OnUpdatePowerButtonClick()
    {
        if (gameEngine.field.PlayCard(gameEngine.player1, gameEngine.player1.Hand[selectedCardHandId], selectedArea)>0){
            if (selectedArea == 0)
            {
                P1A0CardImages[gameEngine.field.FactionAreas[selectedArea,0].Count-1].sprite = cardImages[selectedCardId];
            }
            else if (selectedArea == 1)
            {
                P1A1CardImages[gameEngine.field.FactionAreas[selectedArea,0].Count-1].sprite = cardImages[selectedCardId];
            }
            else if (selectedArea == 2)
            {
                P1A2CardImages[gameEngine.field.FactionAreas[selectedArea,0].Count-1].sprite = cardImages[selectedCardId];
            }
        }

        selectedCardHandId = 0;
        
        if (gameEngine.player1.Hand.Count > 0)
        {
            selectedCardId = gameEngine.player1.Hand[selectedCardHandId].Id;
            cardPreview.sprite = cardImages[gameEngine.player1.Hand[selectedCardHandId].Id];
        }
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
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;



[System.Serializable]
public class CardArea
{
    public List<PlayerImages> players;
}

[System.Serializable]
public class PlayerImages
{
    public List<UnityEngine.UI.Image> cards;
}

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI player1InfoText;
    public TextMeshProUGUI player2InfoText;
    public TextMeshProUGUI gameText;

    public GameEngine gameEngine;
    public Dropdown player1Dropdown;
    public UnityEngine.UI.Button odigrajButton;
    public UnityEngine.UI.Button[] areaButtons;
    public UnityEngine.UI.Image cardPreview;
    public Sprite[] cardImages;

    public CardArea[] areas;


    public int selectedCardHandId = 0;
    public int selectedCardId;
    public int selectedArea = -1;


    void Start()
    {
        selectedCardId = gameEngine.players[0].Hand[selectedCardHandId].Id;


        player1Dropdown.onValueChanged.AddListener(OnCardSelect);
        odigrajButton.onClick.AddListener(OnPlay);
        UpdatePlayerInfo();
    }

    void OnCardSelect(int index)
    {
        selectedCardHandId = index;
        selectedCardId = gameEngine.players[0].Hand[selectedCardHandId].Id;
        cardPreview.sprite = cardImages[selectedCardId];
    }
    // 0 field.

    void OnPlay()
    {
        if (gameEngine.field.PlayCard(gameEngine.players[0], gameEngine.players[0].Hand[selectedCardHandId], selectedArea)>0){
            areas[selectedArea].players[0].cards[gameEngine.field.FactionAreas[selectedArea, 0].Count-1].sprite = cardImages[selectedCardId];
        }
        selectedCardHandId = 0;
        selectedArea = -1;
        
        if (gameEngine.players[0].Hand.Count > 0)
        {
            selectedCardId = gameEngine.players[0].Hand[selectedCardHandId].Id;
            cardPreview.sprite = cardImages[gameEngine.players[0].Hand[selectedCardHandId].Id];
        }
            else
        {
            odigrajButton.enabled = false;
            cardPreview.sprite = cardImages[0];
        }

        UpdatePlayerInfo();
    }

    public void UpdatePlayerInfo()
    {
        
        player1InfoText.text = $"{gameEngine.players[0].CurrentHP} HP\n\n";
        player2InfoText.text = $"{gameEngine.players[1].CurrentHP} HP\n\n";

        player1InfoText.text += "Cards:\n";
        foreach (Card card in gameEngine.players[0].Hand)
        {
            player1InfoText.text += $"ID: {card.Id}, Power: {card.Power}, Faction: {card.Faction}\n";
        }

        player2InfoText.text += "Cards:\n";
        foreach (Card card in gameEngine.players[1].Hand)
        {
            player2InfoText.text += $"ID: {card.Id}, Power: {card.Power}, Faction: {card.Faction}\n";
        }

        UpdateDropdown(gameEngine.players[0], player1Dropdown);
    }

    void UpdateDropdown(Player player, Dropdown dropdown)
    {
        dropdown.ClearOptions();

        List<string> cardOptions = new List<string>();
        foreach (Card card in player.Hand)
        {
            cardOptions.Add($"{card}");
        }

        dropdown.AddOptions(cardOptions);
        if(gameEngine.players[0].Hand.Count> 0)
            cardPreview.sprite = cardImages[gameEngine.players[0].Hand[selectedCardHandId].Id];
    }


}
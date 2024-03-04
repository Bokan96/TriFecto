using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

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
    public TextMeshProUGUI debugText;
    public TextMeshProUGUI gameText;

    public GameEngine gameEngine;
    public UnityEngine.UI.Button odigrajButton;
    public UnityEngine.UI.Button flipButton;
    public UnityEngine.UI.Button[] areaButtons;
    public UnityEngine.UI.Image[] areaImages;
    public UnityEngine.UI.Image cardPreview;
    public Sprite[] cardImages;

    public CardArea[] areas;
    public GameObject[] cardsInHand;


    public int selectedCardHandId = 0;
    public Card selectedCard;
    public int selectedArea = -1;

    public int currentPlayer;


    void Start()
    {
        currentPlayer = 0;
        ShowHand();

        selectedCard = gameEngine.players[currentPlayer].Hand[selectedCardHandId];
        odigrajButton.onClick.AddListener(OnPlay);
        flipButton.onClick.AddListener(OnFlip);
        UpdatePlayerInfo();
    }

    void OnCardSelect(int index)
    {
        selectedCardHandId = index;
        selectedCard = gameEngine.players[currentPlayer].Hand[selectedCardHandId];
        ShowCard(selectedCard);
    }

    void OnPlay2()
    {
        Player player = gameEngine.players[currentPlayer];
        Card card = player.Hand[selectedCardHandId];
        if (gameEngine.field.PlayCard(gameEngine.players[currentPlayer], gameEngine.players[currentPlayer].Hand[selectedCardHandId], selectedArea) > 0)
        {
            areas[selectedArea].players[currentPlayer].cards[gameEngine.field.FactionAreas[selectedArea, currentPlayer].Count - 1].enabled = true;
            areas[selectedArea].players[currentPlayer].cards[gameEngine.field.FactionAreas[selectedArea, currentPlayer].Count - 1].sprite = cardImages[0];
            if (gameEngine.field.FactionAreas[selectedArea, currentPlayer][gameEngine.field.FactionAreas[selectedArea, currentPlayer].Count - 1].IsFlipped == false)
                areas[selectedArea].players[currentPlayer].cards[gameEngine.field.FactionAreas[selectedArea, currentPlayer].Count - 1].sprite = cardImages[selectedCard.Id];
            selectedCardHandId = 0;
        }

        SelectArea(-1);
        ShowHand();
        if (gameEngine.players[currentPlayer].Hand.Count > 0)
        {
            OnCardSelect(selectedCardHandId);
        }
        else
        {
            odigrajButton.enabled = false;
            cardPreview.sprite = cardImages[0];
        }

        UpdatePlayerInfo();
    }

    void OnPlay()
    {
        Player player = gameEngine.players[currentPlayer];
        Card card = player.Hand[selectedCardHandId];

        if (gameEngine.field.PlayCard(player, card, selectedArea)>0){
            areas[selectedArea].players[currentPlayer].cards[gameEngine.field.FactionAreas[selectedArea, currentPlayer].Count - 1].gameObject.SetActive(true);
            areas[selectedArea].players[currentPlayer].cards[gameEngine.field.FactionAreas[selectedArea, currentPlayer].Count - 1].sprite = CardSprite(selectedCard);
            selectedCardHandId = 0;
        }
        
        SelectArea(-1);
        ShowHand();
        if (gameEngine.players[currentPlayer].Hand.Count > 0)
        {
            OnCardSelect(selectedCardHandId);
        }
            else
        {
            odigrajButton.enabled = false;
            cardPreview.sprite = cardImages[0];
        }

        UpdatePlayerInfo();
    }

    public void OnFlip()
    {
        gameEngine.players[currentPlayer].Hand[selectedCardHandId].Flip();
        OnCardSelect(selectedCardHandId);
        ShowHand();
    }


    public void UpdatePlayerInfo()
    {
        debugText.text = "Teren:\n";
        for(int a = 0; a < 3;a++)
        {
            for (int p = 0;p< 2; p++)
            {
                for(int c = 0; c < gameEngine.field.factionAreas[a, p].Count; c++)
                {
                    debugText.text += "a=" + a + " p=" + p + " " + gameEngine.field.factionAreas[a, p][c] + "\n";
                }
            }
        }
    }


    public void ShowCard(Card card)
    {
        if (card.IsFlipped)
            cardPreview.sprite = cardImages[0];
        else
            cardPreview.sprite = cardImages[card.Id];
    }
    public Sprite CardSprite(Card card)
    {
        if (card.IsFlipped)
            return cardImages[0];
        else
            return cardImages[card.Id];
    }
    public void SwitchPlayerUI()
    {
        NextPlayer();
        ShowHand();
    }
    public void NextPlayer()
    {
        currentPlayer = (currentPlayer + 1) % gameEngine.players.Length;
        SelectArea(-1);
        selectedCardHandId = 0;
        selectedCard = gameEngine.players[currentPlayer].Hand[selectedCardHandId];
    }

    public void SelectArea(int area)
    {
        selectedArea = area;
        for (int i = 0; i<areaImages.Length; i++)
        {
            areaImages[i].transform.localScale = Vector3.one;
            areaImages[i].color = Color.gray;
        }
        if(area != -1)
        {
            areaImages[area].transform.localScale = Vector3.one * 1.1f;
            areaImages[area].color = Color.white;
        }
            
    }

    public void ShowHand()
    {
        for (int i = 0; i<6;i++)
        {
            cardsInHand[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < gameEngine.players[currentPlayer].Hand.Count; i++)
        {
            cardsInHand[i].gameObject.SetActive(true);
            Image imageOfCardHand = cardsInHand[i].GetComponent<Image>();
            imageOfCardHand.sprite = CardSprite(gameEngine.players[currentPlayer].Hand[i]);
            if(selectedCardHandId == i)
            {
                imageOfCardHand.transform.localScale = Vector3.one * 1.2f;
                imageOfCardHand.color = Color.white;
            }
            else
            {
                imageOfCardHand.transform.localScale = Vector3.one;
                imageOfCardHand.color = Color.grey;
            }
                
        }
    }
}
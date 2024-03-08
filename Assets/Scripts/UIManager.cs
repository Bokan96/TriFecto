using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
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
    public static TextMeshProUGUI gameText;
    public static UnityEngine.UI.Button playButton;
    public static UnityEngine.UI.Button flipButton;
    public static UnityEngine.UI.Button endTurnButton;
    public static UnityEngine.UI.Button targetSelectButton;
    public static GameObject selectHand;
    public static GameObject selectField;

    public GameEngine gameEngine;

    public GameObject[] uIElements;
    public UnityEngine.UI.Button[] areaButtons;
    public Image[] areaImages;
    public Image cardPreview;
    public Sprite[] cardImages;

    public CardArea[] areas;
    public GameObject[] cardsInHand;


    public static int selectedCardHandId = -1;
    public static Card selectedCardHand;
    public static Card selectedCardField;
    public static int selectedArea = -1;

    public static int currentPlayer;
    private Card playedCard;


    void Start()
    {
        currentPlayer = 0;
        gameText = GameObject.Find("GameText").GetComponent<TextMeshProUGUI>();

        playButton = GameObject.Find("ButtonPlay").GetComponent<UnityEngine.UI.Button>();
        playButton.gameObject.SetActive(false);
        flipButton = GameObject.Find("ButtonFlip").GetComponent<UnityEngine.UI.Button>();
        flipButton.gameObject.SetActive(false);
        endTurnButton = GameObject.Find("ButtonEnd").GetComponent<UnityEngine.UI.Button>();
        targetSelectButton = GameObject.Find("ButtonSelect").GetComponent<UnityEngine.UI.Button>();
        selectHand = GameObject.Find("SelectHand");
        selectField = GameObject.Find("SelectField");
        selectField.gameObject.SetActive(false);

        ShowHand();


        playButton.onClick.AddListener(OnPlay);
        flipButton.onClick.AddListener(OnFlip);
        UpdatePlayerInfo();
    }

    public void OnCardHandSelect(int index)
    {
        selectedCardHandId = index;

        if (index == -1)
        {
            playButton.gameObject.SetActive(false);
            flipButton.gameObject.SetActive(false);
            return;
        }
        
        selectedCardHand = GameEngine.players[currentPlayer].Hand[selectedCardHandId];

        if (selectedCardHand.Playable(GameEngine.players[currentPlayer],selectedArea))
            playButton.gameObject.SetActive(true);
        else
            playButton.gameObject.SetActive(false);
        flipButton.gameObject.SetActive(true);

        ShowCard(selectedCardHand);
    }

    void OnPlay()
    {
        if (selectedCardHandId == -1)
        {
            Debug.Log("Moras izabrati kartu iz ruke.");
            return;
        }
        else if (selectedArea == -1)
        {
            Debug.Log("Moras izabrati teren.");
            return;
        }

        Player player = GameEngine.players[currentPlayer];
        Card card = player.Hand[selectedCardHandId];

        if (card.Playable(player, selectedArea))
        {
            player.Hand.Remove(card);
            GameEngine.field.factionAreas[selectedArea, player.PlayerID].Add(card);
            ShowField();
            PlayCard(player, card, selectedArea);
        }

        SelectArea(-1);
        selectedCardHandId = -1;
        ShowHand();
        OnCardHandSelect(selectedCardHandId);

        UpdatePlayerInfo();
    }

    public void PlayCard(Player player, Card card, int area)    //efekti nakon sto karta dodje na teren
    {
        playedCard = card;
        card.Area = area;
        

        if (card.Id == 3 || card.Id == 9 || card.Id == 15)
        {
            Debug.Log("odigrana flip karta");
            if (GameEngine.field.totalCards() - GameEngine.field.totalCards(area) > 0)
            {
                SwapUiElements();
            }
            else
            {
                Debug.Log("Na susednom terenu nema nijedna karta.");
            }
            

        }
    }

    public void OnFlip()
    {
        if (selectedCardHandId == -1)
        {
            Debug.Log("Moras izabrati kartu iz ruke.");
            return;
        }

        GameEngine.players[currentPlayer].Hand[selectedCardHandId].Flip();
        OnCardHandSelect(selectedCardHandId);
        ShowHand();
    }

    public void TargetSelect()
    {
        if (GameEngine.field.isOnTopOfArea(selectedCardField))
        {
            if(playedCard.Id == 3 || playedCard.Id == 9 || playedCard.Id == 15)
                if (AdjecentAreas(playedCard.Area).Contains(selectedCardField.Area))
                {
                    selectedCardField.Flip();
                    ShowField();
                    SwapUiElements();
                }
                else
                {
                    gameText.text = "Mozes okrenuti kartu samo sa susednog terena";
                }

            
        }
        else
        {
            Debug.Log("Moras odabrati kartu sa vrha terena");
        }
    }

    public void UpdatePlayerInfo()
    {
        debugText.text = "Teren:\n";
        for (int a = 0; a < 3; a++)
        {
            for (int p = 0; p < 2; p++)
            {
                for (int c = 0; c < GameEngine.field.factionAreas[a, p].Count; c++)
                {
                    debugText.text += "a=" + a + " p=" + p + " " + GameEngine.field.factionAreas[a, p][c] + "\n";
                }
            }
        }
    }

    public void ShowCard(Card card)
    {
        cardPreview.sprite = CardSprite(card);
        ShowHand();
    }

    public Sprite CardSprite(Card card)
    {
        if (card.IsFlipped)
            return cardImages[0];
        else
            return cardImages[card.Id];
    }

    public void ShowField()
    {
        for(int a = 0;a < 3;a++)
            for(int p = 0;p < 2;p++)
                for (int c = 0; c < GameEngine.field.factionAreas[a, p].Count; c++)
                {
                    areas[a].players[p].cards[c].gameObject.SetActive(true);
                    areas[a].players[p].cards[c].sprite = CardSprite(GameEngine.field.factionAreas[a, p][c]);
                }
    }
    public void SwitchPlayerUI()
    {
        currentPlayer = (currentPlayer + 1) % GameEngine.players.Length;
        ResetAllSelections();
        ShowHand();
    }

    public void SelectArea(int area)
    {
        selectedArea = area;
        for (int i = 0; i < areaImages.Length; i++)
        {
            areaImages[i].transform.localScale = Vector3.one;
            areaImages[i].color = Color.gray;
        }
        if (area != -1)
        {
            areaImages[area].transform.localScale = Vector3.one * 1.1f;
            areaImages[area].color = Color.white;
        }

        if (selectedCardHand.Playable(GameEngine.players[currentPlayer],selectedArea))
        {
            playButton.gameObject.SetActive(true);
        }
        else
        {
            playButton.gameObject.SetActive(false);
        }

    }

    public void ShowHand()
    {
        for (int i = 0; i < 6; i++)
        {
            cardsInHand[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < GameEngine.players[currentPlayer].Hand.Count; i++)
        {
            cardsInHand[i].gameObject.SetActive(true);
            Image imageOfCardHand = cardsInHand[i].GetComponent<Image>();
            imageOfCardHand.sprite = CardSprite(GameEngine.players[currentPlayer].Hand[i]);
            if (selectedCardHandId == i)
            {
                imageOfCardHand.transform.localScale = Vector3.one * 1.3f;
                imageOfCardHand.color = Color.white;
                imageOfCardHand.transform.rotation = Quaternion.Euler(0, 0, 5);
            }
            else
            {
                imageOfCardHand.transform.localScale = Vector3.one;
                imageOfCardHand.color = Color.grey;
                imageOfCardHand.transform.rotation = Quaternion.Euler(Vector3.zero);
            }

        }
    }

    public void SwapUiElements()
    {
        selectHand.gameObject.SetActive(!selectHand.gameObject.activeInHierarchy);
        selectField.gameObject.SetActive(!selectField.gameObject.activeInHierarchy);
    }

    public void ShowUiElement(int element)
    {
        for(int i=0; i<uIElements.Length; i++)
        {
            uIElements[i].SetActive(false);
        }
        uIElements[element].SetActive(true);
    }

    public int[] AdjecentAreas(int area)
    {
        int[] adjecentAreas;
        if (area == 1)
        {
            adjecentAreas = new int[2];
            adjecentAreas[0] = 0;
            adjecentAreas[1] = 2;
        }
        else
        {
            adjecentAreas = new int[1];
            adjecentAreas[0] = 1;
        }
        return adjecentAreas;
    }

    public void ResetAllSelections()
    {
        selectedArea = -1;
        selectedCardField = null;
        selectedCardHand = null;
        selectedCardHandId = -1;
        cardPreview.sprite = cardImages[0];
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
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
    public static Button playButton;
    public static Button flipButton;
    public static Button endTurnButton;
    public static Button targetSelectButton;
    public static GameObject selectHand;
    public static GameObject selectField;
    public static GameObject handArea;
    public static AudioSource soundClick1;
    public static AudioSource soundClick2;
    public static AudioSource soundUI;
    public static AudioSource soundError;
    public static AudioSource soundTerrain;
    public static AudioSource soundFlip;
    public static Text targetDescription;

    public GameEngine gameEngine;

    public GameObject[] uIElements;
    public UnityEngine.UI.Button[] areaButtons;
    public Image[] areaImages;
    public Image cardPreview;
    public Sprite[] cardImages;

    public CardArea[] areas;
    public GameObject[] cardsInHand;
    public Sprite[] backgroundImages;


    public static int selectedCardHandId = -1;
    public static Card selectedCardHand;
    public static Card selectedCardField;
    public static int selectedArea = -1;

    public static int currentPlayer;
    private Card playedCard;
    private bool animationInProgress;
    private bool cardPlayed;
    private Image cardPreviewPlayed;

    void getComponents()
    {
        gameText = GameObject.Find("GameText").GetComponent<TextMeshProUGUI>();

        playButton = GameObject.Find("ButtonPlay").GetComponent<UnityEngine.UI.Button>();
        flipButton = GameObject.Find("ButtonFlip").GetComponent<UnityEngine.UI.Button>();
        endTurnButton = GameObject.Find("ButtonEnd").GetComponent<UnityEngine.UI.Button>();
        targetSelectButton = GameObject.Find("ButtonSelect").GetComponent<UnityEngine.UI.Button>();

        selectHand = GameObject.Find("SelectHand");
        handArea = GameObject.Find("HandArea");
        selectField = GameObject.Find("SelectField");

        targetDescription = GameObject.Find("TargetDescription").GetComponent<Text>();
        cardPreviewPlayed = GameObject.Find("CardPreviewPlayed").GetComponent<Image>();

        soundClick1 = GameObject.Find("ClickSound1").GetComponent<AudioSource>();
        soundClick2 = GameObject.Find("ClickSound2").GetComponent<AudioSource>();
        soundUI = GameObject.Find("UISound").GetComponent<AudioSource>();
        soundError = GameObject.Find("ErrorSound").GetComponent<AudioSource>();
        soundTerrain = GameObject.Find("TerrainSound").GetComponent<AudioSource>();
        soundFlip = GameObject.Find("FlipSound").GetComponent<AudioSource>(); 

        selectField.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        flipButton.gameObject.SetActive(false);
        targetDescription.gameObject.SetActive(false);

        playButton.onClick.AddListener(OnPlay);
        flipButton.onClick.AddListener(OnFlip);
    }

    void Start()
    {
        getComponents();

        SwitchPlayerUI();
        currentPlayer = 0;
        cardPlayed = false;
        SelectArea(-1);
        ShowHand();
        
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

        soundClick2.Play();

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

        if (player.PlayerID != currentPlayer)
        {
            SwitchPlayerUI();
            selectedCardField = card;
        }
            
        GameObject image = areas[area].players[player.PlayerID].cards[GameEngine.field.totalCards(area,player)-1].gameObject;
        
        

        LeanTween.moveLocalX(image, 30f, 0.05f)
            .setLoopPingPong(3)
            .setEase(LeanTweenType.easeInBounce);


        if (card.IsFlipped)
            return;

        //efekti karata ispod

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

        soundFlip.Play();

        GameObject imageOfCard = GameObject.Find($"Card{selectedCardHandId}");

        if (!animationInProgress)
        {
            animationInProgress = true;
            LeanTween.rotateAroundLocal(imageOfCard, Vector3.forward, 360f, 0.2f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                animationInProgress = false;
            });
        }
        

        GameEngine.players[currentPlayer].Hand[selectedCardHandId].Flip();
        OnCardHandSelect(selectedCardHandId);
        ShowHand();
    }

    public Image getFieldImage(Card card)
    {
        for(int a = 0; a < 3; a++)
        {
            for(int p = 0; p < 2; p++)
            {
                for(int c = 0; c < GameEngine.field.FactionAreas[a,p].Count; c++)
                {
                    if(GameEngine.field.FactionAreas[a, p][c].Id == card.Id)    //error
                        return areas[a].players[p].cards[c];
                }
            }
        }
        Debug.Log("Ne postoji ta karta flag4");
        return null;
    }

    public void TargetSelect()
    {
        if (GameEngine.field.isOnTopOfArea(selectedCardField))
        {
            if(playedCard.Id == 3 || playedCard.Id == 9 || playedCard.Id == 15)
                if (AdjecentAreas(playedCard.Area).Contains(selectedCardField.Area))
                {
                    selectedCardField.Flip();
                    soundFlip.Play();
                    PlayCard(selectedCardField.Player, selectedCardField, selectedCardField.Area);
                    Image imageOfCard = getFieldImage(selectedCardField);

                    if (!animationInProgress)
                    {
                        animationInProgress = true;
                        LeanTween.rotateAroundLocal(imageOfCard.gameObject, Vector3.forward, 360f, 0.2f)
                        .setEase(LeanTweenType.easeInOutQuad)
                        .setOnComplete(() =>
                        {
                            animationInProgress = false;
                        });
                    }

                    ShowField();
                    SwapUiElements();
                }
                else
                {
                    soundError.Play();
                    targetDescription.text = "Mozes okrenuti kartu samo sa susednog terena";
                }

            
        }
        else
        {
            soundError.Play();
            targetDescription.text = "Moras odabrati kartu sa vrha terena";
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
        PreviewCardAnimation();

        ShowHand();
    }

    public void PreviewCardAnimation()
    {
        if (animationInProgress)
            return;

        animationInProgress = true;

        Vector3 startingPosition = cardPreview.transform.position;

        LeanTween.move(cardPreview.gameObject, cardPreview.transform.position + new Vector3(0f, 290f, 0f), 0.7f)
            .setEase(LeanTweenType.easeOutExpo)
            .setOnComplete(() =>
            {
                LeanTween.move(cardPreview.gameObject, cardPreview.transform.position - new Vector3(0f, 290f, 0f), 0.7f)
                    .setEase(LeanTweenType.easeOutExpo)
                    .setOnComplete(() =>
                    {
                        animationInProgress = false;
                    });
                
            });
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
        soundUI.Play();
        ResetAllSelections();

        Image backGroundImage = GameObject.Find("OffScreen").GetComponent<Image>();
        backGroundImage.sprite = backgroundImages[currentPlayer];

        GameObject[] cardPlayAreas = GameObject.FindGameObjectsWithTag("CardPlayArea");

        foreach (GameObject area in cardPlayAreas)
        {
            RectTransform rectTransform = area.GetComponent<RectTransform>();
            VerticalLayoutGroup layout = area.GetComponent<VerticalLayoutGroup>();
            if (rectTransform != null)
            {
                Vector2 currentAnchoredPosition = rectTransform.anchoredPosition;
                currentAnchoredPosition.y = -currentAnchoredPosition.y;
                rectTransform.anchoredPosition = currentAnchoredPosition;
                if (layout.childAlignment == TextAnchor.UpperCenter)
                    layout.childAlignment = TextAnchor.LowerCenter;
                else
                    layout.childAlignment = TextAnchor.UpperCenter;
            }
        }

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

        if (selectedCardHand!=null && selectedCardHand.Playable(GameEngine.players[currentPlayer],selectedArea))
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
                LeanTween.scale(imageOfCardHand.gameObject, new Vector3(1.3f, 1.3f, 1.3f), 0.2f);
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
        handArea.gameObject.SetActive(!handArea.gameObject.activeInHierarchy);
        targetDescription.gameObject.SetActive(!targetDescription.gameObject.activeInHierarchy);
        if(targetDescription.gameObject.activeInHierarchy)
        {
            cardPreviewPlayed.sprite = cardImages[playedCard.Id];
            cardPreview.sprite = cardImages[playedCard.Id];
        }
        
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
        SelectArea(-1);
        selectedCardField = null;
        selectedCardHand = null;
        selectedCardHandId = -1;
        cardPreview.sprite = cardImages[19];
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandSelect : MonoBehaviour
{
    public int handId;
    public UIManager uIManager;
    public void OnClick()
    {
        Card card = uIManager.gameEngine.players[uIManager.currentPlayer].Hand[handId];
        uIManager.selectedCardHandId = handId;
        uIManager.selectedCard = card;
        uIManager.cardPreview.sprite = uIManager.CardSprite(card);
        uIManager.ShowHand();
    }
}

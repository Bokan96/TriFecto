using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandSelect : MonoBehaviour
{
    public int handId;
    public UIManager uIManager;
    public void OnClick()
    {
        Card card = GameEngine.players[UIManager.currentPlayer].Hand[handId];
        UIManager.selectedCardHandId = handId;
        UIManager.selectedCardHand = card;
        uIManager.cardPreview.sprite = uIManager.CardSprite(card);
        uIManager.ShowHand();
    }

    public void OnClick2(int index)
    {
        uIManager.OnCardHandSelect(index);
    }
}

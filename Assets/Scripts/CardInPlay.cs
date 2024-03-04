using UnityEngine;
using UnityEngine.UI;

public class CardInPlay : MonoBehaviour
{
    public UIManager uIManager;
    public int player;
    public int area;
    public int card;

    private Image image;

    public void OnClickCard()
    {
        image = GetComponent<Image>();
        uIManager.cardPreview.sprite = image.sprite;
    }

    public void OnClickCard2()
    {
        Card selectedCard;
        if(uIManager.gameEngine.field.factionAreas[area, player].Count - 1 >= card)
        {
            selectedCard = uIManager.gameEngine.field.factionAreas[area, player][card];
            uIManager.ShowCard(selectedCard);
            Debug.Log($"Selected {uIManager.gameEngine.field.factionAreas[area, player][card]}");
        }
        else
        {
            Debug.Log("Nema karte tu.");
        }
    }
}
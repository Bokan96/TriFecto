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
        if(uIManager.gameEngine.field.factionAreas[area, player].Count - 1 >= card)
        {
            uIManager.cardPreview.sprite = uIManager.cardImages[uIManager.gameEngine.field.factionAreas[area, player][card].Id];
            Debug.Log($"Selected {uIManager.gameEngine.field.factionAreas[0, 0][0]}");
        }
        else
        {
            Debug.Log("Nema karte tu.");
        }
    }
}
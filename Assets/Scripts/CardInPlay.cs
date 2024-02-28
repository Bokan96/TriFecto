using UnityEngine;
using UnityEngine.UI;

public class CardInPlay : MonoBehaviour
{
    public int Id = 0;
    public bool IsFlipped = false;
    public Image image;
    public UIManager uIManager;
    
    public void ChangeId(int newId)
    {
        Id = newId;
        image.sprite = uIManager.cardImages[Id];
        if(newId == -1)
        {
            image.sprite = null;
        }
    }
    public void Flip()
    {
        IsFlipped = !IsFlipped;
    }
    public void Flip(bool flipped)
    {
        IsFlipped = flipped;
    }
}
using UnityEngine;
using UnityEngine.UI;

public class SelectArea : MonoBehaviour
{
    public UIManager uiManager;
    public int areaIndex;
    public Image[] otherAreaImages;


    public void OnImageClick()
    {
        uiManager.selectedArea = areaIndex;
        Debug.Log("Selected" + areaIndex);
    }
}
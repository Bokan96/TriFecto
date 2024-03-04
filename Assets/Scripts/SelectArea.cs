using UnityEngine;
using UnityEngine.UI;

public class SelectArea : MonoBehaviour
{
    public UIManager uiManager;
    public int areaIndex;
    public void OnImageClick()
    {
        uiManager.SelectArea(areaIndex);
        this.transform.SetAsLastSibling();
        Debug.Log("Selected" + areaIndex);
    }
}
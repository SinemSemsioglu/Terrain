 
using UnityEngine;
using UnityEngine.EventSystems;
 
public class DoubleClick : MonoBehaviour, IPointerClickHandler {
 
    public virtual void OnPointerClick(PointerEventData data)
    {
        if (data.clickCount == 2)
        {
            Debug.Log("double click");
        }
    }
}
 
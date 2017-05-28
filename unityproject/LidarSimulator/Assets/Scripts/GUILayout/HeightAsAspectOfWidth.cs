using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sets the preferredHeight of the layoutComponent as an aspect of the width of the RectTransform
/// 
/// @author: Jonathan Jansson
/// </summary>
public class HeightAsAspectOfWidth : MonoBehaviour {

    public float aspect = 1f;

    /// <summary>
    /// Checks so that the gameObject has a LayoutElement component
    /// </summary>
    void Start()
    {
        if (gameObject.GetComponent<LayoutElement>())
        {
            Invoke("SetPreferedWidthAsAspect", 0.01f);
        }
    }
    
    void SetPreferedWidthAsAspect()
    {
        float height = gameObject.GetComponent<RectTransform>().rect.width * aspect;
        gameObject.GetComponent<LayoutElement>().preferredHeight  = Mathf.Abs(height);
    }
}

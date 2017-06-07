using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sets the preferredWidth of the recttransform size as the same as the largest width of the children of the object
/// 
/// @author: Jonathan Jansson
/// </summary>
public class WidthAsChildren : MonoBehaviour {

    /// <summary>
    /// Waits for all other GUI initiations to be set before calling the method which changes the width
    /// </summary>
	void Start ()
    {
        if (gameObject.GetComponent<LayoutElement>())
        {
            Invoke("SetPreferedWidthAsChildren", 0.01f);
        }
    }

    /// <summary>
    /// Itterates through all children of the gameObject and sets the preferred width of the layoutelement component as the width of the widest child
    /// </summary>
	private void SetPreferedWidthAsChildren()
    {
        float maxWidth = 0f;
        
        foreach (RectTransform childRect in transform.GetComponentsInChildren<RectTransform>())
        {
            if (!childRect.name.Equals(transform.name) && childRect.rect.width + childRect.localPosition.x > maxWidth)
            {
                maxWidth = childRect.rect.width + childRect.localPosition.x;
            }
        }
        
        gameObject.GetComponent<LayoutElement>().preferredWidth = Mathf.Abs(maxWidth);
    }
}

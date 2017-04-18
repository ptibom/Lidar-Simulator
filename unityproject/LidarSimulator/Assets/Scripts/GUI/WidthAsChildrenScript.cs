using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A script which sets the with of the recttransform size as the same as the largest width of the children of the object
/// @author: Jonathan Jansson
/// </summary>
public class WidthAsChildrenScript : MonoBehaviour {

    /// <summary>
    /// Waits for all other GUI initializations to be set before calling the method which sets the width
    /// </summary>
	void Start ()
    {
        Invoke("setPreferedWidthAsChildren", 0.01f);
	}

    /// <summary>
    /// Itterates through all children of the gameObject and sets the preferred width of the layoutelement component as the width of the widest child
    /// </summary>
	private void setPreferedWidthAsChildren()
    {
        float maxWidth = 0;

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

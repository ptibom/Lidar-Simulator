using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sets the preferredWidth of the layoutComponent as an aspect of the height of the RectTransform
/// 
/// @author: Jonathan Jansson
/// </summary>
public class WidthAsAspectOfHeight : MonoBehaviour {

    public float aspect = 1f;

    /// <summary>
    /// Checks so that the gameObject has a LayoutElement component
    /// </summary>
	void Start ()
    {
        if (gameObject.GetComponent<LayoutElement>())
        {
            Invoke("SetPreferedWidthAsAspect", 0.01f);
        }
	}
	
	void SetPreferedWidthAsAspect() {
        float width = gameObject.GetComponent<RectTransform>().rect.height * aspect;
        gameObject.GetComponent<LayoutElement>().preferredWidth = Mathf.Abs(width);
    }
}

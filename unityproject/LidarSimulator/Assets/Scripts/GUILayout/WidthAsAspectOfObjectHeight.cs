using UnityEngine;

/// <summary>
/// Sets the width of the RectTransform to an aspect of the squareroot of the height of the passed in object.
/// The reason for taking the squareroot of the height is so that, when changing to resolutions with different 
/// aspect ratios, the height difference will have less impact on the width. Thus not streching the widht too much.
/// 
/// @author: Jonathan Jansson
/// </summary>
public class WidthAsAspectOfObjectHeight : MonoBehaviour {

    public float aspect = 1f;
    public RectTransform objectRectTransform;

	void Start () {
        SetWidthAsAspectOfObject();
	}
	
    void SetWidthAsAspectOfObject()
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(Mathf.Sqrt(objectRectTransform.rect.height) * aspect, rectTransform.rect.height);
    }
}

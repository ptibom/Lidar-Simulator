using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidthAsAspectOfObject : MonoBehaviour {

    public float aspect = 1f;
    public RectTransform objectRectTransform;
    public float normalizedRealAspect = 0f;
	// Use this for initialization
	void Start () {
        SetWidthAsAspectOfObject();
	}
	
    void SetWidthAsAspectOfObject()
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(Mathf.Sqrt(objectRectTransform.rect.height) * aspect, rectTransform.rect.height);
    }
}

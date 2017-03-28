using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WidthAsChildrenScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Invoke("setPreferedWidthAsChildren", 0.01f);
	}

	void setPreferedWidthAsChildren()
    {
        float maxWidth = 0;

        foreach (RectTransform childRect in transform.GetComponentsInChildren<RectTransform>())
        {
            if (!childRect.name.Equals(transform.name) && childRect.rect.width > maxWidth)
            {
                maxWidth = childRect.rect.width;
            }
        }

        gameObject.GetComponent<LayoutElement>().preferredWidth = Mathf.Abs(maxWidth);
    }
}

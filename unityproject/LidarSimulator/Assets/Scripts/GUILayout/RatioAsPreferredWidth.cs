using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatioAsPreferredWidth : MonoBehaviour {

    public float aspect = 1f;

	void Start () {
        Invoke("SetPreferedWidthAsAspect", 0.01f);
	}
	
	void SetPreferedWidthAsAspect() {
        float width = gameObject.GetComponent<RectTransform>().rect.height * aspect;
        gameObject.GetComponent<LayoutElement>().preferredWidth = Mathf.Abs(width);
    }
}

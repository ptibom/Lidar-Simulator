using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatioAsPreferredHeight : MonoBehaviour
{

    public float aspect = 1f;

    void Start()
    {
        Invoke("SetPreferedWidthAsAspect", 0.01f);
    }
    
    void SetPreferedWidthAsAspect()
    {
        float height = gameObject.GetComponent<RectTransform>().rect.width * aspect;
        gameObject.GetComponent<LayoutElement>().preferredHeight  = Mathf.Abs(height);
    }
}

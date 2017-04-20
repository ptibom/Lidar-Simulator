using UnityEngine;

/// <summary>
/// Fixes unitys rendering order problem with transparent materials
/// 
/// @author: Jonathan Jansson
/// </summary>

public class TransperentMaterialFix : MonoBehaviour {

    public Material mat1;
    public Material mat2;
    public Material mat3;

    void Start()
    {
        mat1.SetInt("_ZWrite", 1);
        mat2.SetInt("_ZWrite", 1);
        mat3.SetInt("_ZWrite", 1);
    }
}
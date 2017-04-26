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
    public Material mat4;
    public Material mat5;

    void Start()
    {
        mat1.SetInt("_ZWrite", 1);
        mat2.SetInt("_ZWrite", 1);
        mat3.SetInt("_ZWrite", 1);
        mat4.SetInt("_ZWrite", 1);
        mat5.SetInt("_ZWrite", 1);
    }
}
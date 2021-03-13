using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Creates a backgound gradient texture
/// Ref: https://stackoverflow.com/a/44821232
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(RawImage))]
public class BackgroundGradient : MonoBehaviour
{
    [SerializeField] private Color[] colors = new Color[] { };

    private Texture2D backgroundTexture;

    void Awake()
    {
        if (colors.Length == 0) return;

        RawImage image = GetComponent<RawImage>();

        backgroundTexture = new Texture2D(1, 2);
        backgroundTexture.wrapMode = TextureWrapMode.Clamp;
        backgroundTexture.filterMode = FilterMode.Bilinear;
        backgroundTexture.SetPixels(colors);
        backgroundTexture.Apply();

        image.texture = backgroundTexture;
    }
}

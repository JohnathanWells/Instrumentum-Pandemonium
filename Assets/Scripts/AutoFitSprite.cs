using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFitSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite referenceSettings;

    private void Awake()
    {
        Resize();
    }

    public void ApplyDefaultSettings()
    {

    }

    public void Resize()
    {
        float ppU = referenceSettings.pixelsPerUnit;

        float longestSide = spriteRenderer.sprite.rect.width;

        if (longestSide < spriteRenderer.sprite.rect.height)
        {
            longestSide = spriteRenderer.sprite.rect.height;
        }

        spriteRenderer.transform.localScale = Vector3.one * (ppU / longestSide);
    }

    public void LoadImage()
    {

    }
}

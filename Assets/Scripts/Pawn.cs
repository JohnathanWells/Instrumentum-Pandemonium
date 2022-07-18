using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite referenceSettings;

    public void Initialize(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer spriteRenderer;

    [Header("Flash")]
    public Color hitColor = Color.red;
    public float flashDuration = 0.1f;

    private Color originalColor;
    private float flashTimer;
    private bool isFlashing;

    void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    void Update()
    {
        if (!isFlashing || spriteRenderer == null)
            return;

        flashTimer -= Time.deltaTime;

        if (flashTimer <= 0f)
        {
            spriteRenderer.color = originalColor;
            isFlashing = false;
        }
    }

    public void Flash()
    {
        if (spriteRenderer == null)
            return;

        spriteRenderer.color = hitColor;
        flashTimer = flashDuration;
        isFlashing = true;
    }
}

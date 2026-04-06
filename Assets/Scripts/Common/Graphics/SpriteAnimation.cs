using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpriteAnimation : MonoBehaviour
{
    public List<Sprite> Sprites;
    public float Timer;

    public SpriteRenderer spriteRenderer;
    public Image image;

    float _currentTimer;
    int _currentSpriteIndex;

    void Start()
    {
        SetSprite(Sprites[0]);
    }

    void Update()
    {
        if (Sprites.Count == 0) return;

        _currentTimer += Time.deltaTime;

        if (_currentTimer >= Timer)
        {
            _currentTimer = 0f;

            _currentSpriteIndex = (_currentSpriteIndex + 1) % Sprites.Count;

            SetSprite(Sprites[_currentSpriteIndex]);
        }
    }

    void SetSprite(Sprite sprite)
    {
        if (spriteRenderer != null)
            spriteRenderer.sprite = sprite;

        if (image != null)
            image.sprite = sprite;
    }
}
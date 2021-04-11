using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberry : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;

    int currentSprite;
    int growthPace = 3;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSprite = 0;
        //spriteRenderer.sprite = sprites[0];
        StartCoroutine(Grow());
    }

    private IEnumerator Grow()
    {
        while (currentSprite < sprites.Length)
        {           
            spriteRenderer.sprite = sprites[currentSprite];
            currentSprite++;

            yield return new WaitForSeconds(growthPace);
        }               
    }
}
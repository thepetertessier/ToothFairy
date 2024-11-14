using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public void SetColor(Color color) {
        spriteRenderer.color = color;
    }
}

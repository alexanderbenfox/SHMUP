using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    public Image displayImage;
    private Vector2Int _location;

    public void Init(Sprite displaySprite, Vector2Int showLocation)
    {
        displayImage.sprite = displaySprite;
        _location = showLocation;
    }
}

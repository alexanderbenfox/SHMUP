using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerZone : MonoBehaviour
{
    private BoxCollider2D _box;
    public void Init()
    {
        _box = this.GetComponent<BoxCollider2D>();
        _box.size = new Vector2(GameManager.GM.screenToWorldWidth / 2, GameManager.GM.screenToWorldHeight);
    }
}

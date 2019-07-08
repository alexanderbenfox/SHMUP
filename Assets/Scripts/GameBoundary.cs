using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoundary : ICollidingEntity
{
    public void Init()
    {
        _collider = this.GetComponent<BoxCollider2D>();
        _collider.size = new Vector2(GameManager.GM.screenToWorldWidth, GameManager.GM.screenToWorldHeight);
    }
}

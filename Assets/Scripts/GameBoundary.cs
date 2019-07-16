using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoundary : ICollidingEntity
{
    public Vector2 Center
    {
        get { return _center; }
    }

    public float Height
    {
        get { return _height; }
    }

    public float Width
    {
        get { return _width; }
    }

    private Vector2 _center;
    private float _height;
    private float _width;

    public void Init()
    {
        _collider = this.GetComponent<BoxCollider2D>();
        _collider.size = new Vector2(GameManager.GM.screenToWorldWidth, GameManager.GM.screenToWorldHeight);

        _center = this.transform.position;
        _height = GameManager.GM.screenToWorldHeight;
        _width = GameManager.GM.screenToWorldWidth;
    }

    public Vector2 GetRandomPoint()
    {
        float x = Random.Range(Center.x - Width / 2, Center.x + Width / 2);
        float y = Random.Range(Center.y - Height / 2, Center.y + Height / 2);
        return new Vector2(x, y);
    }
}

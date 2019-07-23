using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerZone : MonoBehaviour
{
    private BoxCollider2D _box;
    private float _width;
    private float _height;

    public void Init()
    {
        _box = this.GetComponent<BoxCollider2D>();
        _box.size = new Vector2(GameManager.GM.screenToWorldWidth / 2, GameManager.GM.screenToWorldHeight);
    }

    public Vector2 GetRandomPoint()
    {
        float x = Random.Range(transform.position.x -_width / 2, transform.position.x + _width / 2);
        float y = Random.Range(transform.position.y - _height / 2, transform.position.y + _height / 2);
        return new Vector2(x, y);
    }
}

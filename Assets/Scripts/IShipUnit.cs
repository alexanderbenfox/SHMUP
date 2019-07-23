using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IShipUnit : ICollidingEntity
{
    protected Ship _owner;

    public virtual void Init(Ship ship)
    {
        _collider = this.GetComponent<BoxCollider2D>();
        _owner = ship;
        GameManager.GM.AddEntityToCollisionSystem(this);
    }

    public override void OnCollide(ICollidingEntity entity)
    {
        if (entity.type == "Player")
        {
            Ship ship = (Ship)entity;
            if (ship)
            {}
        }
        //GameManager.GM.DestroyObject(this);
    }

    public virtual void OnUpdate(float dt)
    {}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : ICollidingEntity
{
    public void Init()
    {
        _collider = this.GetComponent<BoxCollider2D>();
        nonPhysics = true;
        isTrigger = true;
        GameManager.GM.AddEntityToCollisionSystem(this);
        type = "PowerUp";
    }

    public override void OnCollide(ICollidingEntity entity)
    {
        if(entity.type == "Player")
        {
            Ship ship = (Ship)entity;
            if(ship)
            {
                OnPlayer(ship);
            }
        }
        GameManager.GM.DestroyObject(this);

    }

    protected virtual void OnPlayer(Ship ship)
    {
        ship.ChangeWeapon(WeaponType.Wave);
    }
}

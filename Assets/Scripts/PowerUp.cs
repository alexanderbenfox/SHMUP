using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    BlasterWeapon,
    WaveWeapon,
    Reflector
}

public class PowerUp : ICollidingEntity
{
    private PowerUpType _type;

    public virtual void Init(PowerUpType pType)
    {
        _type = pType;
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
        if (_type == PowerUpType.BlasterWeapon)
            BlasterWeapon(ship);
        else if (_type == PowerUpType.WaveWeapon)
            WaveWeapon(ship);
        else if (_type == PowerUpType.Reflector)
            CreateReflector(ship);
    }

    protected void BlasterWeapon(Ship ship)
    {
        ship.ChangeWeapon(WeaponType.Blaster);
    }

    protected void WaveWeapon(Ship ship)
    {
        ship.ChangeWeapon(WeaponType.Wave);
    }

    protected void CreateReflector(Ship ship)
    {
        ship.CreateReflector();
    }
}

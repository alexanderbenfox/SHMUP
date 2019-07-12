using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class IBullet : ICollidingEntity
{
    public bool active = false;
    protected Vector2 _velocity;

    protected delegate Vector3 MovementFunc(Vector2 vel, float lifetime, float dt);
    protected MovementFunc func;
    protected float _bulletLifetime;

    private Ship _owner;

    public void Create(Ship owner)
    {
        _collider = this.GetComponent<BoxCollider2D>();
        _owner = owner;
        GameManager.GM.AddEntityToCollisionSystem(this);
    }

    public virtual void Init(Vector2 initialSpeed)
    {
        _velocity = initialSpeed;
        active = true;
        //linear movement function
        func = delegate (Vector2 vel, float lifetime, float dt)
        {
            return vel * dt;
        };
    }

    public virtual void Loop(float dt)
    {
        _dp = _velocity * dt;//func(_velocity, _bulletLifetime, dt);
        _bulletLifetime += dt;
    }

    public virtual void OnDestruction()
    {
        _velocity = Vector2.zero;
        active = false;
    }

    public override void OnCollide(ICollidingEntity entity)
    {
        if (active)
        {
            if (entity.type != "Player" && !entity.nonPhysics)
                OnDestruction();
            else if(entity.type == "Player")
            {
                if (_owner != entity)
                {
                    Ship reciever = (Ship)entity;
                    reciever.TakeDamage(1);
                    OnDestruction();
                }
            }
        }
    }
}

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

    public void Create()
    {
        GameManager.GM.AddEntityToCollisionSystem(this);
    }

    public virtual void Init(Vector2 initialSpeed)
    {
        active = true;
        _velocity = initialSpeed;
        _collider = this.GetComponent<BoxCollider2D>();
        isTrigger = false;
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
        if(entity.type != "Player")
            OnDestruction();
    }
}

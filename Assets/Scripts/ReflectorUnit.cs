using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectorUnit : IShipUnit
{
    public float speed;
    public float lifetime;
    public float moveTime;
    private Vector2 _direction;

    private HashSet<IBullet> bulletsReflected;
    IBullet bulletCollided;

    public override void Init(Ship ship)
    {
        type = "Destroy";
        bulletsReflected = new HashSet<IBullet>();
        lifetime = 0;
        isTrigger = true;
        nonPhysics = true;
        _direction = Random.Range(0, 2) == 0 ? Vector2.up : -Vector2.up;
        base.Init(ship);
    }

    public override void OnCollide(ICollidingEntity entity)
    {
        if(entity.type == "Bullet")
        {
            IBullet bullet = (IBullet)entity;
            if(bullet)
            {
                if(bullet.OwnedBy != _owner)
                {
                    if (!bulletsReflected.Contains(bullet))
                    {
                        _owner.bulletPool.Create(transform.position, _owner.shootDirection);
                        bulletsReflected.Add(bullet);
                        bullet.OnDestruction();
                    }
                    bulletCollided = bullet;
                }
            }
        }

        

        base.OnCollide(entity);
    }

    public override void OnUpdate(float dt)
    {
        lifetime += dt;
        if(lifetime >= moveTime)
        {
            _direction *= -1;
            lifetime = 0;
        }
        _dp = _direction * (speed * dt);

        if (bulletCollided == null)
            bulletsReflected.Clear();
        bulletCollided = null;
    }
}

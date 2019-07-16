using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Blaster, Wave
}

public class IWeapon
{
    protected BulletPool _bulletPool;

    protected IEnumerator _cooldown;
    protected bool _weaponOnCooldown;

    protected Ship _owner;

    public IWeapon(Ship owner, BulletPool pool)
    {
        _owner = owner;
        _bulletPool = pool;
        _weaponOnCooldown = false;
    }

    protected IEnumerator Cooldown(float cooldownTime)
    {
        _weaponOnCooldown = true;
        float time = 0;

        while(time < cooldownTime)
        {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

        _weaponOnCooldown = false;
    }

    public virtual void Shoot(Vector2 location, Vector2 direction) { }

    public virtual IEnumerator WeaponAfterShotRoutine() { yield return null; }
}

public class BlasterWeapon : IWeapon
{
    public BlasterWeapon(Ship owner, BulletPool pool) : base(owner, pool) { }

    public override void Shoot(Vector2 location, Vector2 direction)
    {
        if (!_weaponOnCooldown)
        {
            _bulletPool.Create(location, direction);
            _weaponOnCooldown = true;
            _owner.PerformWeaponCoroutine(WeaponAfterShotRoutine());
        }
    }

    public override IEnumerator WeaponAfterShotRoutine()
    {
        yield return Cooldown(GameManager.GM.WeaponCooldown);
    }
}

public class WaveWeapon : IWeapon
{
    class WaveBullet : IBullet
    {
        public override void Init(Vector2 initialSpeed)
        {
            _velocity = initialSpeed;
            active = true;
            //linear movement function
            _func = delegate (Vector2 vel, float lifetime, float dt)
            {
                return new Vector2(vel.x, Mathf.Sin(lifetime) + vel.y) * dt;
            };
        }
    }

    public WaveWeapon(Ship owner, BulletPool pool) : base(owner, pool) { }

    public override void Shoot(Vector2 location, Vector2 direction)
    {
        if (!_weaponOnCooldown)
        {
            var bullet = _bulletPool.Create(location, direction);
            //bullet = new WaveBullet();
            //bullet.Init(direction * GameManager.GM.BlastBulletSpeed);
            bullet.SetMovementFunction(
            delegate (Vector2 vel, float lifetime, float dt)
            {
                return new Vector2(vel.x, GameManager.GM.WaveBulletAmplitude * Mathf.Sin(GameManager.GM.WaveBulletFrequency * lifetime) + vel.y) * dt;
            });

            _weaponOnCooldown = true;
            _owner.PerformWeaponCoroutine(WeaponAfterShotRoutine());
        }
    }

    public override IEnumerator WeaponAfterShotRoutine()
    {
        yield return Cooldown(GameManager.GM.WeaponCooldown);
    }
}


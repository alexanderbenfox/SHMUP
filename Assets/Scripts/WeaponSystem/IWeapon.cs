using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IWeapon
{
    protected BulletPool _bulletPool;

    protected IEnumerator _cooldown;
    protected bool _weaponOnCooldown;

    public IWeapon(ref BulletPool pool)
    {
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
    public BlasterWeapon(ref BulletPool pool) : base(ref pool) { }

    public override void Shoot(Vector2 location, Vector2 direction)
    {
        if (!_weaponOnCooldown)
        {
            _bulletPool.Create(location, direction, GameManager.GM.BlasterBulletPrefab);
            _weaponOnCooldown = true;
            GameManager.GM.PerformWeaponCoroutine(WeaponAfterShotRoutine());
        }
    }

    public override IEnumerator WeaponAfterShotRoutine()
    {
        yield return Cooldown(GameManager.GM.WeaponCooldown);
    }
}

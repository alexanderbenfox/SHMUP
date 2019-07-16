using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    //======BULLET PREFABS ======//
    public IBullet BlasterBulletPrefab;

    private List<IBullet> _bullets;
    private int _latestUsedBullet;

    private Ship _owner;

    public void Init(Ship owner)
    {
        _owner = owner;
        _bullets = new List<IBullet>();
        _latestUsedBullet = 0;
    }

    public IBullet Create(Vector2 spawnLocation, Vector2 direction)
    {
        Vector2 initialSpeed = direction * GameManager.GM.BlastBulletSpeed;

        if (_bullets.Count < GameManager.GM.MaxPerPoolBullets)
        {
            IBullet obj = GameObject.Instantiate<IBullet>(BlasterBulletPrefab, transform) as IBullet;
            obj.Create(_owner);
            _bullets.Add(obj);
            obj.Init(initialSpeed);
        }
        _bullets[_latestUsedBullet].OnDestruction();

        _bullets[_latestUsedBullet].transform.position = spawnLocation;
        _bullets[_latestUsedBullet].Init(initialSpeed);

        int lastBullet = _latestUsedBullet;
        _latestUsedBullet = (_latestUsedBullet + 1) % GameManager.GM.MaxPerPoolBullets;

        return _bullets[lastBullet];
    }

    public void UpdateBullets(float dt)
    {
        foreach(IBullet bullet in _bullets)
        {
            if (bullet.active)
                bullet.Loop(dt);
        }
    }


}

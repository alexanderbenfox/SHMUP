using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    private List<IBullet> _bullets;
    private int _latestUsedBullet;

    public void Init()
    {
        _bullets = new List<IBullet>();
        _latestUsedBullet = 0;
    }

    public void Create(Vector2 spawnLocation, Vector2 direction, IBullet bulletPrefab)
    {
        Vector2 initialSpeed = direction * GameManager.GM.BlastBulletSpeed;

        if (_bullets.Count < GameManager.GM.MaxPerPoolBullets)
        {
            IBullet obj = GameObject.Instantiate<IBullet>(bulletPrefab) as IBullet;
            _bullets.Add(obj);
            obj.Init(initialSpeed);
        }
        _bullets[_latestUsedBullet].OnDestruction();

        _bullets[_latestUsedBullet].transform.position = spawnLocation;
        _bullets[_latestUsedBullet].Init(initialSpeed);

        _latestUsedBullet = (_latestUsedBullet + 1) % GameManager.GM.MaxPerPoolBullets;
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

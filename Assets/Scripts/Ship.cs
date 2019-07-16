using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAction
{
    SHOOT, NONE
}

struct PlayerInput
{
    public Vector2 movement;
    public PlayerAction action;
}

[RequireComponent(typeof(BoxCollider2D))]
public class Ship : ICollidingEntity
{
    //
    public Vector2 shootDirection;
    public BulletPool bulletPool;
    
    //private variables
    private PlayerInput _lastFrame;
    private IWeapon _weapon;
    private HealthBar _hpBar;

    public PlayerZone zone;

    //player statistics
    int _maxHealth;
    [SerializeField]
    int _health;

    private bool _controlledByAI;
    private float _lifeTime = 0;

    private void ResetStatus()
    {
        _maxHealth = GameManager.GM.PlayerMaxHealth;
        _health = _maxHealth;
    }

    public void Init(bool isAI, HealthBar healthBar)
    {
        _hpBar = healthBar;
        bulletPool.Init(this);

        ChangeWeapon(WeaponType.Blaster);

        _controlledByAI = isAI;
        shootDirection = Vector2.right;
        if (isAI)
        {
            shootDirection = Vector2.left;
        }

        _collider = this.GetComponent<BoxCollider2D>();
        zone.Init();
        
        type = "Player";

        ResetStatus();
    }

    public void Loop(float dt)
    {
        PlayerInput thisFrame = _controlledByAI ? AIInput() : ProcessInput();

        if(thisFrame.action == PlayerAction.SHOOT)
        {
            //Vector2 shotDirection = new Vector2(shootDirection.x, Vector2.Dot(thisFrame.movement, Vector2.up));
            _weapon.Shoot(transform.position, shootDirection.normalized);
        }

        bulletPool.UpdateBullets(dt);

        _dp = thisFrame.movement * dt;
        _lifeTime += dt;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        _hpBar.ChangeValue((float)_health / (float)_maxHealth);
    }

    public void ChangeWeapon(WeaponType weapon)
    {
        if (weapon == WeaponType.Blaster)
            _weapon = new BlasterWeapon(this, bulletPool);
        else if (weapon == WeaponType.Wave)
            _weapon = new WaveWeapon(this, bulletPool);
    }

    //-------------------------------------------//

    private PlayerInput ProcessInput()
    {
        float horiz = Input.GetAxis("Horizontal") > 0.25f ? GameManager.GM.PlayerSpeedMultiplier : Input.GetAxis("Horizontal") < -0.25f ? -GameManager.GM.PlayerSpeedMultiplier : 0;
        float vert = Input.GetAxis("Vertical") > 0.25f ? GameManager.GM.PlayerSpeedMultiplier : Input.GetAxis("Vertical") < -0.25f ? -GameManager.GM.PlayerSpeedMultiplier : 0;

        PlayerInput input;
        input.movement = new Vector2(horiz, vert);

        if (Input.GetKey(GameManager.GM.playerShootKey))
            input.action = PlayerAction.SHOOT;
        else
            input.action = PlayerAction.NONE;

        return input;
    }

    private PlayerInput AIInput()
    {
        float vert = Mathf.Sin(_lifeTime) > 0.25f ? GameManager.GM.PlayerSpeedMultiplier : Mathf.Sin(_lifeTime) < -0.25f ? -GameManager.GM.PlayerSpeedMultiplier : 0;

        PlayerInput input;
        input.movement = new Vector2(0, vert);

        if (Random.Range(0.0f, 1.0f) > .9)
            input.action = PlayerAction.SHOOT;
        else
            input.action = PlayerAction.NONE;

        return input;
    }

    public override Vector2 GetNextFramePosition()
    {
        return base.GetNextFramePosition();
    }

    public override void OnCollide(ICollidingEntity entity)
    {
        base.OnCollide(entity);
    }

    public override void FinalizeFrame(float dt)
    {
        base.FinalizeFrame(dt);
    }

    private Coroutine _weaponCoroutine;
    public void PerformWeaponCoroutine(IEnumerator enumerator)
    {
        if (_weaponCoroutine != null)
            StopCoroutine(_weaponCoroutine);
        _weaponCoroutine = StartCoroutine(enumerator);
    }
}

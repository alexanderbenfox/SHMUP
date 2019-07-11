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
    
    //private variables
    private PlayerInput _lastFrame;
    private IWeapon _weapon;

    public PlayerZone zone;

    //player statistics
    int _maxHealth;
    int _health;

    private void ResetStatus()
    {
        _maxHealth = GameManager.GM.PlayerMaxHealth;
        _health = _maxHealth;
        _weapon = new BlasterWeapon(ref GameManager.GM.pool);
    }

    public void Init()
    {
        _collider = this.GetComponent<BoxCollider2D>();
        zone.Init();
        shootDirection = Vector2.right;
        isTrigger = false;
        type = "Player";

        ResetStatus();
    }

    public void Loop(float dt)
    {
        PlayerInput thisFrame = ProcessInput();

        if(thisFrame.action == PlayerAction.SHOOT)
        {
            //Vector2 shotDirection = new Vector2(shootDirection.x, Vector2.Dot(thisFrame.movement, Vector2.up));
            _weapon.Shoot(transform.position, shootDirection.normalized);
        }

        _dp = thisFrame.movement * dt;
    }

    //-------------------------------------------//

    private PlayerInput ProcessInput()
    {
        float horiz = Input.GetAxis("Horizontal") > 0 ? GameManager.GM.PlayerSpeedMultiplier : Input.GetAxis("Horizontal") < 0 ? -GameManager.GM.PlayerSpeedMultiplier : 0;
        float vert = Input.GetAxis("Vertical") > 0 ? GameManager.GM.PlayerSpeedMultiplier : Input.GetAxis("Vertical") < 0 ? -GameManager.GM.PlayerSpeedMultiplier : 0;

        PlayerInput input;
        input.movement = new Vector2(horiz, vert);

        if (Input.GetKey(GameManager.GM.playerShootKey))
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
}

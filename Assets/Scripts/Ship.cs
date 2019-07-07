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
public class Ship : MonoBehaviour
{
    //
    public Vector2 shootDirection;
    
    //private variables
    private BoxCollider2D _hurtbox;
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
        _hurtbox = this.GetComponent<BoxCollider2D>();
        zone.Init();
        shootDirection = Vector2.right;

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
        transform.Translate(thisFrame.movement * dt);
    }

    //-------------------------------------------//

    private PlayerInput ProcessInput()
    {
        PlayerInput input;
        input.movement = new Vector2(
            GameManager.GM.PlayerSpeedMultiplier * Input.GetAxis("Horizontal"),
            GameManager.GM.PlayerSpeedMultiplier * Input.GetAxis("Vertical"));

        if (Input.GetKey(GameManager.GM.playerShootKey))
            input.action = PlayerAction.SHOOT;
        else
            input.action = PlayerAction.NONE;

        return input;
    }
}

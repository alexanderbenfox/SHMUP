using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class IBullet : ICollidingEntity
{
    public bool active = false;
    protected Vector2 _velocity;

    public delegate Vector3 MovementFunc(Vector2 vel, float lifetime, float dt);
    protected MovementFunc _func;
    protected float _bulletLifetime;
    public Animator anim;


    private Ship _owner;
    public Ship OwnedBy { get { return _owner; } }

    private SpriteRenderer _spriteRenderer;
    private Sprite _originalSprite;

    Coroutine destructionCoroutine;


    public void Create(Ship owner)
    {
        type = "Bullet";
        _collider = this.GetComponent<BoxCollider2D>();
        _owner = owner;

        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _originalSprite = _spriteRenderer.sprite;

        GameManager.GM.AddEntityToCollisionSystem(this);
    }

    public void SetMovementFunction(MovementFunc func)
    {
        _func = func; 
    }

    public virtual void Init(Vector2 initialSpeed)
    {
        if (destructionCoroutine != null)
            StopCoroutine(destructionCoroutine);

        _spriteRenderer.enabled = true;
        anim.enabled = false;
        _spriteRenderer.sprite = _originalSprite;

        _velocity = initialSpeed;
        active = true;
        //linear movement function
        _func = delegate (Vector2 vel, float lifetime, float dt)
        {
            return vel * dt;
        };
    }

    public virtual void Loop(float dt)
    {
        _dp = _func(_velocity, _bulletLifetime, dt);
        _bulletLifetime += dt;
    }

    

    public virtual void OnDestruction()
    {
        _velocity = Vector2.zero;
        active = false;
        anim.enabled = true;
        destructionCoroutine = StartCoroutine(PlayDestructionAnim());
    }

    private IEnumerator PlayDestructionAnim()
    {
        anim.Play("RedExplosion");
        yield return new WaitForEndOfFrame();

        float time = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(time);

        destructionCoroutine = null;
        _spriteRenderer.enabled = false;
    }

    public override void OnCollide(ICollidingEntity entity)
    {
        if (active)
        {
            if (entity.type == "Destroy")
                OnDestruction();
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

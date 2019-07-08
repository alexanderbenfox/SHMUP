using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    //========================
    public float screenToWorldHeight;
    public float screenToWorldWidth;
    public int NumberOfPlayers;
    public int PlayerMaxHealth;
    public float PlayerSpeedMultiplier;
    public KeyCode playerShootKey;

    //====== Player creation ====//
    private List<Ship> _players;
    public GameObject shipPrefab;

    //======= Physics =====//
    private CollisionSystem _collisionSystem;

    //===== WEAPONS ======//
    public BulletPool pool;
    public int MaxPerPoolBullets;
    public float BlastBulletSpeed;

    public float WeaponCooldown;

    //======BULLET PREFABS ======//
    public IBullet BlasterBulletPrefab;
    

    //===== GENERAL? ===========///
    public Transform worldSpaceContainer;
    public GameBoundary boundary;

    private Coroutine _weaponCoroutine;

    private bool _gameShouldUpdate;
    private Coroutine _gameRoutine;

    public static GameManager GM
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        screenToWorldHeight = Camera.main.orthographicSize * 2.0f;
        screenToWorldWidth = screenToWorldHeight * Screen.width / Screen.height;

        _players = new List<Ship>();
        _gameShouldUpdate = true;

        pool.Init();
        boundary.Init();

        for (int i = 0; i < NumberOfPlayers; i++)
        {
            float spawnX = -screenToWorldWidth / 4 + (screenToWorldWidth / 2 * i);
            float spawnY = 0;

            //create and initialize the new ship objects
            GameObject obj = GameObject.Instantiate<GameObject>(shipPrefab, worldSpaceContainer);
            obj.transform.localPosition = new Vector2(spawnX, spawnY);

            Ship newShip = obj.GetComponentInChildren<Ship>();
            newShip.Init();

            _players.Add(newShip);
        }


        _collisionSystem = GetComponent<CollisionSystem>();
        _collisionSystem.Init();

        _gameRoutine = StartCoroutine(GameRoutine());
    }

    public void AddEntityToCollisionSystem(ICollidingEntity entity)
    {
        _collisionSystem.AddEntity(ref entity);
    }

    public void PerformWeaponCoroutine(IEnumerator enumerator)
    {
        if (_weaponCoroutine != null)
            StopCoroutine(_weaponCoroutine);
        _weaponCoroutine = StartCoroutine(enumerator);
    }

    private IEnumerator GameRoutine()
    {
        while (_gameShouldUpdate)
        {
            Loop(Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private void Loop(float dt)
    {
        pool.UpdateBullets(dt);

        foreach(Ship player in _players)
        {
            player.Loop(dt);
        }

        foreach (Ship player in _players)
        {
            //player.Resolve(dt);
        }
        _collisionSystem.CheckFrame();
    }
}

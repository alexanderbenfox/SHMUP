using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static string p1Color = "#FF6D00";


    private static GameManager _instance;
    public bool initialized = false;

    //========================
    public float screenToWorldHeight;
    public float screenToWorldWidth;
    public int NumberOfPlayers;
    public int PlayerMaxHealth;
    public float PlayerSpeedMultiplier;
    public KeyCode playerShootKey;

    //====== Player creation ====//
    public HealthBar leftBar, rightBar;

    private List<Ship> _players;
    public GameObject leftShipPrefab;
    public GameObject rightShipPrefab;

    //======= Physics =====//
    private CollisionSystem _collisionSystem;

    //===== WEAPONS ======//
    public WeaponType startingWeapon;
    public PowerUp powerUpPrefab;
    public int MaxPerPoolBullets;
    public float BlastBulletSpeed;
    public float WeaponCooldown;

    public float WaveBulletFrequency;
    public float WaveBulletAmplitude;

    //===== GENERAL? ===========///
    public Transform worldSpaceContainer;
    public GameBoundary boundary;
    public GameTimer timer;
    public float MaxRoundTime;

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

    public Vector2 GetRandomPoint(float height, float width)
    {
        float x = Random.Range(-width / 2, width / 2);
        float y = Random.Range(-height / 2, height / 2);
        return new Vector2(x, y);
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

        //boundary.Init();
        var boundaries = GameObject.FindObjectsOfType<GameBoundary>();
        foreach (var bound in boundaries)
        {
            bound.Init();
        }

        for (int i = 0; i < NumberOfPlayers; i++)
        {
            float spawnX = -screenToWorldWidth / 4 + (screenToWorldWidth / 2 * i);
            float spawnY = 0;

            //create and initialize the new ship objects
            GameObject obj = GameObject.Instantiate<GameObject>(i == 0 ? leftShipPrefab : rightShipPrefab, worldSpaceContainer);
            obj.transform.localPosition = new Vector2(spawnX, spawnY);

            Ship newShip = obj.GetComponentInChildren<Ship>();
            //right now only the first spawned ship is player controlled
            newShip.Init(i != 0, i == 0 ? leftBar : rightBar);

            newShip.ChangeWeapon(startingWeapon);
            newShip.color = (PlayerColor)i;

            _players.Add(newShip);
        }


        _collisionSystem = GetComponent<CollisionSystem>();
        _collisionSystem.Init();

        timer.Init();

        //_gameRoutine = StartCoroutine(GameRoutine());
        initialized = true;
    }

    public void AddEntityToCollisionSystem(ICollidingEntity entity)
    {
        _collisionSystem.AddEntity(ref entity);
    }

    public void DestroyObject(ICollidingEntity entity, bool destroy = true)
    {
        if (destroy)
            _collisionSystem.DestroyEntity(ref entity);
        else
            _collisionSystem.RemoveEntity(ref entity);
    }

    public void SpawnRandomPowerUp()
    {
        var pt = boundary.GetRandomPoint();
        PowerUp powerUp = GameObject.Instantiate<PowerUp>(powerUpPrefab) as PowerUp;
        powerUp.transform.position = pt;
        var p = (PowerUpType)Random.Range(0, 3);
        //var p = PowerUpType.Reflector;
        powerUp.Init(p);
    }

    private IEnumerator GameRoutine()
    {
        while (_gameShouldUpdate)
        {
            Loop(Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private void Update()
    {
        if (initialized)
            Loop(Time.deltaTime);
    }

    private void Loop(float dt)
    {
        foreach(Ship player in _players)
        {
            player.Loop(dt);
        }

        foreach (Ship player in _players)
        {
            //player.Resolve(dt);
        }
        timer.OnUpdate(dt);
        _collisionSystem.CheckFrame(dt);
    }
}

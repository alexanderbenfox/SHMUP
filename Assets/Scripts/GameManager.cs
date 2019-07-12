using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    public int MaxPerPoolBullets;
    public float BlastBulletSpeed;

    public float WeaponCooldown;
    

    //===== GENERAL? ===========///
    public Transform worldSpaceContainer;
    public GameBoundary boundary;

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

            _players.Add(newShip);
        }


        _collisionSystem = GetComponent<CollisionSystem>();
        _collisionSystem.Init();

        //_gameRoutine = StartCoroutine(GameRoutine());
        initialized = true;
    }

    public void AddEntityToCollisionSystem(ICollidingEntity entity)
    {
        _collisionSystem.AddEntity(ref entity);
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
        _collisionSystem.CheckFrame(dt);
    }
}

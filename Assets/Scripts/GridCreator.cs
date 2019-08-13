using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ShipOption
{
    public string shipName;
    public Sprite shipSprite;
    public Vector2Int shipLocation;
}

public class GridCreator : MonoBehaviour
{
    const int buttonSize = 60;

    public Sprite lockSprite;
    public GridSquare shipOptionPrefab;
    public ShipOption[] available;
    public CharacterSelector csPrefab;
    public Transform panel;

    private Grid _grid;
    private CharacterSelector _p1Selector, _p2Selector;


    class Grid
    {
        public int x
        { get { return _x; } }

        public int y
        { get { return _y; } }

        private int _x = 0;
        private int _y = 0;
        private List<List<GridSquare>> _grid;
        private GridSquare _gsPrefab;
        private Vector2 _initialSpawnPt;
        private Transform _canvas;

        public Grid(GridSquare gsPrefab, Vector2 spawnPoint, Transform canvas)
        {
            _x = 0;
            _y = 0;

            _canvas = canvas;
            _gsPrefab = gsPrefab;
            _grid = new List<List<GridSquare>>();
            _initialSpawnPt = spawnPoint;
        }

        public void FillEmpty(int x, int y, Sprite emptySprite)
        {
            if(_y < y)
            {
                for(int i = _y; i < y; i++)
                {
                    _grid.Add(new List<GridSquare>());
                    for(int j = 0; j < _x; j++)
                    {
                        _grid[i].Add(CreateGridSq(new Vector2Int(j, i), emptySprite));
                    }
                }
                _y = y;
            }

            if(_x < x)
            {
                for (int i = 0; i < _y; i++)
                {
                    for (int j = _x; j < x; j++)
                    {
                        _grid[i].Add(CreateGridSq(new Vector2Int(j, i), emptySprite));
                    }
                }
                _x = x;
            }
        }

        public Vector2 GetGridLocation(int x, int y)
        {
            return _grid[y][x].transform.position;
        }

        public GridSquare GetSquare(Vector2Int location)
        {
            return _grid[location.y][location.x];
        }

        public void ChangeDisplaySprite(int x, int y, Sprite sprite)
        {
            Debug.Log(x);
            Debug.Log(y);
            if(_grid[y][x] != null)
            {
                _grid[y][x].displayImage.sprite = sprite;
            }
        }

        private GridSquare CreateGridSq(Vector2Int location, Sprite sprite)
        {
            var empty = GameObject.Instantiate<GridSquare>(_gsPrefab, _canvas);

            float spawnX = _initialSpawnPt.x + (float)location.x;
            float spawnY = _initialSpawnPt.y - (float)location.y;

            empty.transform.position = new Vector2(spawnX, spawnY);

            empty.Init(sprite, location);

            return empty;
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        _grid = new Grid(shipOptionPrefab, this.transform.position, this.transform);
        foreach(ShipOption option in available)
        {
            if((option.shipLocation.x + 1) > _grid.x || (option.shipLocation.y + 1) > _grid.y)
            {
                _grid.FillEmpty(option.shipLocation.x + 1, option.shipLocation.y + 1, lockSprite);
            }
            _grid.ChangeDisplaySprite(option.shipLocation.x, option.shipLocation.y, option.shipSprite);
        }

        _p1Selector = GameObject.Instantiate<CharacterSelector>(csPrefab, panel);
        _p2Selector = GameObject.Instantiate<CharacterSelector>(csPrefab, panel);

        if(available.Length >= 2)
        {
            _p1Selector.Init(_grid.GetSquare(available[0].shipLocation), panel);
            _p2Selector.Init(_grid.GetSquare(available[1].shipLocation), panel);
        }
    }
    
}

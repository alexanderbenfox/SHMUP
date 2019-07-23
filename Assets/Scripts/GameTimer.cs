using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text timer;
    public int spawnFrequency;

    private float _maxTime;
    private float _currentTime;
    private int _lastNumeral;

    public void Init()
    {
        _maxTime = GameManager.GM.MaxRoundTime;
        _currentTime = _maxTime;
        _lastNumeral = Mathf.FloorToInt(_currentTime);

        timer.text = _lastNumeral.ToString();
    }

    public void OnUpdate(float dt)
    {
        _currentTime -= dt;
        if(Mathf.FloorToInt(_currentTime) != _lastNumeral)
        {
            _lastNumeral = Mathf.FloorToInt(_currentTime);
            timer.text = _lastNumeral.ToString();

            //
            if (_lastNumeral % spawnFrequency == 0)
                GameManager.GM.SpawnRandomPowerUp();
        }
    }

}

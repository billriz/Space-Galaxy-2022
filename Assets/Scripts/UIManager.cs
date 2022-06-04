﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _ammoCountText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartLevelText;
    [SerializeField]
    private Text _wavesText;
    [SerializeField]
    private Text _homingLaserCountText;

    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Slider _thrusterSlider;


    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        
        if (_gameManager == null)
        {

            Debug.Log("Game Manager is NULL");
        }

    }

    

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score.ToString();

    }

    public void UpdateLives(int currentLives)
    {

        _livesImg.sprite = _livesSprites[currentLives];

        if (currentLives < 1)
        {
            GameOverSequence();            
        }
    }

    public void UpdateAmmoCount(int Ammo, int Max)
    {

        _ammoCountText.text = "Laser: " + Ammo.ToString() + " / " + Max.ToString();

    }

    public void UpdateHomingLaserCount(int Homing, int Max)
    {

        _homingLaserCountText.text = "Homing Laser: " + Homing + " / " + Max;

    }

    public void UpdateThrusterCharge(float thrusterCharge)
    {

        _thrusterSlider.value = thrusterCharge;
    }

    public void UpdateWaves(string WaveName)
    {

        _wavesText.text = "Current Wave: " + WaveName;
    }
    

    void GameOverSequence()
    {

        _gameOverText.enabled = true;
        _restartLevelText.enabled = true;
        StartCoroutine(GameOverFlickerRoutine());
        _gameManager.GameOver();

    }


    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            _gameOverText.enabled = false;
            yield return new WaitForSeconds(.5f);
            _gameOverText.enabled = true;

        }

    }

    
}

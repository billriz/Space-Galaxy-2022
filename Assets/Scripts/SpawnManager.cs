using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

      
    public enum SpawningState {SpawningEnemies, CountingEnemies, GameOver};
    
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private float _spawnRate = 5f;
   
    [SerializeField]
    private EnemyWaves[] _enemyWaves;

    private int _currentWave = 0;

    private UIManager _uiManager;

    [System.Serializable]
    public struct PowerUps
    {
       public string Name;
       public GameObject PowerUpToSpawn;
       public int SpawnWeight;

    }
    [SerializeField]
    private PowerUps[] powerUps;

    private int _totalPowerUpWeight;
    [SerializeField]
    private SpawningState state = SpawningState.CountingEnemies;


    void Start ()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager on SpawnManager is Null");

        }

        _uiManager.UpdateWaves(_enemyWaves[_currentWave].Name);

        
        foreach (PowerUps PowerUpsData in powerUps)
        {

            _totalPowerUpWeight += PowerUpsData.SpawnWeight;

        }
    }


    IEnumerator SpawnEnemyRoutine()
    {               
        
        while(state == SpawningState.SpawningEnemies)
        {
           
            for (int i = 0; i < _enemyWaves[_currentWave].EnemyCount && state != SpawningState.GameOver ; i++)
            {

                SpawnEnemies();               

                yield return new WaitForSeconds(_enemyWaves[_currentWave].SpawnRate);
            }

           if (state != SpawningState.GameOver)
           {
                GetCurrentWave();
           }  

        }
    }

    private void SpawnEnemies()
    {
        Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7.5f, 0f);
        int RandomEnemy = Random.Range(0, _enemyWaves[_currentWave].EnemyToSpawn.Length);
        GameObject newEnemy = Instantiate(_enemyWaves[_currentWave].EnemyToSpawn[RandomEnemy], posToSpawn, Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;

    }

    private void GetCurrentWave()
    {
        _currentWave++;

        if (_currentWave <= _enemyWaves.Length - 1)
        {
            _uiManager.UpdateWaves(_enemyWaves[_currentWave].Name);
        }

        if (_currentWave == _enemyWaves.Length)
        {
            state = SpawningState.CountingEnemies;
            StartCoroutine(isBossDefeated());
        }

        if (_currentWave == _enemyWaves.Length - 2)        {

            state = SpawningState.CountingEnemies;
            StartCoroutine(CountingEnemiesRoutine());            
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        
        while (state != SpawningState.GameOver)
        {       

            Vector3 PosToSpawn = new Vector3(Random.Range(-9f, 9f), 7.2f, 0f);            
            int _randomWeight = Random.Range(0, _totalPowerUpWeight);
            foreach (PowerUps PowerUpsData in powerUps)
            {
                if (_randomWeight <= PowerUpsData.SpawnWeight)
                {

                    Instantiate(PowerUpsData.PowerUpToSpawn, PosToSpawn, Quaternion.identity);
                    break;
                }
                else
                {
                    _randomWeight -= PowerUpsData.SpawnWeight;
                }

            }
         
            yield return new WaitForSeconds(Random.Range(3, 8));

        }
    }


    public void OnPlayerDeath()
    {
       
        state = SpawningState.GameOver;
    }

    public void OnAstroidDestroyed()
    {
        state = SpawningState.SpawningEnemies;
        _uiManager.GameStartRoutine();
        StartCoroutine(SpawnEnemyRoutine());

        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator CountingEnemiesRoutine()
    {
        while (state == SpawningState.CountingEnemies)
        {
            yield return new WaitForSeconds(.5f);
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {

                state = SpawningState.SpawningEnemies;
                _currentWave++;
                _uiManager.UpdateWaves(_enemyWaves[_currentWave].Name);
                StartCoroutine(SpawnEnemyRoutine());

            }

        }

        yield break;
    }

    IEnumerator isBossDefeated()
    {
        while (state == SpawningState.CountingEnemies)
        {
            yield return new WaitForSeconds(5.0f);
            if (GameObject.FindGameObjectWithTag("Boss") == null)
            {
                state = SpawningState.GameOver;
                _uiManager.UpdateVictory();

            }  
        }

        yield break;
    }
}

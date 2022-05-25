using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

   // [SerializeField]
  //  private GameObject [] _enemyPrefab;

    
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private float _spawnRate = 5f;

    private int _randomPowerUp;


    private bool _stopSpawning = false;
    [SerializeField]
    private EnemyWaves[] _enemyWaves;

    private int _currentWave = 0;

    private UIManager _uiManager;



    void Start ()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager on SpawnManager is Null");

        }

        _uiManager.UpdateWaves(_enemyWaves[_currentWave].Name);

        

    }


    IEnumerator SpawnEnemyRoutine()
    {
               
        while(_stopSpawning == false)
        {
           
            for (int i = 0; i < _enemyWaves[_currentWave].EnemyCount; i++)
            {
                
                Vector3 posToSpawn = new Vector3(Random.Range(-8f,8f), 7.2f, 0f);
                int RandomEnemy = Random.Range(0, _enemyWaves[_currentWave].EnemyToSpawn.Length);               
                GameObject newEnemy = Instantiate(_enemyWaves[_currentWave].EnemyToSpawn[RandomEnemy], posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;

                yield return new WaitForSeconds(_enemyWaves[_currentWave].SpawnRate);
            }

            _currentWave++;            

            if (_currentWave > _enemyWaves.Length - 1)
            {               
                _stopSpawning = true;
                _uiManager.UpdateWaves("Waves Completed");
                break;
            }

            _uiManager.UpdateWaves(_enemyWaves[_currentWave].Name);

        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (_stopSpawning == false)
        {       

            Vector3 PosToSpawn = new Vector3(Random.Range(-8f, 8f), 7.2f, 0f);
            _randomPowerUp = Random.Range(0, 6);
            if (_randomPowerUp == 5 && Random.value >= .85f)
            {
                _randomPowerUp = 5;
                
            }
            else
            {
                _randomPowerUp = Random.Range(0, 5);
            }
                        
            Instantiate(_powerUps[_randomPowerUp], PosToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));

        }
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void OnAstroidDestroyed()
    {
        
        StartCoroutine(SpawnEnemyRoutine());

        StartCoroutine(SpawnPowerUpRoutine());
    }
}

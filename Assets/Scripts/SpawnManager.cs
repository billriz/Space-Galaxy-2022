using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject [] _enemyPrefab;

    
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private float _spawnRate = 5f;

    private int _randomPowerUp;


    private bool _stopSpawning = false;
    
   

    IEnumerator SpawnEnemyRoutine()
    {
               
        while(_stopSpawning == false)
        {
            float RandomX = Random.Range(-8f, 8f);
            int RandomEnemy = Random.Range(0, 3);
            Vector3 posToSpawn = new Vector3(RandomX, 7.2f, 0f);
            GameObject newEnemy = Instantiate(_enemyPrefab[RandomEnemy], posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnRate);
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

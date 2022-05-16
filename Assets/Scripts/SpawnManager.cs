using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private float _spawnRate = 5f;


    private bool _stopSpawning = false;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
               
        while(_stopSpawning == false)
        {
            float RandomX = Random.Range(-8f, 8f);
            Vector3 posToSpawn = new Vector3(RandomX, 7.2f, 0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (_stopSpawning == false)
        {       

            Vector3 PosToSpawn = new Vector3(Random.Range(-8f, 8f), 7.2f, 0f);
            int randomPowerUp = Random.Range(0, 5);
            Instantiate(_powerUps[randomPowerUp], PosToSpawn, Quaternion.identity);
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

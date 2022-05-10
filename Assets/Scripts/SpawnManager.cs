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
    private float _spawnRate = 5f;


    private bool _stopSpawning = false;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
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

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}

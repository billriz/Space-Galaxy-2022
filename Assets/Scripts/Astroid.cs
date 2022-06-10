using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{

    private float _speed = 20.0f;

    [SerializeField]
    private GameObject _explosionPrefab;


    private SpawnManager _spawnManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
           Debug.Log("Spawn Manager is Null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Rotate(Vector3.back * _speed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            StartCoroutine(DestroyAstroidRoutine());

        }
    }

    IEnumerator DestroyAstroidRoutine()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        this.gameObject.SetActive(false);
        _spawnManager.OnAstroidDestroyed();
        Destroy(this.gameObject, 3.0f);
       
    }
}

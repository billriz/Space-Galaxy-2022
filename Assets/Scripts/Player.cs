using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;

    private bool _canFireLaser = true;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private float _fireRate = .5f;


    private SpawnManager _spawnManager;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null");
        }


    }

    // Update is called once per frame
    void Update()
    {

        PlayerMovement();

       
        if (Input.GetKeyDown(KeyCode.Space) && _canFireLaser)
        {

            FireLaser();

        }
       
    }

    void PlayerMovement()
    {
        
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");


        transform.Translate(Vector3.right * HorizontalInput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * VerticalInput * _speed * Time.deltaTime);

        if (transform.position.x > 11.34f)
        {
            transform.position = new Vector3(-11.34f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.34f)
        {
            transform.position = new Vector3(11.34f, transform.position.y, 0);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.9f, 0f), 0);
              
    }

    IEnumerator ReloadTimer()
    {

        yield return new WaitForSeconds(_fireRate);
        _canFireLaser = true;

    }

    void FireLaser()
    {
        Instantiate(_laserPrefab, transform.position + new Vector3(0f, .85f, 0f), Quaternion.identity);
        _canFireLaser = false;
        StartCoroutine(ReloadTimer());

    }

    public void Damage()
    {

        _lives--;

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);

        }
    }
}

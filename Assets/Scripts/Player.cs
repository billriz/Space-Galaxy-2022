using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    private bool _canFireLaser = true;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private float _fireRate = .5f;

    private float _speedboost = 1.0f;

    [SerializeField]
    private bool _isTripleShootActive = false;
    [SerializeField]
    private bool _isSpeedBoostActive = false;



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

        if (_isSpeedBoostActive == true)
        {
            _speedboost = 2.0f;
        }
        else
        {
            _speedboost = 1.0f;
        }


        transform.Translate(Vector3.right * HorizontalInput * _speed * _speedboost * Time.deltaTime);
        transform.Translate(Vector3.up * VerticalInput * _speed * _speedboost * Time.deltaTime);

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
       
        if (_isTripleShootActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0f, .85f, 0f), Quaternion.identity);
        }
        
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

    

    public void TripleShotActive()
    {

        _isTripleShootActive = true;
        StartCoroutine(TripleShotCoolDown());
    }

    IEnumerator TripleShotCoolDown()
    {

        yield return new WaitForSeconds(8.0f);
        _isTripleShootActive = false;

    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostCoolDown());
    }

    IEnumerator SpeedBoostCoolDown()
    {
        yield return new WaitForSeconds(8.0f);
        _isSpeedBoostActive = false;
    }
}

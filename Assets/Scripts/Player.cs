using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private int _score;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private GameObject[] _visualDamge;

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
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private AudioClip _laserAudioClip;
    [SerializeField]
    private AudioClip _powerUpSoundClip;


    private SpawnManager _spawnManager;

    private UIManager _uIManager;





    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -4.5f, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null");
        }

        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uIManager == null)
        {
            Debug.Log("UI Manager is NULL");

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

        AudioSource.PlayClipAtPoint(_laserAudioClip, transform.position);

        _canFireLaser = false;
        StartCoroutine(ReloadTimer());

    }

    public void Damage()
    {

        if (_isShieldActive == true)
        {
            return;
        }
        else
        {
            _lives--;
            _uIManager.UpdateLives(_lives);
            VisualDamage();
        }


        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);

        }
    }

    public void AddPoints(int points)
    {

        _score += points;
        _uIManager.UpdateScore(_score);

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

    public void ShieldActive()
    {
        _isShieldActive = true;
        _playerShield.SetActive(true);
        StartCoroutine(ShieldCoolDown());

    }

    IEnumerator ShieldCoolDown()
    {
        yield return new WaitForSeconds(8.0f);
        _isShieldActive = false;
        _playerShield.SetActive(false);

    }

    void VisualDamage()
    {
        switch(_lives)
        {
            case 2:
                int randomDamage = Random.Range(0, 2);
                _visualDamge[randomDamage].SetActive(true);
                break;

            case 1:
                if (_visualDamge[0].activeSelf)
                {
                    _visualDamge[1].SetActive(true);
                }
                else
                {
                    _visualDamge[0].SetActive(true);
                }
                break;

            default:
                break;


        }   

    }

    public void  PowerUpSound()
    {

        AudioSource.PlayClipAtPoint(_powerUpSoundClip, transform.position);

    }
}

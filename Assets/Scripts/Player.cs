using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private int _score;

    private int _ammoCount;
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
    [SerializeField]
    private float _speedboost = 1.0f;


    private float _thrusterCharge = 100f;
    private float _thrusterUseRate = 20.0f;
    private float _thrusterChargeRate = 2.5f;
    


    private float _angle = 100;

    private int _hits;

    [SerializeField]
    private bool _isTripleShootActive = false;
    [SerializeField]
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldActive = false;

    private bool _isPhotonBlastActive = false;

    private bool _isThrusterActive;
    [SerializeField]
    private AudioClip _laserAudioClip;
    [SerializeField]
    private AudioClip _powerUpSoundClip;


    private SpawnManager _spawnManager;

    private UIManager _uIManager;
    [SerializeField]
    private SpriteRenderer _shieldstrength;

    private CameraShake _cameraShake;




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

        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        if (_cameraShake == null)
        {
            Debug.Log("Camera Shake is Null");
        }

        _ammoCount = 15;
        _uIManager.UpdateAmmoCount(_ammoCount);

    }

    // Update is called once per frame
    void Update()
    {

        PlayerMovement();


        if (Input.GetKeyDown(KeyCode.Space) && _canFireLaser && _ammoCount > 0)
        {

            FireLaser();

        }

        if (Input.GetKey(KeyCode.LeftShift) && _thrusterCharge > 0.0f) 
        {
            _isThrusterActive = true;
            UseThrusterFuel();
        }
        else
        {
            _isThrusterActive = false;
            if (_thrusterCharge < 100.0f)
            {
                GenerateThrusterFuel();
            }
        }

    }

    void PlayerMovement()
    {
       
        CheckSpeedBoost();

        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical"); 

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
        else if (_isPhotonBlastActive == true)
        {
           
            for (int i = 0; i < 9; i++)
            {
                _angle = _angle - 20;
                GameObject photonBlast = Instantiate(_laserPrefab, transform.position, Quaternion.Euler(0, 0, _angle));

            }

            _angle = 100;


        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0f, .85f, 0f), Quaternion.identity);
        }

        AudioSource.PlayClipAtPoint(_laserAudioClip, transform.position);

        _ammoCount -= 1;
        _uIManager.UpdateAmmoCount(_ammoCount);

        _canFireLaser = false;
        StartCoroutine(ReloadTimer());

    }

    public void Damage()
    {

        if (_isShieldActive == true)
        {
            _hits += 1;
            ShieldStrength(_hits);

            return;
        }
        else
        {
            _lives--;
            _uIManager.UpdateLives(_lives);
            StartCoroutine(_cameraShake.Shake(0.5f, .15f));
            VisualDamage();
        }


        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);

        }
    }

    public void RepairDamage()
    {

        if (_lives < 3)
        {
            _lives++;
            _uIManager.UpdateLives(_lives);
            if (_visualDamge[0].activeSelf)
            {
                _visualDamge[0].SetActive(false);

            }
            else
            {
                _visualDamge[1].SetActive(false);
            }
            

        }

    }

    public void AddPoints(int points)
    {

        _score += points;
        _uIManager.UpdateScore(_score);

    }

    public void AddLasers()
    {
        _ammoCount += 15;
        _uIManager.UpdateAmmoCount(_ammoCount);
        
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
      
    }

    public void PhotonBlastActive()
    {

        _isPhotonBlastActive = true;
        StartCoroutine(PhotonBlastCoolDown());

    }

    IEnumerator PhotonBlastCoolDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isPhotonBlastActive = false;
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

    void CheckSpeedBoost()
    {

        if (_isSpeedBoostActive == true)
        {
            _speedboost = 1.5f;
        }
        else if (_isThrusterActive == true)
        {
            _speedboost = 2.0f;
        }
        else
        {
            _speedboost = 1.0f;

        }
    }

    void UseThrusterFuel()
    {

        _thrusterCharge = Mathf.MoveTowards(_thrusterCharge, 0.0f, _thrusterUseRate * Time.deltaTime);
        _uIManager.UpdateThrusterCharge(_thrusterCharge);
        

    }

    void GenerateThrusterFuel()
    {
        _thrusterCharge = Mathf.MoveTowards(_thrusterCharge, 100.0f, _thrusterChargeRate * Time.deltaTime);
        _uIManager.UpdateThrusterCharge(_thrusterCharge);
    }

    void ShieldStrength(int hits)
    {

        switch(hits)
        {

            case 0:
                break;
            case 1:
                _shieldstrength.color = new Color(1, 1, 1, .50f);
                break;
            case 2:
                _shieldstrength.color = new Color(1, 1, 1, .10f);
                break;
            case 3:
                _isShieldActive = false;
                _hits = 0;
                _playerShield.SetActive(false);
                _shieldstrength.color = new Color(1, 1, 1, 1);
                break;
            default:
                break;


        }

    }
}

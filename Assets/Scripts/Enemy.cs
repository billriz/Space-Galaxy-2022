using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private int EnemyID; // 0 = Normal 1 = Smart 2 = Aggresive 3 = Dodge 4 = Boss

    
    private float _fireRate;
    [SerializeField]
    private bool _canFire;

    private bool _canFireAtPlayer = true;
    private bool _canFireAtPowerUp = true;

    
    [SerializeField]
    private AudioClip _enemyExplosionClip;
    [SerializeField]
    private GameObject _enemyLaserPrefab;



    
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private GameObject _EnemyShield;

    private bool _isEnemyDestroyed;



    private Player _player;

    private Animator _anim;

    private Vector3 _direction;
    private bool _canMoveAtAngle;
        
    private float _castDistance = 10.0f;

    private float _playerDistance = 4.0f;

    private float _distanceToPlayer;


    private float _ramMultiplier = 6.0f;

    [SerializeField]
    float LaserCastRadius = .5f;
    [SerializeField]
    float LaserCastDistance = 8.0f;

    float DodgeRate = 1.0f;




    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.Log("Player is NULL");
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {

            Debug.Log("Animator is NULL");
        }

        StartCoroutine(FireControlRoutine());

        if (EnemyID != 4 && Random.value >= .6f)
        {
            _canMoveAtAngle = true;

            if (transform.position.x > 0.0f)
            {
                _direction = Vector3.left;
            }
            else
            {
                _direction = Vector3.right;
            }
        }

        if (Random.value >= .85f && EnemyID != 4)
        {

            _EnemyShield.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {

        CalculatMovement();

        if (_canFire == true)
        {

            FireLaser();
            
        }

        CheckForPowerUps();

        if (EnemyID == 1)
        {

            CheckForPlayer();
        }

        if (EnemyID == 2)
        {

            RammingPlayer();
        }

        if (EnemyID == 3)
        {

            DodgeLaser();
        }
        

    }

    void CalculatMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (_canMoveAtAngle == true)
        {

            transform.Translate(_direction * _speed * Time.deltaTime);
        }
        

        if (transform.position.y <= -7.1f)
        {
            float Randomx = Random.Range(-9f, 9f);
            transform.position = new Vector3(Randomx, 7.5f, 0);
            if (transform.position.x > 0.0f)
            {
                _direction = Vector3.left;
            }
            else
            {
                _direction = Vector3.right;
            }

        }

        if (EnemyID  == 4)
        {

            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 3.25f, 7.25f), 0f);
        }

    }

    void FireLaser()
    {
        
        _canFire = false;      
        
        GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        AudioSource.PlayClipAtPoint(_laserSoundClip, transform.position);

        for (int i = 0; i < lasers.Length; i++)
        {

            lasers[i].SetEnemyLaser();
        }

        StartCoroutine(FireControlRoutine());

    }

    void FireAtPowerUps()
    {
        _canFireAtPowerUp = false;

        GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        AudioSource.PlayClipAtPoint(_laserSoundClip, transform.position);

        for (int i = 0; i < lasers.Length; i++)
        {

            lasers[i].SetEnemyLaser();
        }

        StartCoroutine(FireAtPowerUpControlRoutine());
    }

    void FireSmartLaser()
    {
        _canFireAtPlayer = false;
        GameObject enemySmartLaser = Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0f, 2.54f, 0f), Quaternion.identity);
        Laser[] lasers = enemySmartLaser.GetComponentsInChildren<Laser>();
        AudioSource.PlayClipAtPoint(_laserSoundClip, transform.position);

        for (int i = 0; i < lasers.Length; i++)
        {

            lasers[i].SetSmartEnemyLaser();
        }

        StartCoroutine(SmartLaserContolRoutine());

    }

    IEnumerator FireControlRoutine()
    {

        _fireRate = Random.Range(3.0f, 7.0f);
        yield return new WaitForSeconds(_fireRate);
        if (_isEnemyDestroyed == false)
        {

            _canFire = true;
        }

    }

    IEnumerator FireAtPowerUpControlRoutine()
    {

        yield return new WaitForSeconds(2.0f);
        if (_isEnemyDestroyed == false)
        {

            _canFireAtPowerUp = true;
        }

    }

    IEnumerator SmartLaserContolRoutine()
    {

        yield return new WaitForSeconds(2.0F);
        if (_isEnemyDestroyed == false)
        {
            _canFireAtPlayer = true;
        }
    }

        private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (_player != null)
            {

                _player.Damage();
            }

            if (_EnemyShield.activeSelf)
            {
                _EnemyShield.SetActive(true);
                return;
            }
            else
            {
               
                EnemyDestroyed();

            }            
                      
        }

        if (other.tag == "Laser")
        {

            Destroy(other.gameObject);

            if (_EnemyShield.activeSelf)
            {
                _EnemyShield.SetActive(false);
                return;

            }
            else
            {
                
                if (_player != null)
                {
                    _player.AddPoints(10);
                }

                EnemyDestroyed();

            }          
           
        }

    }

    void CheckForPowerUps()
    {

       
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _castDistance, LayerMask.GetMask("PowerUps"));
        
        
        if (hit.collider != null && _canFireAtPowerUp == true && _isEnemyDestroyed == false)
        {
            
            FireAtPowerUps();
        }

    }

    void CheckForPlayer()
    {
       
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, _castDistance, LayerMask.GetMask("Player"));       
        
        
        if (hit.collider != null && _canFireAtPlayer == true && _isEnemyDestroyed == false)
        {
            
            FireSmartLaser();
           
        } 
    }

    void RammingPlayer()
    {

        _distanceToPlayer = Vector2.Distance(_player.transform.position, this.transform.position);

        if (_distanceToPlayer <= _playerDistance)
        {

            Vector3 dir = this.transform.position - _player.transform.position;
            dir = dir.normalized;
            this.transform.position -= dir * Time.deltaTime * (_speed * _ramMultiplier);

        }

    }

    void DodgeLaser()
    {

        RaycastHit2D Laserhit = Physics2D.CircleCast(transform.position, LaserCastRadius, Vector2.down, LaserCastDistance, LayerMask.GetMask("Laser"));               

        if (Laserhit.collider != null)
        {
          
            if (Laserhit.collider.CompareTag("Laser"))
            {
                
                transform.position = new Vector3(transform.position.x - DodgeRate, transform.position.y, transform.position.z);
                DodgeRate -= .3f;

                if (DodgeRate <= 0f)
                {
                    DodgeRate = .05f;
                     
                }
            }
        }
    }

  

    void EnemyDestroyed()
    {

        _isEnemyDestroyed = true;
        _speed = 0.2f;
        _anim.SetTrigger("OnEnemyDeath");
        AudioSource.PlayClipAtPoint(_enemyExplosionClip, transform.position);
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.5f);
    }
}

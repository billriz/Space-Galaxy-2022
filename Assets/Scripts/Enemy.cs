using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private float _fireRate;

    private bool _canFire;

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

        if (Random.value >= .6f)
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

        if (Random.value >= .85f)
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

    IEnumerator FireControlRoutine()
    {
        _fireRate = Random.Range(3.0f, 7.0f);
        yield return new WaitForSeconds(_fireRate);
        if (_isEnemyDestroyed == false)
        {
            _canFire = true;
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

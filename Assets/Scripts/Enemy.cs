using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;

    private Animator _anim;

    
    
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
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -7.1f)
        {
            float Randomx = Random.Range(-9f, 9f);
            transform.position = new Vector3(Randomx, 7.5f, 0);

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

            EnemyDestroyed();

           // _anim.SetTrigger("OnEnemyDeath");
           // Destroy(this.gameObject, 1.2f);
        }

        if (other.tag == "Laser")
        {

            Destroy(other.gameObject);
          //  _anim.SetTrigger("OnEnemyDeath");
         //   Destroy(other.gameObject, 1.2f);
            if (_player != null)
            {
               _player.AddPoints(10);
            }

            EnemyDestroyed();
        }

    }


    void EnemyDestroyed()
    {

        _speed = 0.2f;
        _anim.SetTrigger("OnEnemyDeath");
        Destroy(this.gameObject, 2.5f);
    }
}

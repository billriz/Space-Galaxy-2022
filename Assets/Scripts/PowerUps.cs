using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{

    private float _speed = 3.5f;
    [SerializeField]
    private int _powerUpID; // 0 = Triple Shot 1 = Speed Boost 2 = Shield 3 = Recharge Laser 4 = Repair 5 = Photon Blast


    private GameObject _player;

    [SerializeField]
    private GameObject _explosionPrefab;

    private void Start()
    {
       

    }




    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            PlayerCollectingPowerUps();
        }
        else
        {

            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }  
        

        if (transform.position.y < -8.0f)
        {

            Destroy(this.gameObject);

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {

                player.PowerUpSound();
               
                switch(_powerUpID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;

                    case 1:
                        player.SpeedBoostActive();
                        break;

                    case 2:
                        player.ShieldActive();
                        break;

                    case 3:
                        player.AddLasers();
                        break;

                    case 4:
                        player.RepairDamage();
                        break;

                    case 5:
                        player.PhotonBlastActive();

                        break;

                    default:
                        break;
                           
                }   
                
                Destroy(this.gameObject);
            }
            
        }

        if (other.tag == "Laser")
        {

            Destroy(other.gameObject);
            GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            explosion.transform.localScale = new Vector3(.5f, .5f, .5f);
            Destroy(this.gameObject);
        }
    }

   



    void PlayerCollectingPowerUps()
    {

        _player = GameObject.Find("Player");
        Vector3 _direction = this.transform.position - _player.transform.position;
        _direction = _direction.normalized;
        this.transform.position -= _direction * Time.deltaTime * (_speed * 3);

    }
}

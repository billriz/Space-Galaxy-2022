using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{

    private float _speed = 3.5f;
    [SerializeField]
    private int _powerUpID; // 0 = Triple Shot 1 = Speed Boost 2 = Shield 3 = Recharge Laser

       
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

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


                    default:
                        break;
                           
                }   
                
                Destroy(this.gameObject);
            }
            
        }
    }
}

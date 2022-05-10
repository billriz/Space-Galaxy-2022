using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    
    
    // Start is called before the first frame update
    void Start()
    {
        
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);

        }
        else if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {

                player.Damage();
            }
            
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;



    private bool _isEnemyLaser = false;

    private bool _isSmartEnemyLaser = false;

       
    

    // Update is called once per frame
    void Update()
    {

        CalculatMovement();
        
    }

    void CalculatMovement()
    {
        
        if (_isEnemyLaser == false)
        {
            MoveUp();          
        }
        else
        {

            MoveDown();
        }
        
        
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8.0f)
        {
            if (transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8.0f)
        {
            if (transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void SetEnemyLaser()
    {

        _isEnemyLaser = true;

    }

    public void SetSmartEnemyLaser()
    {
        _isSmartEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true || _isSmartEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            if ( player != null)
            {

                player.Damage();
            }

            Destroy(this.gameObject);

        }
    }
}

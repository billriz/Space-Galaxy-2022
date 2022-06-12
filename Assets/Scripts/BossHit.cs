using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHit : MonoBehaviour
{

    private EnemyBoss _boss;
    
    
    
    // Update is called once per frame
    void Update()
    {
        _boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyBoss>();

        if (_boss == null)
        {
            Debug.LogError("The Boss is Null");
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.tag == "Laser")
        {
            _boss.BossDamage();
            GetComponent<Collider2D>().enabled = false;
            GameObject Explosion = this.transform.GetChild(0).gameObject;
            GameObject Fire = this.transform.GetChild(1).gameObject;
            Explosion.SetActive(true);
            Fire.SetActive(true);
            // StartCoroutine(HitAnimationRoutine());
            Destroy(other.gameObject);

        }
       
    }
}

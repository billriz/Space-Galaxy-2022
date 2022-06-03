using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{

    private float _bossSpeed = 1.5f;
    [SerializeField]
    private float _bossSideSpeed = .10f;
    [SerializeField]
    private int _hitCount;


    private bool _canFireMain = true;

    private bool _canFireTurit = true;

    private float _turitSpeed = 1.5f;

    private float _mainLaserFireRate;

    private float _turitLaserFireRate;

    private float _angle = 10f;
    [SerializeField]
    private float _angleZ;

    [SerializeField]
    private GameObject _bossLaser;
    [SerializeField]
    private GameObject _turitLaserCenter, _turitLaserLeft, _turitLaserRight;
    [SerializeField]
    private GameObject _turitCenter, _turitLeft, _turitRight;

    public float start;
    public float end;
    public float t = 0;
    public float PosX;
    private bool _isInPosition;
    [SerializeField]
    private GameObject _bossExplosionPreFab;
    [SerializeField]
    private GameObject _bossHitExplosionPreFab;

    






    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        CalculateMovement();



        if (_canFireMain == true)
        {
           FireMainLaser();
          
        }  
        
        if (_canFireTurit == true)
        {

            FireTurit();
            
        }

         
        
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _bossSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 3.25f, 7.25f), 0f);

        if (transform.position.y <= 3.25f)
        {

            if (!_isInPosition)
            {
                start = transform.position.x;
                end = 8.0f;
                _isInPosition = true;

            }
            t += _bossSideSpeed * Time.deltaTime;
            if (t >= 1.0f)
            {
                start = transform.position.x;
                end = -transform.position.x;
                t = 0.0f;
            }

            SideMovement();

        }

        TuritMovement();

    }

    private void SideMovement()
    {
        PosX = Mathf.Lerp(start, end, t);
        transform.position = new Vector3(PosX, transform.position.y, transform.position.z);

    }

    void TuritMovement()
    {
        _angleZ = _turitCenter.transform.rotation.z;
        _turitCenter.transform.Rotate(0, 0, _angle * _turitSpeed * Time.deltaTime);
        _turitRight.transform.Rotate(0, 0, _angle * _turitSpeed * Time.deltaTime);
        _turitLeft.transform.Rotate(0, 0, _angle * _turitSpeed * Time.deltaTime);
        if (_angleZ > .41f)
        {
            _angle = -10f;
        }
        else if (_angleZ < -.41f)
        {
            _angle = 10f;
        }

    }

    void FireMainLaser()
    {
        _canFireMain = false;
        Instantiate(_bossLaser, transform.position, Quaternion.identity);
        StartCoroutine(MainLaserCoolDownRoutine());
    }

    void FireTurit()
    {
        _canFireTurit = false;

        GameObject _turitLaser = Instantiate(_turitLaserCenter, _turitCenter.transform.position, _turitCenter.transform.rotation, _turitCenter.transform);
        GameObject _turitLaser1 = Instantiate(_turitLaserCenter, _turitLeft.transform.position, _turitLeft.transform.rotation, _turitLeft.transform);
        GameObject _turitLaser2 = Instantiate(_turitLaserCenter, _turitRight.transform.position, _turitRight.transform.rotation, _turitRight.transform);
        _turitLaser.transform.parent = null;
        _turitLaser1.transform.parent = null;
        _turitLaser2.transform.parent = null;

        StartCoroutine(TuritLaserCoolDownRoutine());
        
    }
   

    IEnumerator MainLaserCoolDownRoutine()
    {
        _mainLaserFireRate = Random.Range(3.0f, 7.0f);
        yield return new WaitForSeconds(_mainLaserFireRate);
        _canFireMain = true;
    }

    IEnumerator TuritLaserCoolDownRoutine()
    {
        _turitLaserFireRate = Random.Range(2.0f, 10.0f);
        yield return new WaitForSeconds(_turitLaserFireRate);
        _canFireTurit = true;
    }

       

    public void BossDamage()
    {

        _hitCount += 1;
                
        if (_hitCount > 15)
        {
            _bossSideSpeed = .08f;
        }
        if (_hitCount >= 25)
        {
            t = 0f;
            Instantiate(_bossExplosionPreFab, transform.position, Quaternion.identity);
            this.gameObject.SetActive(false);
            Destroy(this.gameObject, 2.5f);
        }

    }
   
        

}

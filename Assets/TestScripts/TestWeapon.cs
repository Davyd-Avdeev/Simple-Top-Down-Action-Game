using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bullet;
    public GameObject bulletV2;

    public float spreadAngle;
    public int bulletIn—hamber;
    public int bulletCount;
    public int bulletDrob;

    List<Quaternion> bullets;

    public float startReloadTime;
    public float timeReload;

    [SerializeField]
    private ParticleSystem ImpactParticleSystem;

    public float startShotTime;
    private float timeShot;

    bool buttonPresed;

    private TrailRenderer bulletTrail;
    private float LastShootTime;

    private void Start()
    {
        bulletTrail = bullet.GetComponent<TrailRenderer>();
        bullets = new List<Quaternion>(bulletDrob);
        for (int i = 0; i < bulletDrob; i++)
        {
            bullets.Add(Quaternion.Euler(Vector3.zero));
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            buttonPresed = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            buttonPresed = false;
        }

        if (timeShot <= 0 && buttonPresed)
        {
            timeShot = startShotTime;
            Shoot();
        }
        else
        {
            timeShot -= Time.deltaTime;
        }
    }

    private Vector2 GetDirection()
    {
        Vector2 direction = transform.right;

        if (true)
        {
            direction += new Vector2(
                Random.Range(-0.1f, 0.1f),
                Random.Range(-0.1f, 0.1f)
            );

            direction.Normalize();
        }

        return direction;
    }
    //void Shoot()
    //{
    //    for (int i = 0; i < bulletDrob; i++)
    //    {
    //        Quaternion rnd = Random.rotation;
    //        Vector3 pos = new Vector3(firePoint.position.x, firePoint.position.y, 39.42422f);
    //        GameObject b = Instantiate(bullet, pos, firePoint.rotation);
    //        b.transform.rotation = Quaternion.RotateTowards(b.transform.rotation, firePoint.rotation, spreadAngle);
    //        b.transform.rotation = Quaternion.RotateTowards(b.transform.rotation, rnd, spreadAngle);
    //    }
    //}

    void Shoot()
    {
        for (int i = 0; i < bulletDrob; i++)
        {
            Quaternion rnd = Random.rotation;
            //RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right + rnd);            

            Vector2 direction = GetDirection();

            RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, direction);

            if (hitInfo.collider != null)
            {
                TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hitInfo));



                LastShootTime = Time.time;
            }
        }       

    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit2D Hit)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;

        while (time < 0.5f)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, Hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }
        Trail.transform.position = Hit.point;
        Instantiate(ImpactParticleSystem, Hit.point, Quaternion.LookRotation(Hit.normal));

        Destroy(Trail.gameObject, Trail.time);
        Debug.Log("I DIED");
    }
}

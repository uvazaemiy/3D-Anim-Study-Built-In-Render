using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    [SerializeField] private Transform firePoints;
    [SerializeField] private GameObject bulletPrefab;
    public TurretData data;
    public GameObject range;
    private float fireRate, speedOfBullet, damage;
    private bool canRotate;
    private Transform target;

    private void Start()
    {
        fireRate = data.FireRate;
        speedOfBullet = data.SpeedOfBullet;
        damage = data.Damage;
        name = data.TurretName;
        canRotate = data.CanRotate;
        StartCoroutine(Shoot(fireRate));
    }
    private void Update()
    {
        if (!canRotate) return;
        if(target) transform.GetChild(0).GetChild(0).LookAt(target.position, Vector3.up);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!target && col.tag is "Enemy") target = col.transform;
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.transform == target) target = null;
    }

    IEnumerator Shoot(float delay)
    {
        if (target)
        {
            foreach (Transform firePoint in firePoints)
            {
                Rigidbody curBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();
                curBullet.AddForce(firePoint.forward * speedOfBullet);
                curBullet.transform.name = damage.ToString();
                Destroy(curBullet.gameObject, 1f);
            }
        }
        yield return new WaitForSeconds(delay);
        StartCoroutine(Shoot(fireRate));
    }
}

using System.Collections;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform[] bulletSpawns;
    public LayerMask collisionMask;

    public float additionalSpeed = 1.5f;
    public float maxFireTime = 7f;
    public float cooldownTime = 3f;
    public float fireRate = 0.1f;
    public float cannonRange = 150f;
    public float bulletDamage = 25f;
    public float bulletLifetime = 3f;
    public float bulletWidth = 0.3f;
    public float cannonMaxFireAngle = 5f;

    private float fireTimer = 0f;
    private float fireCooldown = 0f;
    private bool isCooldown = false;
    private float planeSpeed;
    private Plane ownerPlane;
    private Rigidbody planeRigidbody;
    private AudioScript audioScript;

    private void Awake()
    {
        ownerPlane = GetComponent<Plane>();
        planeRigidbody = GetComponentInParent<Rigidbody>();
        audioScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioScript>();
    }

    private void Update()
    {
        if (planeRigidbody != null)
        {
            planeSpeed = planeRigidbody.velocity.magnitude;
        }

        if (!isCooldown && Input.GetMouseButton(0))
        {
            TryFireCannon();
        }

        if (Input.GetMouseButtonUp(0))
        {
            fireTimer = 0f;
        }
    }

    public void TryFireCannon()
    {
        fireTimer += Time.deltaTime;
        fireCooldown += Time.deltaTime;

        if (fireTimer >= maxFireTime)
        {
            StartCoroutine(Cooldown());
        }
        else if (fireCooldown >= fireRate)
        {
            FireBullets();
            fireCooldown = 0f;
        }
    }

    private void FireBullets()
    {
        float bulletSpeed = GetBulletSpeed();
        audioScript.PlaySFX(audioScript.Shooting);
        audioScript.SetVolume(0.3f);

        foreach (Transform spawn in bulletSpawns)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawn.position, Quaternion.LookRotation(spawn.forward));
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.velocity = spawn.forward * bulletSpeed;

            StartCoroutine(DetectHit(bullet, spawn.position, spawn.forward, bulletSpeed));
        }
    }


    private IEnumerator DetectHit(GameObject bullet, Vector3 spawnPosition, Vector3 direction, float speed)
    {
        Vector3 lastPosition = spawnPosition;
        float startTime = Time.time;

        while (Time.time < startTime + bulletLifetime)
        {
            Vector3 currentPosition = bullet.transform.position;
            Vector3 diff = currentPosition - lastPosition;
            float distance = speed * Time.deltaTime;

            Ray ray = new Ray(lastPosition, diff.normalized);
            if (Physics.SphereCast(ray, bulletWidth, out RaycastHit hit, distance, collisionMask))
            {
                Plane targetPlane = hit.collider.GetComponent<Plane>();

                if (targetPlane != null && targetPlane != ownerPlane)
                {
                    targetPlane.ApplyDamage(bulletDamage);
                    Destroy(bullet);
                    yield break;
                }
            }

            lastPosition = currentPosition;
            yield return null;
        }

        Destroy(bullet);
    }

    public float GetBulletSpeed()
    {
        return planeSpeed <= 75f ? 75f * additionalSpeed : planeSpeed * additionalSpeed;
    }

    private IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
        fireTimer = 0f;
        fireCooldown = 0f;
    }
}
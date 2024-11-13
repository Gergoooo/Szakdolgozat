using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : Plane
{
    private Transform player => PlayerPlaneController.Instance;
    public float approachSpeedFactor = 1.5f;
    public float minDistanceFromPlayer = 150f;
    public float rollFactor = 0.1f; 
    public LayerMask groundLayer;
    public float groundAvoidanceDistance = 150f;
    public float groundAvoidanceAngle = 45f; 
    public float groundCollisionDistance = 250f;
    public float minGroundDistance = 100f; 
    public GunScript gunScript;
    private EnemySpawner spawner;


    public float reactionDelayMin = 7.5f;
    public float reactionDelayMax = 5f;
    public float reactionDelayDistance = 250f;

    private Queue<ControlInput> inputQueue = new Queue<ControlInput>();

    private struct ControlInput
    {
        public float time;
        public Vector3 targetPosition;

        public ControlInput(float time, Vector3 targetPosition)
        {
            this.time = time;
            this.targetPosition = targetPosition;
        }
    }

    private bool isAvoidingGround = false;

    protected override void Start()
    {
        base.Start();
        spawner = FindObjectOfType<EnemySpawner>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (IsCloseToGround(out float distanceToGround))
        {
            Vector3 steering = AvoidGround(distanceToGround);
            SetControlInput(steering);
            isAvoidingGround = true;
        }
        else
        {
            isAvoidingGround = false;
            SteerToTarget(Time.fixedDeltaTime, transform.position);
        }

        TryFireAtPlayer();
    }

    private void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        thrust = (distanceToPlayer > minDistanceFromPlayer) ?
            Mathf.Lerp(thrust, topSpeed, Time.deltaTime * approachSpeedFactor) :
            Mathf.Lerp(thrust, 0, Time.deltaTime * approachSpeedFactor * 0.5f);

        rb.velocity = transform.forward * thrust;

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    private bool IsCloseToGround(out float distanceToGround)
    {
        var velocityRot = Quaternion.LookRotation(rb.velocity.normalized);
        var ray = new Ray(rb.position, velocityRot * Quaternion.Euler(groundAvoidanceAngle, 0, 0) * Vector3.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, groundCollisionDistance, groundLayer))
        {
            distanceToGround = hit.distance;
            return distanceToGround < minGroundDistance;
        }
        distanceToGround = Mathf.Infinity;
        return false;
    }

    private Vector3 AvoidGround(float distanceToGround)
    {
        float distanceFactor = (minGroundDistance - distanceToGround) / minGroundDistance;
        distanceFactor = Mathf.Clamp01(distanceFactor);

        var roll = transform.rotation.eulerAngles.z;
        if (roll > 180f) roll -= 360f;

        return new Vector3(-1, 0, Mathf.Clamp(-roll * rollFactor, -1, 1) + distanceFactor);
    }

    private void SteerToTarget(float dt, Vector3 planePosition)
    {
        bool foundTarget = false;
        Vector3 targetPosition = Vector3.zero;

        float delay = reactionDelayMax;

        if (Vector3.Distance(planePosition, player.position) < reactionDelayDistance)
        {
            delay = reactionDelayMin;
        }

        while (inputQueue.Count > 0)
        {
            var input = inputQueue.Peek();

            if (input.time + delay <= Time.time)
            {
                targetPosition = input.targetPosition;
                inputQueue.Dequeue();
                foundTarget = true;
            }
            else
            {
                break; 
            }
        }

        if (foundTarget)
        {
            MoveTowardsPlayer();
        }

        if (!isAvoidingGround) 
        {
            Vector3 randomOffset = new Vector3(
               Random.Range(-2f, 2f),
               Random.Range(-2f, 2f),
               Random.Range(-2f, 2f)
            );
            inputQueue.Enqueue(new ControlInput(Time.time, targetPosition));
        }
    }

    private void TryFireAtPlayer()
    {
        if (player == null || gunScript == null) return;

        Vector3 targetPosition = player.position;
        Vector3 targetVelocity = player.GetComponent<Rigidbody>().velocity;

        Vector3 leadPoint = Utilities.FirstOrderIntercept(
            rb.position,
            rb.velocity,
            gunScript.GetBulletSpeed(),
            targetPosition,
            targetVelocity
        );

        Vector3 error = leadPoint - rb.position;
        float distanceToTarget = error.magnitude;
        Vector3 targetDirection = error.normalized;

        float angleToTarget = Vector3.Angle(transform.forward, targetDirection);
        if (distanceToTarget < gunScript.cannonRange * 0.8f && angleToTarget < gunScript.cannonMaxFireAngle * 0.75f)
        {
            gunScript.TryFireCannon();
        }
    }

    protected override void HandleDeathSequence()
    {
        audioScript.PlaySFX(audioScript.Explosion);

        GameObject explosion = Instantiate(explosionEffect, gameObject.transform.position, gameObject.transform.rotation);
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
        spawner.HandleEnemyDestroyed(gameObject);
        Destroy(explosion, explosionDuration);
    }

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Level"))
        {
            HandleDeathSequence();
        }
        if (other.gameObject.CompareTag("Player"))
        {
            HandleDeathSequence();
            spawner.HandleEnemyDestroyed(gameObject);
            
        }
    }
}
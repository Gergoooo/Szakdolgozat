using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Plane : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Speed")]
    [SerializeField]
    protected float thrust = 100f;
    [SerializeField]
    protected float topSpeed = 200f;
    [SerializeField]
    protected float rotationSpeed = 57f;
    protected float defaultLift = 1f;
    
    //Health
    protected float health = 100f;
    protected float currentHealth;

    //Explosion
    [Header("Explosion")]
    public GameObject explosionEffect;
    protected float explosionDuration = 2f;

    //Audio Manager
    public AudioScript audioScript;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = health;
    }

    protected virtual void FixedUpdate()
    {
        rb.velocity = transform.forward * thrust;
    }

    public virtual void SetThrottle(float value)
    {
        thrust = Mathf.Clamp(value, 1, topSpeed);
    }

    public virtual void SetControlInput(Vector3 input)
    {
        float rollInput = input.z * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward, -rollInput);

        float pitchInput = input.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.right, pitchInput);
    }

    public float Thrust
    {
        get => thrust;
        set => thrust = Mathf.Clamp(value, 0, topSpeed);
    }

    public float TopSpeed => topSpeed;

    public float RotationSpeed => rotationSpeed;

    public virtual void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            HandleDeathSequence();
        }
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Level"))
        {
            HandleDeathSequence();
        }
    }

    protected virtual void HandleDeathSequence()
    {
        gameObject.SetActive(false);
    }
    
    protected void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlaneController : Plane
{
    private GameOverMenu gameOverMenu;
    [Header("GameOver")]
    public GameObject gameOver;
    public GameObject planeBody;
    public GameObject cameraObject;


    [Header("Health")]
    public HealthManager healthBar;

    public static Transform Instance;

    private void Awake()
    {
        Instance = transform;
    }

    protected override void Start()
    {
        base.Start();
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, health);
        }
        if (audioScript != null)
        {
            audioScript.PlayLoopingEngineSound();
            audioScript.SetVolume(0.5f);
            audioScript.SetPitch(0.5f);
        }
        gameOverMenu = FindObjectOfType<GameOverMenu>();
    }
    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }

        HandleThrustInput();
        HandleRotationInput();
    }

    void HandleThrustInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetThrottle(thrust + 10f);
            audioScript.SetVolume(0.05f);
            audioScript.SetPitch(0.05f);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetThrottle(thrust - 10f);
            audioScript.SetVolume(-0.05f);
            audioScript.SetPitch(-0.05f);
        }
    }

    void HandleRotationInput()
    {
        float rollInput = Input.GetAxis("Horizontal");
        float pitchInput = Input.GetAxis("Vertical");
        SetControlInput(new Vector3(pitchInput, 0, rollInput));
    }

    public override void ApplyDamage(float damage)
    {
        base.ApplyDamage(damage); 
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, health);
        }
    }

    protected override void HandleDeathSequence()
    {
        audioScript.StopEngineSound();

        audioScript.PlaySFX(audioScript.Explosion);

        GameObject explosion = Instantiate(explosionEffect, planeBody.transform.position, planeBody.transform.rotation);

        if (planeBody != null)
        {
            planeBody.SetActive(false);
        }

        Destroy(explosion, explosionDuration);

        if (gameOver != null)
        {
            gameOverMenu.ShowGameOverScreen();
        }
    }

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Level"))
        {
            HandleDeathSequence();
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            ApplyDamage(20f);
        }
    }
}
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //Movement Settings
    [SerializeField] private float speed = 5f; //Player movement speed

    private Rigidbody2D _rb; //Rigidbody2D component

    //Keyboard & Joystick Inputs
    private float keyboardHorizontalInput; //Horizontal input from keyboard
    private float keyboardVerticalInput; //Vertical input from keyboard
    private float joystickHorizontalInput; //Horizontal input from joystick
    private float joystickVerticalInput; //Vertical input from joystick

    private float _horizontalInput; //Final horizontal movement input
    private float _verticalInput; //Final vertical movement input

    public FixedJoystick fixedJoystick; //Reference to joystick

    [SerializeField] private GameObject projectilePrefab; //Normal projectile prefab
    [SerializeField] private Transform shootPoint; //Empty object at player gun
    [SerializeField] private float projectileSpeed = 10f; //Speed of projectiles

    [SerializeField] private Transform flame; //Flame object for visual effect
    [SerializeField] private float minFlameX = 0.01f; //Minimum flame width
    [SerializeField] private float maxFlameX = 0.2f; //Maximum flame width

    [SerializeField] private float reverseBlendSpeed = 5f; //Speed to interpolate reverse effect
    private float currentReverseMultiplier = 1f; //Current multiplier for movement
    private float targetReverseMultiplier = 1f; //Target multiplier for movement

    [Header("Death Settings")]
    [SerializeField] private float destroyDelay = 0.5f; //Delay before destroying player after death
    private Animator animator; //Animator for player
    private bool isDead = false; //Tracks if player is dead

    [Header("UI")]
    [SerializeField] private UIManager uiManager; //Reference to UI manager

    [Header("Homing Bullet")]
    [SerializeField] private GameObject homingBulletPrefab; //Prefab for homing bullets
    private bool homingBulletsActive = false; //Is homing bullet power-up active

    [Header("Shield")]
    [SerializeField] private GameObject shieldVisualPrefab; //Visual for shield
    private GameObject activeShieldVisual; //Instantiated shield object
    private bool shieldActive = false; //Is shield currently active
    [SerializeField] private Transform shieldAnchor; //Anchor point for shield visuals

    [Header("Multi Shot")]
    [SerializeField] private float multiShotAngle = 15f; //Angle between multi-shot bullets
    private bool multiShotActive = false; //Is multi-shot active

    [Header("Power Up UI Icons")]
    [SerializeField] private GameObject homingIcon; //UI icon for homing bullets
    [SerializeField] private GameObject shieldIcon; //UI icon for shield
    [SerializeField] private GameObject multishotIcon; //UI icon for multi-shot

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource; //Audio source for shooting
    [SerializeField] private AudioClip shootSound; //Sound played when shooting
    [SerializeField] private AudioClip playerDeathSound; //Sound played when dying

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); //Get Rigidbody2D component
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate; //Smooth physics
        _rb.freezeRotation = true; //Prevent rotation from physics

        animator = GetComponent<Animator>(); //Get Animator component
    }

    private void Update()
    {
        //Smoothly blend toward target reverse multiplier
        currentReverseMultiplier = Mathf.Lerp(
            currentReverseMultiplier,
            targetReverseMultiplier,
            reverseBlendSpeed * Time.deltaTime
        );

        //Update keyboard input
        keyboardHorizontalInput = Input.GetAxisRaw("Horizontal");
        keyboardVerticalInput = Input.GetAxisRaw("Vertical");

        //Update joystick input
        joystickHorizontalInput = fixedJoystick.Horizontal;
        joystickVerticalInput = fixedJoystick.Vertical;

        //Combine both inputs with multiplier
        _horizontalInput = Mathf.Clamp(keyboardHorizontalInput + joystickHorizontalInput, -1f, 1f) * currentReverseMultiplier;
        _verticalInput = Mathf.Clamp(keyboardVerticalInput + joystickVerticalInput, -1f, 1f) * currentReverseMultiplier;

        // Shoot left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        //Apply movement velocity
        _rb.velocity = new Vector2(
            _horizontalInput * speed,
            _verticalInput * speed
        );

        RotatePlayer(); //Rotate player based on vertical input
        UpdateFlameScale(); //Update flame visual size
    }

    private void RotatePlayer()
    {
        float maxTilt = 15f; //Maximum tilt angle
        float targetAngle = _verticalInput * maxTilt; //Calculate target angle
        _rb.rotation = Mathf.Lerp(_rb.rotation, targetAngle, 0.1f); //Smoothly rotate
    }

    private void UpdateFlameScale()
    {
        if (flame == null) return;

        float t = (_horizontalInput + 1f) / 2f; //Convert horizontal input from range -1..1 into 0..1
        float targetX = Mathf.Lerp(minFlameX, maxFlameX, t); //Calculate target flame width
        float currentY = flame.localScale.y; //Keep current Y scale
        flame.localScale = Vector3.Lerp(flame.localScale, new Vector3(targetX, currentY, 1f), 0.1f); //Smoothly update scale
    }

    public void Shoot()
    {
        if (shootPoint == null) return;

        //Play shooting sound
        if (audioSource != null && shootSound != null)
            audioSource.PlayOneShot(shootSound);

        if (multiShotActive)
        {
            ShootBulletAtAngle(0f); //Center bullet
            ShootBulletAtAngle(multiShotAngle); //Upward angled bullet
            ShootBulletAtAngle(-multiShotAngle); //Downward angled bullet
        }
        else
        {
            ShootBulletAtAngle(0f); //Single shot
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Enemy") || other.CompareTag("EnemyFeature"))
        {
            if (shieldActive)
            {
                Destroy(other.gameObject); //Shield destroys incoming enemy
                return;
            }

            Die(); //Player dies
            return;
        }

        if (other.CompareTag("Boss") || other.CompareTag("BossFeature"))
        {
            if (!shieldActive)
                Die(); //Player dies
        }
    }

    private void Die()
    {
        isDead = true;
        homingBulletsActive = false; //Disable homing bullets

        if (_rb != null)
            _rb.velocity = Vector2.zero; //Stop movement

        if (animator != null)
            animator.SetTrigger("OnDeath"); //Play death animation

        if (animator == null)
            gameObject.SetActive(false); //Fallback if no animator

        //Play the death sound
        if (audioSource != null && playerDeathSound != null)
            audioSource.PlayOneShot(playerDeathSound);

        Destroy(gameObject, destroyDelay); //Destroy player after delay

        if (uiManager != null)
            uiManager.OnPlayerDeath(); //Notify UI
    }

    public void SetMovementReversed(bool reversed)
    {
        targetReverseMultiplier = reversed ? -1f : 1f; //Set multiplier for movement
    }

    //Homing Bullets Power Up---
    public void ActivateHomingBullets(float duration)
    {
        StartCoroutine(HomingBulletsRoutine(duration));
    }

    private IEnumerator HomingBulletsRoutine(float duration)
    {
        homingBulletsActive = true; // Enable homing bullets
        if (homingIcon != null) homingIcon.SetActive(true); //Show UI icon

        yield return new WaitForSeconds(duration); //Wait for duration

        homingBulletsActive = false; //Disable homing bullets
        if (homingIcon != null) homingIcon.SetActive(false); //Hide UI icon
    }

    //Shield Power Up---
    public void ActivateShield(float duration)
    {
        StartCoroutine(ShieldRoutine(duration));
    }

    private IEnumerator ShieldRoutine(float duration)
    {
        shieldActive = true; //Enable shield
        if (shieldIcon != null) shieldIcon.SetActive(true); //Show shield UI

        if (shieldVisualPrefab != null && shieldAnchor != null)
        {
            activeShieldVisual = Instantiate(shieldVisualPrefab, shieldAnchor); //Spawn shield visual
            activeShieldVisual.transform.localPosition = Vector3.zero; //Reset local position
        }

        yield return new WaitForSeconds(duration); //Wait for duration

        DeactivateShield(); //Disable shield
    }

    private void DeactivateShield()
    {
        shieldActive = false; //Disable shield
        if (shieldIcon != null) shieldIcon.SetActive(false); //Hide shield UI

        if (activeShieldVisual != null)
            Destroy(activeShieldVisual); //Destroy shield visual
    }

    //Multi-Shot Power Up---
    private void ShootBulletAtAngle(float angleOffset)
    {
        GameObject bullet;

        if (homingBulletsActive && homingBulletPrefab != null)
        {
            bullet = Instantiate(homingBulletPrefab, shootPoint.position, Quaternion.identity); //Spawn homing bullet

            HomingBullets homing = bullet.GetComponent<HomingBullets>();
            homing.SetSpeed(projectileSpeed); //Set speed
        }
        else
        {
            bullet = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity); //Spawn normal bullet

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            Vector2 direction = Quaternion.Euler(0, 0, angleOffset) * Vector2.right; //Calculate direction

            rb.velocity = direction * projectileSpeed; //Set velocity
        }
    }

    public void ActivateMultiShot(float duration)
    {
        StartCoroutine(MultiShotRoutine(duration));
    }

    private IEnumerator MultiShotRoutine(float duration)
    {
        multiShotActive = true; // Enable multi-shot
        if (multishotIcon != null) multishotIcon.SetActive(true); //Show UI icon

        yield return new WaitForSeconds(duration); //Wait for duration

        multiShotActive = false; // Disable multi-shot
        if (multishotIcon != null) multishotIcon.SetActive(false); //Hide UI icon
    }
}

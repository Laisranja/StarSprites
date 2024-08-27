using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Nave parameters")]
    [SerializeField] private float naveAcceleration = 10f;
    [SerializeField] private float naveMaxVelocity = 10f;
    [SerializeField] private float naveRotationSpeed = 180f;
    [SerializeField] private float bulletSpeed = 8f;

    [Header("Object References")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Rigidbody2D tiroPrefab;

    private Rigidbody2D naveRigidbody;
    private bool isAlive = true;
    private bool isAccelerating = false;

    private void Start()
    {
        naveRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isAlive)
        {
            HandleNaveAcceleration();
            HandleNaveRotation();
            HandleShooting();
        }
    }

    private void FixedUpdate()
    {
        if (isAlive && isAccelerating)
        {
            naveRigidbody.AddForce(naveAcceleration * transform.up);
            naveRigidbody.velocity = Vector2.ClampMagnitude(naveRigidbody.velocity, naveMaxVelocity);
        }
    }

    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody2D bullet = Instantiate(tiroPrefab, bulletSpawn.position, Quaternion.identity);

            Vector2 naveVelocity = naveRigidbody.velocity;
            Vector2 naveDirection = transform.up;
            float naveForwardSpeed = Vector2.Dot(naveVelocity, naveDirection);

            if (naveForwardSpeed < 0)
            {
                naveForwardSpeed = 0;
            }
            bullet.velocity = naveDirection * naveForwardSpeed;

            bullet.AddForce(bulletSpeed * transform.up, ForceMode2D.Impulse);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid"))
        {
            isAlive = false;

            GameManager gameManager = FindAnyObjectByType<GameManager>();

            gameManager.GameOver();

            Destroy(gameObject);
        }
    }
    private void HandleNaveAcceleration()
    {
        isAccelerating = Input.GetKey(KeyCode.W);
    }

    private void HandleNaveRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(naveRotationSpeed * Time.deltaTime * transform.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-naveRotationSpeed * Time.deltaTime * transform.forward);
        }

    }
}

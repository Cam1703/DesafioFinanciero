using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public Rigidbody2D rb;
    public Arma arma;
    public Camera mainCamera; // Referencia a la cámara principal

    public float invulnerabilityDuration = 0.5f; // Duración de la invulnerabilidad en segundos
    private bool isInvulnerable = false; // Estado de invulnerabilidad
    private float invulnerabilityTimer = 0f; // Temporizador de invulnerabilidad

    Vector2 moveDirection;
    Vector2 mousePosition;
    float camVerticalExtent;
    float camHorizontalExtent;
    private bool canShoot = true; // Flag para controlar si se puede disparar
    private float shootTimer = 0f; // Temporizador para controlar el retraso entre disparos
    private float shootCooldown = 0.2f; // Tiempo de espera entre disparos



    [SerializeField] SobreviviendoAlAhorroGameManager gameManager;
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Si no se asigna la cámara, se obtiene la cámara principal automáticamente
        }

        // Calcula los límites de la cámara
        camVerticalExtent = mainCamera.orthographicSize;
        camHorizontalExtent = camVerticalExtent * mainCamera.aspect;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Actualizar temporizador de disparo
        if (!canShoot)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                canShoot = true;
            }
        }

        // Disparar si se presiona el botón izquierdo del mouse y se puede disparar
        if (Input.GetMouseButton(0) && canShoot)
        {
            arma.Disparar();
            gameManager.RestarMonedas(10);

            // Iniciar el temporizador de espera entre disparos
            canShoot = false;
            shootTimer = shootCooldown;
        }

        moveDirection = new Vector2(moveX, moveY).normalized;
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Actualizar el temporizador de invulnerabilidad
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer <= 0)
            {
                isInvulnerable = false;
            }
        }

        if (isInvulnerable)
        {
            // Parpadear el sprite del jugador mientras es invulnerable
            float blinkInterval = 0.1f; // Intervalo de parpadeo en segundos
            float blinkTimer = Mathf.PingPong(Time.time, blinkInterval);
            GetComponentInChildren<SpriteRenderer>().enabled = blinkTimer > blinkInterval / 2;
        }
        else
        {
            // Restaurar el sprite del jugador si no es invulnerable
            GetComponentInChildren<SpriteRenderer>().enabled = true;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);

        // Limitar la posición del jugador dentro del rango de visión de la cámara
        rb.position = new Vector2(
            Mathf.Clamp(rb.position.x, -camHorizontalExtent + 1, camHorizontalExtent - 1),
            Mathf.Clamp(rb.position.y, -camVerticalExtent + 1, camVerticalExtent - 1)
        );


        int smoothness = 6; // Suavidad del cambio de escala
        //mirroring dependiendo de posición del mouse
        // Set the target scale based on mouse position
        Vector3 targetScale = (mousePosition.x > rb.position.x) ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);

        // Smoothly interpolate towards the target scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * smoothness);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvulnerable)
        {
            gameManager.RestarVida();
            
            // Activar la invulnerabilidad
            isInvulnerable = true;
            invulnerabilityTimer = invulnerabilityDuration;
            
        }
    }
}

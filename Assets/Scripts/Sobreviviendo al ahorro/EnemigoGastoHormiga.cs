using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemigoGastoHormiga : MonoBehaviour
{
    [SerializeField] private int vida = 100;
    [SerializeField] private int daño = 10;
    private GameObject jugador;
    private Rigidbody2D rb;
    private Vector2 movement;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private TMP_Text vidaTexto;
    public Camera mainCamera; // Referencia a la cámara principal

    float camVerticalExtent;
    float camHorizontalExtent;

    [SerializeField] SobreviviendoAlAhorroGameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jugador = GameObject.FindGameObjectWithTag("Player");
        gameManager = FindObjectOfType<SobreviviendoAlAhorroGameManager>();
        vidaTexto.text = vida.ToString();

        // Inicia el movimiento aleatorio
        StartCoroutine(RandomMovement());
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Si no se asigna la cámara, se obtiene la cámara principal automáticamente
        }

        // Calcula los límites de la cámara
        camVerticalExtent = mainCamera.orthographicSize;
        camHorizontalExtent = camVerticalExtent * mainCamera.aspect;
    }

    IEnumerator RandomMovement()
    {
        while (true)
        {
            // Genera un nuevo vector de movimiento aleatorio
            movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

            // Espera un tiempo aleatorio antes de cambiar de dirección
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
            
        }
    }

    private void FixedUpdate()
    {
        // Mueve al personaje con el movimiento actual
        moveCharacter(movement);
    }

    void moveCharacter(Vector2 direction)
    {
        // Calcula la posición potencial del siguiente movimiento
        Vector2 nextPosition = (Vector2)transform.position + (direction * moveSpeed * Time.deltaTime);

        // Limita la posición dentro de los límites de la cámara
        float clampedX = Mathf.Clamp(nextPosition.x, mainCamera.transform.position.x - camHorizontalExtent, mainCamera.transform.position.x + camHorizontalExtent);
        float clampedY = Mathf.Clamp(nextPosition.y, mainCamera.transform.position.y - camVerticalExtent, mainCamera.transform.position.y + camVerticalExtent);

        // Actualiza la posición solo si está dentro de los límites de la cámara
        rb.MovePosition(new Vector2(clampedX, clampedY));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si el enemigo colisiona con un objeto de etiqueta "Moneda"
        if (collision.gameObject.CompareTag("Moneda"))
        {
            // Reduce la vida del enemigo y actualiza el texto de la vida
            vida -= daño;
            vidaTexto.text = vida.ToString();

            // Destruye al enemigo si su vida es menor o igual a cero
            if (vida <= 0)
            {
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reduce cantidad de monedas del jugador
            gameManager.RestarMonedas(daño);
            Destroy(gameObject);
        }
    }
}

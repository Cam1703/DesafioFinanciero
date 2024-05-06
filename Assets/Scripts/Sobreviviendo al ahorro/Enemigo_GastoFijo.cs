using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemigo_GastoFijo : MonoBehaviour
{
    [SerializeField] public int vida = 100;
    [SerializeField] private int daño = 10;
    private GameObject jugador;
    private Rigidbody2D rb;
    private Vector2 movement;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private TMP_Text vidaTexto;
    public bool spawnearEstaRonda = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jugador = GameObject.FindGameObjectWithTag("Player");
        vidaTexto.text = vida.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (jugador != null)
        {
            //mirroring dependiendo de posición del jugador
            if (transform.position.x > jugador?.transform?.position.x)
            {
                Vector3 scale = new Vector3(-1, 1, 1);
                transform.localScale = scale;
                vidaTexto.transform.localScale = new Vector3(-1, 1, 1); // Voltea solo el texto
            }
            else
            {
                Vector3 scale = new Vector3(1, 1, 1);
                transform.localScale = scale;
                vidaTexto.transform.localScale = new Vector3(1, 1, 1); // Mantiene la escala original del texto
            }
            Vector3 direction = (Vector3)(jugador?.transform?.position - transform?.position);
            movement = direction.normalized;
        }
    

    }

    private void FixedUpdate()
    {
        moveCharacter(movement);
    }
    void moveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Moneda"))
        {
            vida -= daño;
            vidaTexto.text = vida.ToString();
            if (vida <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

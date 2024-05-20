using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arma : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject balaPrefab;
    public Transform firePoint;
    public float bulletForce = 20f;
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Disparar()
    {
        // Obtiene la posición del mouse en el mundo
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Asegura que la z sea cero para 2D

        // Calcula la dirección hacia la que se disparará la bala
        Vector2 direccion = (mousePos - firePoint.position).normalized;

        // Instancia la bala
        GameObject bala = Instantiate(balaPrefab, firePoint.position, Quaternion.identity);
        audioSource.Play();

        // Obtiene el componente Rigidbody2D de la bala
        Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();

        // Aplica fuerza a la bala en la dirección calculada
        rb.AddForce(direccion * bulletForce, ForceMode2D.Impulse);
    }

}

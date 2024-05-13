using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class GestionAlumnos : MonoBehaviour
{
    [SerializeField] private GameObject filaTablaAlumnosPrefab;
    [SerializeField] private Transform contenedorTablaAlumnos;

    [SerializeField] private GameManager gameManager;
    private List<Usuario> alumnos;
    private string codigoSalon;

    // Start is called before the first frame update
    void Start()
    {
        codigoSalon = gameManager.GetSalonActual().codigoSalon;
        alumnos = GetUsuariosAlumnosInscritosAlSalon();
        MostrarAlumnosEnTabla();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<Usuario> GetUsuariosAlumnosInscritosAlSalon()
    {
        List<Usuario> usuarios = SaveSystem.LoadUsers();

        return usuarios.FindAll(usuario => !usuario.isProfesor && codigoSalon == usuario.codigoDeClase);
    }


    private void CrearFilaTablaAlumnos(Usuario alumno, int id)
    {
        GameObject filaTablaAlumnos = Instantiate(filaTablaAlumnosPrefab, contenedorTablaAlumnos);
        filaTablaAlumnos.transform.GetChild(0).GetComponent<TMP_Text>().text = id.ToString();
        filaTablaAlumnos.transform.GetChild(1).GetComponent<TMP_Text>().text = alumno.nombres;
        filaTablaAlumnos.transform.GetChild(2).GetComponent<TMP_Text>().text = alumno.apellidos;
        filaTablaAlumnos.transform.GetChild(3).GetComponent<TMP_Text>().text = alumno.usuario;
    }

    public void MostrarAlumnosEnTabla()
    {
        alumnos = GetUsuariosAlumnosInscritosAlSalon();
        foreach (Transform child in contenedorTablaAlumnos)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < alumnos.Count; i++)
        {
            CrearFilaTablaAlumnos(alumnos[i], i + 1);
        }
    }
}

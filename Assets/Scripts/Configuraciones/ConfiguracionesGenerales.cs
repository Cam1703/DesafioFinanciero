using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfiguracionesGenerales : MonoBehaviour
{

    [SerializeField] private TMP_InputField usuarioInput;
    [SerializeField] private TMP_InputField contrasenaInput;
    [SerializeField] private TMP_InputField nombresInput;
    [SerializeField] private TMP_InputField apellidsoInput;
    [SerializeField] private TMP_InputField codigoDeClaseInput;
    [SerializeField] private TMP_Text codigoDeClaseText;

    [SerializeField] private Button buttonGuardar;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject buttonTabsPanel;
    [SerializeField] private TMP_Text mensajeError;
    private Usuario usuarioActual;
    private bool isGuardando;

    // Start is called before the first frame update
    void Start()
    {
        usuarioActual = gameManager.GetUsuarioActual();
        Debug.Log("Usuario actual: " + usuarioActual);
        if (usuarioActual != null)
        {
            usuarioInput.text = usuarioActual.usuario;
            // La contraseña ya no se guarda en texto plano, así que no hay nada que
            // mostrar aquí: se deja vacío y solo se cambia si el usuario escribe una nueva.
            contrasenaInput.text = "";
            if (contrasenaInput.placeholder is TMP_Text placeholderText)
            {
                placeholderText.text = "Dejar en blanco para no cambiarla";
            }
            nombresInput.text = usuarioActual.nombres;
            apellidsoInput.text = usuarioActual.apellidos;
            if (!usuarioActual.isProfesor) // Si no es profesor
            {
                codigoDeClaseInput.gameObject.SetActive(true);
                codigoDeClaseText.gameObject.SetActive(true);
                codigoDeClaseInput.text = usuarioActual.codigoDeClase;
                buttonTabsPanel.SetActive(false);
            }
            else
            {
                codigoDeClaseInput.gameObject.SetActive(false);
                codigoDeClaseText.gameObject.SetActive(false);
            }
        }
    }

    public async void GuardarCambios()
    {
        if (isGuardando) return;
        isGuardando = true;
        if (buttonGuardar != null) buttonGuardar.enabled = false;
        if (mensajeError != null) mensajeError.gameObject.SetActive(false);

        try
        {
            Usuario usuario = gameManager.GetUsuarioActual();

            Salon nuevoSalon = null;
            if (!usuario.isProfesor)
            {
                // Antes esto no se validaba (a diferencia del registro), así que se podía
                // guardar un código de salón inexistente y romper Selección de Juego más adelante.
                nuevoSalon = await SaveSystem.GetSalonByCodigoAsync(codigoDeClaseInput.text);
                if (nuevoSalon == null)
                {
                    if (mensajeError != null)
                    {
                        mensajeError.text = "El código de salón ingresado no existe.";
                        mensajeError.gameObject.SetActive(true);
                    }
                    return;
                }
            }

            usuario.usuario = usuarioInput.text;
            if (!string.IsNullOrEmpty(contrasenaInput.text))
            {
                (string hash, string salt) = PasswordHasher.Hash(contrasenaInput.text);
                usuario.passwordHash = hash;
                usuario.passwordSalt = salt;
            }
            usuario.nombres = nombresInput.text;
            usuario.apellidos = apellidsoInput.text;
            usuario.codigoDeClase = codigoDeClaseInput.text;

            Debug.Log("Usuario modificado: " + usuario);
            await SaveSystem.ModifyUserAsync(usuario);
            gameManager.SetUsuarioActual(usuario);
            if (nuevoSalon != null)
            {
                gameManager.SetSalonActual(nuevoSalon);
            }

            contrasenaInput.text = "";
        }
        catch (Exception e)
        {
            Debug.LogError("Error al guardar cambios de perfil: " + e);
            if (mensajeError != null)
            {
                mensajeError.text = "No se pudo conectar con el servidor. Intenta de nuevo.";
                mensajeError.gameObject.SetActive(true);
            }
        }
        finally
        {
            isGuardando = false;
            if (buttonGuardar != null) buttonGuardar.enabled = true;
        }
    }
}

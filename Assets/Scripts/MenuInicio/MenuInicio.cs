using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MenuInicio : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TMP_InputField usuarioInput;
    [SerializeField] private TMP_InputField contrasenaInput;
    [SerializeField] private TMP_Text mensajeError;
    [SerializeField] private Button botonIniciarSesion;

    private bool isIniciandoSesion;

    private void Awake()
    {
        SaveSystem.Init();
    }

    public async void IniciarSesion()
    {
        if (isIniciandoSesion) return;
        isIniciandoSesion = true;
        if (botonIniciarSesion != null) botonIniciarSesion.enabled = false;
        mensajeError.gameObject.SetActive(false);

        try
        {
            Usuario usuario = await SaveSystem.BuscarUsuarioAsync(usuarioInput.text);
            bool credencialesValidas = usuario != null && PasswordHasher.Verify(contrasenaInput.text, usuario.passwordHash, usuario.passwordSalt);

            if (!credencialesValidas)
            {
                mensajeError.text = "Usuario o contraseña incorrectos";
                mensajeError.gameObject.SetActive(true);
                return;
            }

            gameManager.SetUsuarioActual(usuario);

            // Se cachea el salón del alumno una sola vez al iniciar sesión, para que
            // el resto de pantallas (selección de juego, minijuegos) lo lean en
            // memoria vía gameManager.GetSalonActual() en vez de volver a consultar Firestore.
            if (!usuario.isProfesor && !string.IsNullOrEmpty(usuario.codigoDeClase))
            {
                Salon salon = await SaveSystem.GetSalonByCodigoAsync(usuario.codigoDeClase);
                if (salon != null)
                {
                    gameManager.SetSalonActual(salon);
                }
                else
                {
                    Debug.LogWarning($"El usuario {usuario.usuario} tiene codigoDeClase='{usuario.codigoDeClase}' pero ese salón ya no existe.");
                }
            }

            gameManager.CambiarEscena("MenuPrincipal");
        }
        catch (Exception e)
        {
            Debug.LogError("Error al iniciar sesión: " + e);
            mensajeError.text = "No se pudo conectar con el servidor. Intenta de nuevo.";
            mensajeError.gameObject.SetActive(true);
        }
        finally
        {
            isIniciandoSesion = false;
            if (botonIniciarSesion != null) botonIniciarSesion.enabled = true;
        }
    }
}

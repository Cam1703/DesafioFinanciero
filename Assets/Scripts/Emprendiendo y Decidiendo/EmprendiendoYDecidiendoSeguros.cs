using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EmprendiendoYDecidiendoSeguros : MonoBehaviour
{
    private float seguroContraIncendios = 100f;
    private float seguroContraRobo = 100f;

    [SerializeField] private TMP_Text seguroContraIncendiosPrecioText;
    [SerializeField] private TMP_Text seguroContraRoboPrecioText;

    [SerializeField] private Button seguroContraIncendiosButton;
    [SerializeField] private Button seguroContraRoboButton;
    [SerializeField] private Sprite buttonSelectedSprite;
    [SerializeField] private Sprite buttonUnselectedSprite;

    [SerializeField] private EmprendiendoYDecidiendoPresupuesto presupuesto;

    // Start is called before the first frame update
    void Start()
    {
        seguroContraIncendiosPrecioText.text = seguroContraIncendios.ToString() + "S/. mensuales";
        seguroContraRoboPrecioText.text = seguroContraRobo.ToString() + "S/. mensuales";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AgreagarSeguroContraIncendiosAlPresupuesto()
    {
        presupuesto.ActualizarPagoSeguros(seguroContraIncendios);
    }

    private void AgreagarSeguroContraRoboAlPresupuesto()
    {
        presupuesto.ActualizarPagoSeguros(seguroContraRobo);
    }

    private void QuitarSeguroContraIncendiosDelPresupuesto()
    {
        presupuesto.ActualizarPagoSeguros(-seguroContraIncendios);
    }

    private void QuitarSeguroContraRoboDelPresupuesto()
    {
        presupuesto.ActualizarPagoSeguros(-seguroContraRobo);
    }

    public void UpdateBotonSprite(Button button)
    {
        if (button.image.sprite == buttonSelectedSprite)
        {
            button.image.sprite = buttonUnselectedSprite;
            button.GetComponentInChildren<TMP_Text>().text = "Agregar al presupuesto";
        }
        else
        {
            button.image.sprite = buttonSelectedSprite;
            button.GetComponentInChildren<TMP_Text>().text = "Quitar del presupuesto";
        }
    }

    public void AgregarOQuitarSeguroDelPresupuesto(string nombreSeguro)
    {
        if(nombreSeguro== "Incendios")
        {
            if (seguroContraIncendiosButton.image.sprite == buttonSelectedSprite)
            {
                QuitarSeguroContraIncendiosDelPresupuesto();
            }
            else
            {
                AgreagarSeguroContraIncendiosAlPresupuesto();
            }
            UpdateBotonSprite(seguroContraIncendiosButton);
        }
        else if(nombreSeguro == "Robo")
        {
            if (seguroContraRoboButton.image.sprite == buttonSelectedSprite)
            {
                QuitarSeguroContraRoboDelPresupuesto();
            }
            else
            {
                AgreagarSeguroContraRoboAlPresupuesto();
            }
            UpdateBotonSprite(seguroContraRoboButton);
        }
    }

    
}

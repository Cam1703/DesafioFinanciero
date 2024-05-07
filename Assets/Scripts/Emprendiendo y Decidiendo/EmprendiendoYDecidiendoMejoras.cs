using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class EmprendiendoYDecidiendoMejoras : MonoBehaviour
{
    private float expandirLocalPrecio = 1500f;
    private float mejoresInsumosPrecio = 1500f;
    private float campañaPublicitariaPrecio = 1500f;
    private float mejoresSillasPrecio = 1500f;

    [SerializeField] TMP_Text expandirLocalPrecioText;
    [SerializeField] TMP_Text mejoresInsumosPrecioText;
    [SerializeField] TMP_Text campañaPublicitariaPrecioText;
    [SerializeField] TMP_Text mejoresSillasPrecioText;

    [SerializeField] Button expandirLocalButton;
    [SerializeField] Button mejoresInsumosButton;
    [SerializeField] Button campañaPublicitariaButton;
    [SerializeField] Button mejoresSillasButton;

    [SerializeField] GameObject panelCompra;
    [SerializeField] EmprendiendoYDecidiendoPresupuesto presupuesto;
    [SerializeField] private EmprendiendoYDecidiendoPanelMejoras panelDeMejoras; 

    // Start is called before the first frame update
    void Start()
    {
        panelDeMejoras = panelCompra.GetComponent<EmprendiendoYDecidiendoPanelMejoras>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AbrirPanelDeCompra()
    {
        panelCompra.SetActive(true);
    }

    public void CerrarPanelDeCompra()
    {
        panelCompra.SetActive(false);
    }

    public void AbrirExpandirLocalEnPanelDeCompra()
    {
        panelDeMejoras.SetNombreYPrecioDeMejora("Expandir Local", expandirLocalPrecio);
    }

    public void AbrirMejoresInsumosEnPanelDeCompra()
    {
        panelDeMejoras.SetNombreYPrecioDeMejora("Mejores Insumos", mejoresInsumosPrecio);
    }

    public void AbrirCampañaPublicitariaEnPanelDeCompra()
    {
        panelDeMejoras.SetNombreYPrecioDeMejora("Campaña Publicitaria", campañaPublicitariaPrecio);
    }

    public void AbrirMejoresSillasEnPanelDeCompra()
    {
        panelDeMejoras.SetNombreYPrecioDeMejora("Mejores Sillas", mejoresSillasPrecio);
    }
}

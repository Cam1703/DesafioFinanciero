using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiarPanel : MonoBehaviour
{
     [SerializeField] List<GameObject> paneles;

    public void CambiarPanelActivo(int panelActivo)
    {
        paneles[panelActivo].SetActive(true);
        for (int i = 0; i < paneles.Count; i++)
        {
            if (i != panelActivo)
            {
                paneles[i].SetActive(false);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CambiarPanelActivo(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

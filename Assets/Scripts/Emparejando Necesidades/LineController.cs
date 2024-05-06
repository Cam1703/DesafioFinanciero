using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform startPosition;
    private Transform endPosition;
    [SerializeField] private DemandanteController objetoScriptDemandante;
    private OfertanteController objetoScriptOfertante;
    private bool lineDrawn = false;
    private PointManager pointManager;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false; // Desactivar el LineRenderer al inicio
        pointManager = FindObjectOfType<PointManager>();
    }

    private void Update()
    {


        if (objetoScriptDemandante.isPaired)
        {
            startPosition = objetoScriptDemandante.transform;
        }
        else
        {
            startPosition = null;
        }

        if (!lineDrawn && startPosition != null && endPosition != null)
        {
            lineRenderer.enabled = true; // Activar el LineRenderer antes de dibujar la línea
            SetUpLine(new Transform[] { startPosition, endPosition });
            lineDrawn = true;
         
        }

        if (Input.GetMouseButtonDown(0) && !lineDrawn)
        {
            // Obtener el objeto que se ha clickeado
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                // Verificar si el objeto clickeado es el objeto actual
                if (hit.collider.CompareTag("Ofertante"))
                {
                    objetoScriptOfertante = hit.collider.GetComponent<OfertanteController>();
                    if (objetoScriptOfertante.isPaired)
                    {
                        endPosition = hit.collider.transform;
                    }
                    else
                    {
                        endPosition = null;
                    }
                }
            }
        }


    }

    public void SetUpLine(Transform[] points)
    {
        //objetoScriptDemandante.setClickToFalse();
        //objetoScriptOfertante.setClickToFalse();

        lineRenderer.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.SetPosition(i, points[i].position);
        }
    }

    public void GetPuntoInicio()
    {
        startPosition = gameObject.GetComponentInParent<Transform>();
    }

    public void GetPuntoFin(GameObject ofertante)
    {
        endPosition = ofertante.GetComponent<Transform>();
    }

}

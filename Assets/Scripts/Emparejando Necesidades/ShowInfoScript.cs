using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowInfoScript : MonoBehaviour
{
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private string infoToShow;

    // Start is called before the first frame update
    void Start()
    {
        infoText.text = infoToShow;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                infoText.gameObject.SetActive(!infoText.gameObject.activeSelf);
            }
        }
    }
}

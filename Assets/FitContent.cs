using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitContent : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<RectTransform>().position = new Vector3(gameObject.GetComponent<RectTransform>().position.x, 0, 0);
    }


}

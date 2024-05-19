using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonInitializer : MonoBehaviour
{
    public GameManager gameManager;
    void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        // Encontrar todos los botones en la escena
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button button in buttons)
        {
            AddEventTriggerListener(button);
        }
        Debug.Log("Botones inicializados: "+ buttons.Length);
    }

    private void AddEventTriggerListener(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();

        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };

        entry.callback.AddListener((eventData) => { gameManager.PlayButtonSound(); });
        trigger.triggers.Add(entry);
    }
}

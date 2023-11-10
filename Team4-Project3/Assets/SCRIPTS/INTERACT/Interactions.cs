using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class Interactions : MonoBehaviour
{
    public LayerMask interactL;
    public int raycastDistance = 7;

    private GameObject tooltipUI;
    private TMP_Text tooltipText;

    private bool canInteract = false;
    private string type;
    private UnityEvent interEvent;

    // Start is called before the first frame update
    void Start()
    {
        tooltipUI = transform.GetChild(1).gameObject; // Gets the tool-tip panel by climbing down MainUI's children.
        tooltipText = tooltipUI.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteract();
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void CheckForInteract() // Checks whether an interactable is in front of the player (camera).
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, raycastDistance, interactL))
        {
            canInteract = true;

            interEvent = hit.collider.GetComponent<InteractBehavior>().onInteract;
            tooltipUI.SetActive(true);
            tooltipText.text = "Press E to Interact";
            type = hit.collider.GetComponent<InteractBehavior>().type;
        }
        else { canInteract = false; tooltipUI.SetActive(false); }
    }

    private void Interact() // Activates what the interactable is supposed to do.
    {
        switch (type)
        {
            case "fishing":
                break;
            case "rod":
                Debug.Log("rod");
                break;
            case "shovel":
                Debug.Log("shovel");
                break;
            default:
                break;
        }
        interEvent.Invoke();
    }
}

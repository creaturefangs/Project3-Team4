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
    private GameObject target;
    private string type;
    private UnityEvent interEvent;

    private Fishing fishing;

    // Start is called before the first frame update
    void Start()
    {
        tooltipUI = transform.GetChild(2).gameObject; // Gets the tool-tip panel by climbing down MainUI's children.
        tooltipText = tooltipUI.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        fishing = GetComponent<Fishing>();
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
            target = hit.collider.gameObject;
            interEvent = target.GetComponent<InteractBehavior>().onInteract;
            type = target.GetComponent<InteractBehavior>().type;

            canInteract = true;
            tooltipUI.SetActive(true);
            tooltipText.text = "Press E to Interact";
        }
        else { canInteract = false; tooltipUI.SetActive(false); }
    }

    private void Interact() // Activates what the interactable is supposed to do.
    {
        switch (type)
        {
            case "fish":
                fishing.StartMinigame(target);
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

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class Interactions : MonoBehaviour
{
    public LayerMask interactL;
    public int raycastDistance = 5;

    public GameObject tooltipUI;
    public TMP_Text tooltipText;

    public bool canInteract = true;
    private bool interactTarget = false;
    private GameObject target;
    private string type;
    private UnityEvent interEvent;

    private Fishing fishing;
    private PauseManager pause;
    private Inventory inv;

    // Start is called before the first frame update
    void Start()
    {
        tooltipUI = transform.GetChild(2).gameObject; // Gets the tool-tip panel by climbing down MainUI's children.
        tooltipText = tooltipUI.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        fishing = GetComponent<Fishing>();
        pause = GetComponentInParent<PauseManager>();
        inv = GetComponent<Inventory>();
        canInteract = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteract();
        if (interactTarget && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void CheckForInteract() // Checks whether an interactable is in front of the player (camera).
    {
        RaycastHit hit;
        if (canInteract && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, raycastDistance, interactL))
        {
            target = hit.collider.gameObject;
            interEvent = target.GetComponent<InteractBehavior>().onInteract;
            type = target.GetComponent<InteractBehavior>().type;

            interactTarget = true;
            switch (type)
            {
                case "fish":
                    tooltipText.text = "Press E to begin Mini-Game";
                    break;
                case "home":
                    if (inv.currency >= inv.requirement) { tooltipText.text = "Press E to Choose Level"; }
                    else { tooltipText.text = $"You need at least {inv.requirement - inv.currency} more fish to leave for the day!"; }
                    break;
                default:
                    tooltipText.text = "Press E to Interact";
                    break;
            }
            tooltipUI.SetActive(true);
        }
        else
        {
            interactTarget = false;
            if (!fishing.inMinigame) { tooltipUI.SetActive(false); }
        }
    }

    private void Interact() // Activates what the interactable is supposed to do.
    {
        switch (type)
        {
            case "fish":
                fishing.StartMinigame(target);
                break;
            case "home":
                GameObject levelSelectUI = transform.parent.GetChild(2).gameObject;
                if (inv.currency >= inv.requirement)
                {
                    levelSelectUI.SetActive(true);
                    pause.EnterMenu();
                }
                break;
            case "rod":
                Debug.Log("rod");
                break;
            case "shovel":
                Debug.Log("shovel");
                break;
            case "net":
                Debug.Log("net");
                break;
            case "dirt":
                Debug.Log("dirt");
                break;
            default:
                break;
        }
        interEvent.Invoke();
    }

    private IEnumerator TemporaryTip(string tip)
    {
        tooltipUI.SetActive(true);
        tooltipText.text = tip;
        yield return new WaitForSeconds(5f);
        tooltipUI.SetActive(false);
    }
}

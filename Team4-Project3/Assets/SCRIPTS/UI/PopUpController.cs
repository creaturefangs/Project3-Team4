using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpController : MonoBehaviour
{
    public GameObject popupUI;
    private TMP_Text itemName;
    private TMP_Text description;
    private TMP_Text itemValue;

    private int currentValue = 0;

    private Inventory inv;

    void Start()
    {
        // Disable the tooltip initially
        popupUI.SetActive(false);
        itemName = popupUI.transform.GetChild(0).GetComponent<TMP_Text>();
        description = popupUI.transform.GetChild(1).GetComponent<TMP_Text>();
        itemValue = popupUI.transform.GetChild(3).GetComponent<TMP_Text>();

        inv = GameObject.Find("MainUI").GetComponentInChildren<Inventory>();
    }

    // Called when the mouse pointer enters the icon
   public void OnMouseEnter()
    {
        SetItemText();
        Vector2 mousePos = Input.mousePosition;

        // Set tooltip position (optional)
        popupUI.transform.position = new Vector2(mousePos.x, mousePos.y);

        // Enable the tooltip
        popupUI.SetActive(true);
    }

    // Called when the mouse pointer exits the icon
    public void OnMouseExit()
    {
        // Disable the tooltip
        popupUI.SetActive(false);
    }

    public void OnMouseDown()
    {
        if ((inv.currency - currentValue) >= 0)
        {
            popupUI.SetActive(false);
            Destroy(gameObject);
        }
        inv.BuyItem(currentValue, name);
    }

    private void SetItemText()
    {
        switch (name)
        {
            case "BountifulHarvest":
                itemName.text = "Bountiful Harvest";
                description.text = "Fish have a chance of being worth more when caught.";
                currentValue = 3;
                break;
            case "FishWhisperer":
                itemName.text = "Fish Whisperer";
                description.text = "Better chance of rarer fish spawning.";
                currentValue = 5;
                break;
            case "LooseDirt":
                itemName.text = "Loose Dirt";
                description.text = "Spend less time digging.";
                currentValue = 2;
                break;
            case "SecondWind":
                itemName.text = "Second Wind";
                description.text = "Sprint faster.";
                currentValue = 2;
                break;
            case "StrongerLine":
                itemName.text = "Stronger Line";
                description.text = "Make the fishing mini-game shorter.";
                currentValue = 5;
                break;
            case "ThickNet":
                itemName.text = "Thick Net";
                description.text = "Better chance of catching something in the net.";
                currentValue = 2;
                break;
            default:
                itemName.text = "Item";
                description.text = "An item you can purchase.";
                currentValue = 1;
                break;
        }
        itemValue.text = $"{currentValue} coins";
    }
}



using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpController : MonoBehaviour
{
    public TMP_Text popupText;
    public GameObject popupUI;

    void Start()
    {
        // Disable the tooltip initially
        popupText.enabled = false;
        popupUI.SetActive(false);
    }

    // Called when the mouse pointer enters the icon
   public void OnMouseEnter()
    {
        if (gameObject.CompareTag("IconTag"))
        {
            // Customize the tooltip text based on the tag
            popupText.text = "This is the tooltip for IconTag";

            // Set tooltip position (optional)
            popupText.transform.position = new Vector2(transform.position.x + 20f, transform.position.y);
            popupUI.transform.position = new Vector2(transform.position.x + 20f, transform.position.y);

            // Enable the tooltip
            popupText.enabled = true;
            popupUI.SetActive(true);
        }
    }

    // Called when the mouse pointer exits the icon
    public void OnMouseExit()
    {
        // Disable the tooltip
        popupText.enabled = false;
        popupUI.SetActive(false);
    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTip : MonoBehaviour
{

    public GameObject interactshopUI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            interactshopUI.SetActive(true);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            interactshopUI.SetActive(false);
        }
    }
}
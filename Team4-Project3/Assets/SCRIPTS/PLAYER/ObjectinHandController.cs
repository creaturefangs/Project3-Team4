using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectinHandController : MonoBehaviour
{
    private GameObject hand;
    public List<GameObject> tools = new List<GameObject>();



    void Start()
    {
        // Assuming your hand GameObject is named "ObjectInHand"
        hand = GameObject.Find("ObjectInHand");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Press 'P' key to pick up an object
            PickUpObject();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            // Press 'U' key to put away the object
            PutAwayObject();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Press '1' key to pull out the fishing net
            PullOutObject(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Press '2' key to pull out the shovel
            PullOutObject(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Press '3' key to pull out the fishing pole
            PullOutObject(2);
        }
    }

    void PickUpObject()
    {
        // Find the active child object in the hand
        string objectInHandName = FindObjectInHand();

        if (!string.IsNullOrEmpty(objectInHandName))
        {
            // Pick up the object
            Debug.Log("Picked up " + objectInHandName);
        }
    }

    void PutAwayObject()
    {
        // Find the active child object in the hand
        string objectInHandName = FindObjectInHand();

        if (!string.IsNullOrEmpty(objectInHandName))
        {
            // Put away the object and destroy it
            Debug.Log("Put away and destroyed " + objectInHandName);
            Destroy(hand.transform.Find(objectInHandName).gameObject);
        }
        else
        {
            Debug.Log("You don't have anything in your hand to put away.");
        }
    }

    void PullOutObject(int index)
    {
        // Check if the index is within the range of the tools list
        if (index >= 0 && index < tools.Count)
        {
            // Instantiate and activate the specified tool in the hand
            GameObject toolPrefab = tools[index];
            GameObject spawnedTool = Instantiate(toolPrefab, hand.transform);
            spawnedTool.SetActive(true);

            Debug.Log("Pulled out " + toolPrefab.name);
        }
        else
        {
            Debug.Log("Invalid tool index: " + index);
        }
    }


    string FindObjectInHand()
    {
        foreach (Transform child in hand.transform)
        {
            if (child.gameObject.activeSelf)
            {
                return child.name;
            }
        }
        return null;
    }
}



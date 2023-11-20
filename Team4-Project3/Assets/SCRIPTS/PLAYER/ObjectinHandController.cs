using System;
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
        if (Input.GetKeyDown(KeyCode.E)) // Press 'E' key to pick up an object
        {
            PickUpObject();
        }

        if (Input.GetKeyDown(KeyCode.Q)) // Press 'Q' key to put away the object
        {
            PutAwayObject();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) // Press '1' key to pull out the fishing net
        {
            //PullOutObject(0);
            SwitchItem(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // Press '2' key to pull out the shovel
        {
            //PullOutObject(1);
            SwitchItem(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) // Press '3' key to pull out the fishing pole
        {
            //PullOutObject(2);
            SwitchItem(2);
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            int currentItem = ObjectIndex();
            if (currentItem == 0) { SwitchItem(hand.transform.childCount - 1); }
            else { SwitchItem(currentItem - 1); }
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            int currentItem = ObjectIndex();
            if (currentItem == hand.transform.childCount - 1) { SwitchItem(0); }
            else { SwitchItem(currentItem + 1); }
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
            // Set the object in hand to inactive
            GameObject objectInHand = hand.transform.Find(objectInHandName).gameObject;
            objectInHand.SetActive(false);

            Debug.Log("Put away " + objectInHandName);
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

    private void SwitchItem(int index)
    {
        if (index >= 0 && index < hand.transform.childCount)
        {
            for (var i = 0; i < hand.transform.childCount; i++)
            {
                if (i == index) { hand.transform.GetChild(i).gameObject.SetActive(true); }
                else { hand.transform.GetChild(i).gameObject.SetActive(false); }
            }

        }
        else if (tools[index]) // If it's a valid tool but not yet instantiated...
        {
            foreach (Transform child in hand.transform) { child.gameObject.SetActive(false); } // Sets all other tools as inactive.
            GameObject toolPrefab = tools[index];
            GameObject spawnedTool = Instantiate(toolPrefab, hand.transform);
            spawnedTool.name = spawnedTool.name.Split("(Clone)")[0]; // Removes the "Clone" ending from the game object when instantiated.
            spawnedTool.transform.SetSiblingIndex(index); // Orders the child to the correct position among the parent object children.
            spawnedTool.SetActive(true);
        }
        else
        {
            Debug.Log("Invalid tool index: " + index);
        }
    }

    private int ObjectIndex()
    {

        for (var i = 0; i < hand.transform.childCount; i++)
        {
            if (hand.transform.GetChild(i).gameObject.activeSelf) { return i; }
        }
        return 0;
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



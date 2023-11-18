using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FishBehavior : MonoBehaviour
{
    public string rarity = "";

    private GameObject hook;
    private float speed = 0.5f;
    private Fishing fishing;

    public bool inWater;

    // Start is called before the first frame update
    void Start()
    {
        hook = GameObject.Find("FishingHook");
        fishing = GameObject.Find("MainUI").GetComponentInChildren<Fishing>();
    }

    // Update is called once per frame
    void Update()
    {
        // NoticeHook();
        if (!inWater)
        {
            Vector3 playerPos = GameObject.Find("First Person Controller Minimal").transform.position;
            transform.position = new Vector3(Random.Range(playerPos.x - 20f, playerPos.x + 21f), fishing.waterLevel, Random.Range(playerPos.z - 20f, playerPos.z + 21f));
        }
    }

    private void NoticeHook()
    {
        float distance = Vector3.Distance(transform.position, hook.transform.position);
        if (distance < 15f) // Fish will rotate to look at the hook if it's near it.
        {
            var target = Quaternion.LookRotation(hook.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, target, speed);
        }

        if (distance < 10f) // Fish will move towards the hook if it's even closer.
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, hook.transform.position, Time.deltaTime * speed);
        }

        if (distance < 5f && Input.GetKeyDown(KeyCode.E)) { fishing.StartMinigame(gameObject); } // Starts the mini-game if close enough to the hook and the player presses E.
    }

    private void OnTriggerEnter(Collider other) // Make sure either the fish or water have a RigidBody!
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            inWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            inWater = false;
        }
    }
}

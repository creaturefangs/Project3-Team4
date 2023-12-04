using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class FishBehavior : MonoBehaviour
{
    public string rarity = "";

    private GameObject hook;
    private float speed = 1.0f;
    private Fishing fishing;

    private bool inWater;
    private bool validSpawn = false; // Confirms that the fish WAS spawned in water at some point, even if it leaves the water collider afterwards.
    private bool onHook = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    bool alertActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        fishing = GameObject.Find("MainUI").GetComponentInChildren<Fishing>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inWater && !validSpawn)
        {
            Vector3 playerPos = GameObject.Find("First Person Controller Minimal").transform.position;
            transform.position = new Vector3(Random.Range(playerPos.x - 20f, playerPos.x + 21f), fishing.waterLevel, Random.Range(playerPos.z - 20f, playerPos.z + 21f));
        }

        if (fishing.lineCast && !fishing.inMinigame) { hook = GameObject.Find("FishingBob"); NoticeHook(); }
        else if (transform.position != originalPosition && !onHook)
        {
            if (!fishing.lineCast || fishing.inMinigame) { LoseInterest(); }
        }

        if (transform.position == originalPosition && transform.rotation != originalRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, speed / 3); ;
        }
    }

    private void NoticeHook() // Depending on distance, fish will look towards / move towards the fishing bob (hook).
    {
        float distance = Vector3.Distance(transform.position, hook.transform.position);
        if (distance < 15f) // Fish will rotate to look at the hook if it's near it.
        {

            var target = Quaternion.LookRotation(hook.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, target, speed);
            if (!alertActivated) { StartCoroutine(Alert()); }
        }

        if (distance < 10f) // Fish will move towards the hook if it's even closer.
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, hook.transform.position, Time.deltaTime * speed);
        }
    }

    private void LoseInterest()
    {
        var target = Quaternion.LookRotation(originalPosition - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, target, speed / 6);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPosition, Time.deltaTime * (speed / 3));
        alertActivated = false;
    }

    private void OnTriggerEnter(Collider other) // Make sure either the fish or the other game object have a RigidBody!
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            inWater = true;
            validSpawn = true;
            if (originalPosition == Vector3.zero) { originalPosition = transform.position; originalRotation = transform.rotation; }
        }
        if (other.gameObject.name == "Bob" && !fishing.inMinigame)
        {
            onHook = true; fishing.StartMinigame(gameObject); // Starts the mini-game if close enough to the hook and the player presses E.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            inWater = false;
        }
    }

    private IEnumerator Alert() // Instantiates an exclamation icon above the fish's head when the hook is noticed.
    {
        alertActivated = true;
        GameObject prefab = Resources.Load<GameObject>("PREFABS/FishExclamation");
        GameObject alert = Instantiate(prefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        alert.transform.LookAt(Camera.main.transform);
        yield return new WaitForSeconds(3);
        Destroy(alert);
    }
}

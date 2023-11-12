using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPositionSpawner : MonoBehaviour
{
    public GameObject[] myObjects;

    //public GameObject Sphere;

    [Header("Points on where to randomly spawn the objects")]
    public float PointA;
    public float PointB;
    public float PointC;
    public float PointD;

    public float Height;

    public float waitTime = 5f;

    // Update is called once per frame
    void Start()
    {
        // Starting in 2 seconds.
        // a Seagull will be launched every 10 seconds
        InvokeRepeating("SpawnObjects", 2.0f, waitTime);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int randomIndex = Random.Range(0, myObjects.Length);
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-10, 11), Height, Random.Range(-10, 11));
            //transform.rotation = Random.rotation;

            Instantiate(myObjects[randomIndex], randomSpawnPosition, Quaternion.identity);
        }
    }

    public void SpawnObjects()
    {
        /* Debug.Log("Activated");
        float radius = 3f;
        Vector3 randomPos = Random.insideUnitSphere * radius;
        randomPos += transform.position;
        randomPos.y = 0f;
        
        Vector3 direction = randomPos - transform.position;
        direction.Normalize();
        
        float dotProduct = Vector3.Dot(transform.forward, direction);
        float dotProductAngle = Mathf.Acos(dotProduct / transform.forward.magnitude * direction.magnitude);
        
        randomPos.x = Mathf.Cos(dotProductAngle) * radius + transform.position.x;
        randomPos.z = Mathf.Sin(dotProductAngle * (Random.value > 0.5f ? 1f : -1f)) * radius + transform.position.z;
        
        GameObject go = Instantiate(_spherePrefab, randomPos, Quaternion.identity);
        go.transform.position = randomPos; */
        
        
        int randomIndex = Random.Range(0, myObjects.Length);
        Vector3 randomSpawnPosition = new Vector3(Random.Range(PointA, PointB), 100, Random.Range(PointC, PointD));
        //transform.rotation = Random.rotation;

        Instantiate(myObjects[randomIndex], randomSpawnPosition, Quaternion.identity);
    }
}

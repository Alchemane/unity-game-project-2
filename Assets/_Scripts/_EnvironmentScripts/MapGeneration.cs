using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public GameObject hexagonPrefab;

    public int mapSize;
    List<GameObject> frontier = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        GameObject originHexagon = Instantiate(hexagonPrefab, Vector3.zero, Quaternion.identity);
        frontier.Add(originHexagon);

        for (int i = 0; i < mapSize - 1; i++)
        {
            GenerateNeighbor();
        }
    }
    
    // generate hexagon from random hexagon
    void GenerateNeighbor()
    {
        GameObject currentHexagon = frontier[Random.Range(0, frontier.Count)];
        Vector3 newPosition = GetRandomNeighbor(currentHexagon);
        GameObject newHexagon = Instantiate(hexagonPrefab, newPosition, Quaternion.identity);
        frontier.Add(newHexagon);
    }

    // locate random hexagon
    Vector3 GetRandomNeighbor(GameObject hexagon)
    {
        Bounds hexagonBounds = hexagon.GetComponent<Renderer>().bounds;
        float hexagonWidth = hexagonBounds.size.x;
        float hexagonHeight = hexagonBounds.size.y;

        Vector3 currentPosition = hexagon.transform.position;

        Vector3[] possibleDirections = {
            new Vector3(hexagonWidth, 0, 0),
            new Vector3(-hexagonWidth, 0, 0),
            new Vector3(0, 0, hexagonHeight),
            new Vector3(0, 0, -hexagonHeight),
        };

        // logic here for if hexagon present in the random possible direction

        //while()

        int randomIndex = Random.Range(0, possibleDirections.Length);
        Vector3 randomDirection = possibleDirections[randomIndex];

        Vector3 newPosition = currentPosition + randomDirection;

        return newPosition;
    }
}
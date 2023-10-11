using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public GameObject hexagonPrefab;

    public int mapSize;
    List<GameObject> frontier = new List<GameObject>();
    Dictionary<Vector3, GameObject> hexagonPositions = new Dictionary<Vector3, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject originHexagon = Instantiate(hexagonPrefab, Vector3.zero, Quaternion.identity);
        frontier.Add(originHexagon);
        hexagonPositions[Vector3.zero] = originHexagon; // Initialize with origin hexagon

        for (int i = 0; i < mapSize - 1; i++)
        {
            GenerateNeighbor();
        }
    }

    // generate hexagon from random hexagon
    void GenerateNeighbor()
    {
        GameObject currentHexagon = frontier[Random.Range(0, frontier.Count)];
        Vector3 newPosition = GetRandomNeighborPosition(currentHexagon);

        // Check if the position is already occupied
        while (hexagonPositions.ContainsKey(newPosition))
        {
            // Generate a new random position
            newPosition = GetRandomNeighborPosition(currentHexagon);
        }

        GameObject newHexagon = Instantiate(hexagonPrefab, newPosition, Quaternion.identity);
        frontier.Add(newHexagon);
        hexagonPositions[newPosition] = newHexagon; // Store the new position
    }

    // locate random hexagon
    Vector3 GetRandomNeighborPosition(GameObject hexagon)
    {
        Bounds hexagonBounds = hexagon.GetComponent<Renderer>().bounds;
        float hexagonWidth = hexagonBounds.size.x;
        float hexagonHeight = hexagonBounds.size.y;

        Vector3 currentPosition = hexagon.transform.position;

        float angle = Mathf.PI / 3;  // 60 degrees in radians
        Vector3[] possibleDirections = new Vector3[6];

        for (int i = 0; i < 6; i++)
        {
            float x = Mathf.Cos(angle * i) * hexagonWidth;
            float z = Mathf.Sin(angle * i) * hexagonWidth;
            possibleDirections[i] = new Vector3(x, 0, z);
        }






        int randomIndex = Random.Range(0, possibleDirections.Length);
        Vector3 randomDirection = possibleDirections[randomIndex];

        Vector3 newPosition = currentPosition + randomDirection;

        return newPosition;
    }
}
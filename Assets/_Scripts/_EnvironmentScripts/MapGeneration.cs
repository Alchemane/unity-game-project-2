using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public GameObject hexagonPrefab;
    public int mapSize;
    List<GameObject> frontier = new List<GameObject>();
    HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    void Start()
    {
        // Place the initial hexagon
        GameObject initialHexagon = Instantiate(hexagonPrefab, Vector3.zero, Quaternion.identity);
        frontier.Add(initialHexagon);
        occupiedPositions.Add(Vector3.zero);

        // Generate the rest of the hexagons
        for (int i = 0; i < mapSize - 1; i++)
        {
            GenerateNeighbor();
        }
    }

    void GenerateNeighbor()
    {
        GameObject currentHexagon = frontier[frontier.Count - 1]; // Get the last hexagon in the frontier
        Vector3 newPosition = GetRandomNeighborPosition(currentHexagon);

        GameObject newHexagon = Instantiate(hexagonPrefab, newPosition, Quaternion.identity);
        frontier.Add(newHexagon);
        occupiedPositions.Add(newPosition);
    }

    Vector3 GetRandomNeighborPosition(GameObject hexagon)
    {
        Bounds hexagonBounds = hexagon.GetComponent<Renderer>().bounds;
        float hexagonWidth = hexagonBounds.size.x;

        Vector3 currentPosition = hexagon.transform.position;

        float angle = Mathf.PI / 3;  // 60 degrees in radians
        Vector3[] possibleDirections = new Vector3[6];

        for (int i = 0; i < 6; i++)
        {
            float x = Mathf.Cos(angle * i) * hexagonWidth;
            float z = Mathf.Sin(angle * i) * hexagonWidth;
            possibleDirections[i] = new Vector3(x, 0, z);
        }

        int randomIndex = UnityEngine.Random.Range(0, possibleDirections.Length);
        Vector3 randomDirection = possibleDirections[randomIndex];

        Vector3 newPosition = currentPosition + randomDirection;

        return newPosition;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameRoot.InGame.Environment.EnvironmentGeneration
{
    public class MapGeneration : MonoBehaviour
    {
        public GameObject hexagonPrefab;
        public int mapSize; // private when config menu set up
        readonly List<GameObject> frontier = new List<GameObject>();
        readonly HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

        private Vector3 minBounds; // map data
        private Vector3 maxBounds;

        void Start()
        {
            // place the initial hexagon
            GameObject originHexagon = Instantiate(hexagonPrefab, Vector3.zero, Quaternion.identity);
            frontier.Add(originHexagon);
            occupiedPositions.Add(Vector3.zero);

            // generate the rest of the hexagons
            for (int i = 0; i < mapSize - 1; i++)
            {
                GenerateNeighbor();
            }
        }

        void GenerateNeighbor()
        {
            int backtrackIndex = frontier.Count - 1;

            while (backtrackIndex >= 0)
            {
                GameObject currentHexagon = frontier[backtrackIndex];
                Vector3 newPosition = GetRandomNeighborPosition(currentHexagon);

                if (!occupiedPositions.Contains(newPosition))
                {
                    GameObject newHexagon = Instantiate(hexagonPrefab, newPosition, Quaternion.identity);
                    UpdateBounds(newPosition); // update map data
                    frontier.Add(newHexagon);
                    occupiedPositions.Add(newPosition);
                    break;
                }
                else
                {
                    backtrackIndex--; // backtrack to a previous neighbor for expansion
                }
            }
            if (backtrackIndex < 0)
            {
                Debug.Log("backtrackIndex < 0 error");
            }
        }
        Vector3 GetRandomNeighborPosition(GameObject hexagon)
        {
            Bounds hexagonBounds = hexagon.GetComponent<Renderer>().bounds;
            float hexagonWidth = hexagonBounds.size.x;

            Vector3 currentPosition = hexagon.transform.position;

            float angle = Mathf.PI / 3;  // 60 degrees in radians
            Vector3[] possibleDirections = new Vector3[6];

            for (int i = 0; i < possibleDirections.Length; i++)
            {
                float x = Mathf.Cos(angle * i) * hexagonWidth;
                float z = Mathf.Sin(angle * i) * hexagonWidth;
                possibleDirections[i] = new Vector3(x, 0, z);
            }

            int randomIndex = UnityEngine.Random.Range(0, possibleDirections.Length);
            Vector3 randomDirection = possibleDirections[randomIndex];
            Vector3 newPosition = currentPosition + randomDirection;

            // round the coordinates to avoid floating-point inaccuracies
            newPosition = new Vector3(
                (float)Math.Round(newPosition.x, 2),
                (float)Math.Round(newPosition.y, 2),
                (float)Math.Round(newPosition.z, 2)
            );

            return newPosition;
        }

        void UpdateBounds(Vector3 newPosition)
        {
            minBounds = new Vector3(Mathf.Min(minBounds.x, newPosition.x), 0, Mathf.Min(minBounds.z, newPosition.z));
            maxBounds = new Vector3(Mathf.Max(maxBounds.x, newPosition.x), 0, Mathf.Max(maxBounds.z, newPosition.z));
        }

        public Vector3 GetMinBounds()
        {
            return minBounds;
        }
        public Vector3 GetMaxBounds()
        {
            return maxBounds;
        }
    }
}
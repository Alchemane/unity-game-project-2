using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GameRoot.InGame.Units.Movement
{
    public class UnitMovement : MonoBehaviour
    {
        Camera mainCamera;
        NavMeshAgent navMeshAgent;
        public LayerMask ground;

        public void Start()
        {
            mainCamera = Camera.main;
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.LeftShift))
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
                {
                    navMeshAgent.SetDestination(hit.point);
                }
            }
        }
    }
}
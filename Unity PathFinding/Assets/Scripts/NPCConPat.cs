using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Assets.Script
{

    public class NPCConPat : MonoBehaviour
    {

        [SerializeField]
        bool _patrolWaiting;

        [SerializeField]
        float _totalWaitTime = 3f;

        [SerializeField]
        float _switchProbability = 0.2f;

        ConWay _currentWaypoint;

        ConWay _previousWaypoint;

        NavMeshAgent _navMeshAgent;


        bool _traveling;

        bool _patrolForward;

        float _waitTimer;

        bool _waiting;
        private int _waypointsVisited;
        private object allwaypoints;

        void Start()
        {
            _navMeshAgent = this.GetComponent<NavMeshAgent>();

            if (_navMeshAgent == null)
            {
                Debug.LogError("The Nav mesh agent is not attached to " + gameObject.name);
            }
            else
            {
                if (_currentWaypoint == null)
                {
                    GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

                    if (allWaypoints.Length > 0)
                    {
                        while (_currentWaypoint == null)
                        {
                            int random = UnityEngine.Random.Range(0, allWaypoints.Length);
                            ConWay startingWaypoint = allWaypoints[random].GetComponent<ConWay>();

                            if (startingWaypoint != null)
                            {
                                _currentWaypoint = startingWaypoint;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to find any waypoint for use.");
                    }
                }
                SetDestination();
            }
        }
        void Update()
        {
            if (_traveling && _navMeshAgent.remainingDistance <= 1.0f)
            {
                _traveling = false;
                _waypointsVisited++;

                if (_patrolWaiting)
                {
                    _waiting = true;

                    _waitTimer = 0f;
                }
                else
                {
                    SetDestination();
                }
            }

            if (_waiting)
            {
                _waitTimer += Time.deltaTime;
                if (_waitTimer >= _totalWaitTime)
                {
                    _waiting = false;

                    SetDestination();
                }
            }
        }

        private void SetDestination()
        {
            if (_waypointsVisited > 0)
            {
                ConWay nextWaypoint = _currentWaypoint.NextWaypoint(_previousWaypoint);
                _previousWaypoint = _currentWaypoint;
                _currentWaypoint = nextWaypoint;
            }
            Vector3 targetVector = _currentWaypoint.transform.position;
            _navMeshAgent.SetDestination(targetVector);
            _traveling = true;
        }
    }
}
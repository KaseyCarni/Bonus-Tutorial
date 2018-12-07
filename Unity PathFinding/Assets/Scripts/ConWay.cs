using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConWay : MonoBehaviour {
    [SerializeField]
    protected float _connectivityRadius = 50F;

    [SerializeField]
    protected float debugDrawRadius = 1.0F;

    List<ConWay> _connections;

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _connectivityRadius);
    }

    // Use this for initialization
    void Start () {
        GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        _connections = new List<ConWay>();

        for(int i = 0; i < allWaypoints.Length; i++)
        {
            ConWay nextWaypoint = allWaypoints[i].GetComponent<ConWay>();

            if(nextWaypoint != null)
            {
                if(Vector3.Distance(this.transform.position, nextWaypoint.transform.position) <= _connectivityRadius && nextWaypoint != this)
                {
                    _connections.Add(nextWaypoint);
                }
            }
        }
    }


    public ConWay NextWaypoint(ConWay previousWaypoint)
    {
        if(_connections.Count == 0)
        {
            Debug.LogError("Not Enough Waypoint in count.");
            return null;
        }
        else if(_connections.Count == 1 && _connections.Contains(previousWaypoint))
        {
            return previousWaypoint;
        }
        else
        {
            ConWay nextWaypoint;
            int nextIndex = 0;

            do
            {
                nextIndex = UnityEngine.Random.Range(0, _connections.Count);
                nextWaypoint = _connections[nextIndex];
            }
            while (nextWaypoint == previousWaypoint);

            return nextWaypoint;
        }
    }
}

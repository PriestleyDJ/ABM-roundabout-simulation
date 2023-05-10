using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABMTrafficSimulation
{
    public class LaneSegmentation : MonoBehaviour
    {
        public List<LaneSegmentation> nextLanes;
        // might need to manually get rid of what i did
        [HideInInspector] public int id;
        [HideInInspector] public List<Waypoints> waypoints;

        public bool IsInLane(Vector3 _pos)
        {
            TrafficNetwork trafficNetwork = GetComponentInParent<TrafficNetwork>();
            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                float distance1 = Vector3.Distance(waypoints[i].transform.position, _pos);
                float distance2 = Vector3.Distance(waypoints[i + 1].transform.position, _pos);
                float distance3 = Vector3.Distance(waypoints[i].transform.position, waypoints[i + 1].transform.position);
                float distance4 = (distance1 + distance2) - distance3;
                if (distance4 < trafficNetwork.laneDetection && distance4 > -trafficNetwork.laneDetection)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
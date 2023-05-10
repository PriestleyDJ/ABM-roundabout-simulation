using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABMTrafficSimulation {
    public class Waypoints : MonoBehaviour
    {
        public LaneSegmentation lane;

        public void Refresh(int _laneID, LaneSegmentation _lane)
        {
            lane = _lane;
            tag = "Waypoint";
            name = _laneID.ToString();

            if (GetComponent<SphereCollider>())
            {
                DestroyImmediate(gameObject.GetComponent<SphereCollider>());
            }
        }

        public Vector3 GetPos()
        {
            return transform.position;
        }
    }
}

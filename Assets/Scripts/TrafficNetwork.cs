using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABMTrafficSimulation 
{
    public class TrafficNetwork : MonoBehaviour
    {
        public bool showGizmos = false;
        public float segmentDetection = 0.1f;
        public float waypointSize = 0.5f;

        public List<LaneSegmentation> lanes = new List<LaneSegmentation>();
        public List<Junction> junctions = new List<Junction>();
    }
}

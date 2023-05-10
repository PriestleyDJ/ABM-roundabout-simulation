using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABMTrafficSimulation 
{
    public class LaneSegmentation : MonoBehaviour
    {
        public List<LaneSegmentation> nextLanes;

        [HideInInspector] public int id;
        [HideInInspector] public List<Waypoints> waypoints;
    }
}
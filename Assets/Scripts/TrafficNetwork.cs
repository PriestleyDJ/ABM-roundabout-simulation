using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABMTrafficSimulation 
{
    public class TrafficNetwork : MonoBehaviour
    {
        public bool showGizmos = false;
        public float laneDetection = 0.1f;
        public float waypointSize = 0.5f;
        // must change
        public ArrowDraw arrowDrawType = ArrowDraw.ByLength;
        public int arrowCount = 1;
        public float arrowDistance = 5;
        public float arrowSizeWaypoint = 1;
        public float arrowSizeIntersection = 0.5f;

        public List<LaneSegmentation> lanes = new List<LaneSegmentation>();
        public List<Junction> junctions = new List<Junction>();
        public string[] objectLayers;
        public LaneSegmentation curSegment = null;

        public List<Waypoints> GetWaypoints()
        {
            List<Waypoints> waypoints = new List<Waypoints>();

            foreach (LaneSegmentation lane in lanes)
            {
                waypoints.AddRange(lane.waypoints);
            }
            return waypoints;
        }

        public void ResumeNetwork()
        {
            Junction[] junctions = GameObject.FindObjectsOfType<Junction>();
            foreach (Junction j in junctions)
                j.JunctionResume();
        }

        public void SaveNetwork()
        {
            Junction[] junctions = GameObject.FindObjectsOfType<Junction>();
            foreach (Junction j in junctions)
                j.JunctionSnapshot();
        }
    }

    public enum ArrowDraw
    {
        FixedCount, ByLength, Off
    }
}

// Traffic Simulation
// https://github.com/mchrbn/unity-traffic-simulation

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace ABMTrafficSimulation {
    [CustomEditor(typeof(TrafficNetwork))]
    public class TrafficEditor : Editor {

        private TrafficNetwork wps;
        
        //References for moving a waypoint
        private Vector3 startPosition;
        private Vector3 lastPoint;
        private Waypoints lastWaypoint;
        
        [MenuItem("Component/Traffic Simulation/Create Traffic Objects")]
        private static void CreateTraffic(){
            EditorHelper.SetUndoGroup("Create Traffic Objects");
            
            GameObject mainGo = EditorHelper.CreateGameObject("Traffic System");
            mainGo.transform.position = Vector3.zero;
            EditorHelper.AddComponent<TrafficNetwork>(mainGo);

            GameObject segmentsGo = EditorHelper.CreateGameObject("Segments", mainGo.transform);
            segmentsGo.transform.position = Vector3.zero;

            GameObject intersectionsGo = EditorHelper.CreateGameObject("Intersections", mainGo.transform);
            intersectionsGo.transform.position = Vector3.zero;
            
            //Close Undo Operation
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        void OnEnable(){
            wps = target as TrafficNetwork;
        }

        private void OnSceneGUI() {
            Event e = Event.current;
            if (e == null) return;

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) && e.type == EventType.MouseDown && e.button == 0) {
                //Add a new waypoint on mouseclick + shift
                if (e.shift) {
                    if (wps.curSegment == null) {
                        return;
                    }

                    EditorHelper.BeginUndoGroup("Add Waypoint", wps);
                    AddWaypoint(hit.point);

                    //Close Undo Group
                    Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
                }

                //Create a segment + add a new waypoint on mouseclick + ctrl
                else if (e.control) {
                    EditorHelper.BeginUndoGroup("Add Segment", wps);
                    AddSegment(hit.point);
                    AddWaypoint(hit.point);

                    //Close Undo Group
                    Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
                }

                //Create an intersection type
                else if (e.alt) {
                    EditorHelper.BeginUndoGroup("Add Intersection", wps);
                    AddIntersection(hit.point);

                    //Close Undo Group
                    Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
                }
            }

            //Set waypoint system as the selected gameobject in hierarchy
            Selection.activeGameObject = wps.gameObject;

            //Handle the selected waypoint
            if (lastWaypoint != null) {
                //Uses a endless plain for the ray to hit
                Plane plane = new Plane(Vector3.up.normalized, lastWaypoint.GetPos());
                plane.Raycast(ray, out float dst);
                Vector3 hitPoint = ray.GetPoint(dst);

                //Reset lastPoint if the mouse button is pressed down the first time
                if (e.type == EventType.MouseDown && e.button == 0) {
                    lastPoint = hitPoint;
                    startPosition = lastWaypoint.transform.position;
                }

                //Move the selected waypoint
                if (e.type == EventType.MouseDrag && e.button == 0) {
                    Vector3 realDPos = new Vector3(hitPoint.x - lastPoint.x, 0, hitPoint.z - lastPoint.z);

                    lastWaypoint.transform.position += realDPos;
                    lastPoint = hitPoint;
                }

                //Release the selected waypoint
                if (e.type == EventType.MouseUp && e.button == 0) {
                    Vector3 curPos = lastWaypoint.transform.position;
                    lastWaypoint.transform.position = startPosition;
                    Undo.RegisterFullObjectHierarchyUndo(lastWaypoint, "Move Waypoint");
                    lastWaypoint.transform.position = curPos;
                }

                //Draw a Sphere
                Handles.SphereHandleCap(0, lastWaypoint.GetPos(), Quaternion.identity, wps.waypointSize * 2f, EventType.Repaint);
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                SceneView.RepaintAll();
            }

            //Set the current hovering waypoint
            if (lastWaypoint == null) {
                lastWaypoint = wps.GetWaypoints().FirstOrDefault(i => EditorHelper.SphereHit(i.GetPos(), wps.waypointSize, ray));
            }

            //Update the current segment to the currently interacting one
            if (lastWaypoint != null && e.type == EventType.MouseDown) {
                wps.curSegment = lastWaypoint.lane;
            }
            
            //Reset current waypoint
            else if (lastWaypoint != null && e.type == EventType.MouseMove) {
                lastWaypoint = null;
            }
        }

        public override void OnInspectorGUI() {
            EditorGUI.BeginChangeCheck();

            //Register an Undo if changes are made after this call
            Undo.RecordObject(wps, "Traffic Inspector Edit");

            //Draw the Inspector
            TrafficEditorInspector.DrawInspector(wps, serializedObject, out bool restructureSystem);

            //Rename waypoints if some have been deleted
            if (restructureSystem) {
                RestructureSystem();
            }

            //Repaint the scene if values have been edited
            if (EditorGUI.EndChangeCheck()) {
                SceneView.RepaintAll();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void AddWaypoint(Vector3 position) {
            GameObject go = EditorHelper.CreateGameObject("Waypoint-" + wps.curSegment.waypoints.Count, wps.curSegment.transform);
            go.transform.position = position;

            Waypoints wp = EditorHelper.AddComponent<Waypoints>(go);
            wp.Refresh(wps.curSegment.waypoints.Count, wps.curSegment);

            //Record changes to the TrafficSystem (string not relevant here)
            Undo.RecordObject(wps.curSegment, "");
            wps.curSegment.waypoints.Add(wp);
        }

        private void AddSegment(Vector3 position) {
            int segId = wps.lanes.Count;
            GameObject segGo = EditorHelper.CreateGameObject("Segment-" + segId, wps.transform.GetChild(0).transform);
            segGo.transform.position = position;

            wps.curSegment = EditorHelper.AddComponent<LaneSegmentation>(segGo);
            wps.curSegment.id = segId;
            wps.curSegment.waypoints = new List<Waypoints>();
            wps.curSegment.nextLanes = new List<LaneSegmentation>();

            //Record changes to the TrafficSystem (string not relevant here)
            Undo.RecordObject(wps, "");
            wps.lanes.Add(wps.curSegment);
        }

        private void AddIntersection(Vector3 position) {
            int intId = wps.junctions.Count;
            GameObject intGo = EditorHelper.CreateGameObject("Intersection-" + intId, wps.transform.GetChild(1).transform);
            intGo.transform.position = position;

            BoxCollider bc = EditorHelper.AddComponent<BoxCollider>(intGo);
            bc.isTrigger = true;
            Junction intersection = EditorHelper.AddComponent<Junction>(intGo);
            intersection.id = intId;

            //Record changes to the TrafficSystem (string not relevant here)
            Undo.RecordObject(wps, "");
            wps.junctions.Add(intersection);
        }

        void RestructureSystem(){
            //Rename and restructure segments and waypoints
            List<LaneSegmentation> nSegments = new List<LaneSegmentation>();
            int itSeg = 0;
            foreach(Transform tS in wps.transform.GetChild(0).transform){
                LaneSegmentation segment = tS.GetComponent<LaneSegmentation>();
                if(segment != null){
                    List<Waypoints> nWaypoints = new List<Waypoints>();
                    segment.id = itSeg;
                    segment.gameObject.name = "Segment-" + itSeg;
                    
                    int itWp = 0;
                    foreach(Transform tW in segment.gameObject.transform){
                        Waypoints waypoint = tW.GetComponent<Waypoints>();
                        if(waypoint != null) {
                            waypoint.Refresh(itWp, segment);
                            nWaypoints.Add(waypoint);
                            itWp++;
                        }
                    }

                    segment.waypoints = nWaypoints;
                    nSegments.Add(segment);
                    itSeg++;
                }
            }

            //Check if next segments still exist
            foreach(LaneSegmentation segment in nSegments){
                List<LaneSegmentation> nNextSegments = new List<LaneSegmentation>();
                foreach(LaneSegmentation nextSeg in segment.nextLanes){
                    if(nextSeg != null){
                        nNextSegments.Add(nextSeg);
                    }
                }
                segment.nextLanes = nNextSegments;
            }
            wps.lanes = nSegments;

            //Check intersections
            List<Junction> nIntersections = new List<Junction>();
            int itInter = 0;
            foreach(Transform tI in wps.transform.GetChild(1).transform){
                Junction intersection = tI.GetComponent<Junction>();
                if(intersection != null){
                    intersection.id = itInter;
                    intersection.gameObject.name = "Intersection-" + itInter;
                    nIntersections.Add(intersection);
                    itInter++;
                }
            }
            wps.junctions = nIntersections;
            
            //Tell Unity that something changed and the scene has to be saved
            if (!EditorUtility.IsDirty(target)) {
                EditorUtility.SetDirty(target);
            }

            Debug.Log("[Traffic Simulation] Successfully rebuilt the traffic system.");
        }
    }
}

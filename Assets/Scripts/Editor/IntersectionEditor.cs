// Traffic Simulation
// https://github.com/mchrbn/unity-traffic-simulation

using UnityEngine;
using UnityEditor;

namespace ABMTrafficSimulation {
    [CustomEditor(typeof(Junction))]
    public class IntersectionEditor : Editor
    {
        private Junction intersection;

        void OnEnable(){
            intersection = target as Junction;
        }

        public override void OnInspectorGUI(){
            intersection.junctionState = (JunctionState) EditorGUILayout.EnumPopup("Intersection type", intersection.junctionState);

            EditorGUI.BeginDisabledGroup(intersection.junctionState != JunctionState.WAIT);

            EditorGUILayout.LabelField("Stop", EditorStyles.boldLabel);
            SerializedProperty sPrioritySegments = serializedObject.FindProperty("prioritySegments");
            EditorGUILayout.PropertyField(sPrioritySegments, new GUIContent("Priority Segments"), true);
            serializedObject.ApplyModifiedProperties();

            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(intersection.junctionState != JunctionState.TRAFFIC_LIGHTS);

            EditorGUILayout.LabelField("Traffic Lights", EditorStyles.boldLabel);
            intersection.lightsDuration = EditorGUILayout.FloatField("Light Duration (in s.)", intersection.lightsDuration);
            intersection.amberDuration = EditorGUILayout.FloatField("Orange Light Duration (in s.)", intersection.amberDuration);
            SerializedProperty sLightsNbr1 = serializedObject.FindProperty("lightsNbr1");
            SerializedProperty sLightsNbr2 = serializedObject.FindProperty("lightsNbr2");
            EditorGUILayout.PropertyField(sLightsNbr1, new GUIContent("Lights #1 (first to be red)"), true);
            EditorGUILayout.PropertyField(sLightsNbr2, new GUIContent("Lights #2"), true);
            serializedObject.ApplyModifiedProperties();
            
            EditorGUI.EndDisabledGroup();
        }
    }
}

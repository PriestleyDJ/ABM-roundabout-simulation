﻿// Traffic Simulation
// https://github.com/mchrbn/unity-traffic-simulation

using UnityEngine;
using UnityEditor;

namespace ABMTrafficSimulation{
    public class VehicleEditor : Editor
    {
        [MenuItem("Component/Traffic Simulation/Setup Vehicle")]
        private static void SetupVehicle(){
            EditorHelper.SetUndoGroup("Setup Vehicle");

            GameObject selected = Selection.activeGameObject;

            //Create raycast anchor
            GameObject anchor = EditorHelper.CreateGameObject("Raycast Anchor", selected.transform);

            //Add AI scripts
            VehicleAgent vehicleAgent = EditorHelper.AddComponent<VehicleAgent>(selected);
            Wheels wheelDrive = EditorHelper.AddComponent<Wheels>(selected);

            TrafficNetwork ts = GameObject.FindObjectOfType<TrafficNetwork>();

            //Configure the vehicle AI script with created objects
            anchor.transform.localPosition = Vector3.zero;
            anchor.transform.localRotation = Quaternion.Euler(Vector3.zero);
            vehicleAgent.raycastPos = anchor.transform;

            if(ts != null) vehicleAgent.trafficNetwork = ts;

            //Create layer AutonomousVehicle if it doesn't exist
            EditorHelper.CreateLayer("AutonomousVehicle");
            
            //Set the tag and layer name
            selected.tag = "AutonomousVehicle";
            EditorHelper.SetLayer(selected, LayerMask.NameToLayer("AutonomousVehicle"), true);
        }
    }
}
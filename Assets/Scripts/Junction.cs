using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABMTrafficSimulation
{
    public enum JunctionState
    {
        WAIT,
        TRAFFIC_LIGHTS
    }

    public class Junction : MonoBehaviour
    {
        public int id;
        public JunctionState junctionState;
        public List<LaneSegmentation> lanes;
        public float lightsDuration = 15;
        public float amberDuration = 3;
        public List<LaneSegmentation> lanesToLights;
        public bool redLight = true;

        List<GameObject> junctionQueue;
        List<GameObject> junctionVehicles;
        TrafficNetwork trafficNetwork;
        

        // Start is called before the first frame update
        private void Start()
        {
            if (junctionState == JunctionState.TRAFFIC_LIGHTS)
            {
                InvokeRepeating("ChangeTrafficLight", lightsDuration, lightsDuration);
            }
            junctionQueue = new List<GameObject>();
            junctionVehicles = new List<GameObject>();            
        }
        private void ChangeTrafficLight()
        {
            if (redLight == true)
            {
                redLight = false;
            }
            else if (redLight == false)
            {
                redLight = true;
            }
            Invoke("AdjustJunctionQueue", amberDuration);            
        }

        private bool IsInJunction(GameObject _target)
        {
            foreach (GameObject vehicle in junctionVehicles)
            {
                if (vehicle.GetInstanceID() == _target.GetInstanceID())
                {
                    return true;
                }
            }
            foreach (GameObject vehicle in junctionQueue)
            {
                if (vehicle.GetInstanceID() == _target.GetInstanceID())
                {
                    return true;
                }
            }
            return false;
        }

        private void BeginWait(GameObject _vehicleAgent)
        {
            VehicleAgent vehicleAgent = _vehicleAgent.GetComponent<VehicleAgent>();

            if (IsLane(vehicleAgent.GetCurrentLane()))
            {
                vehicleAgent.vehicleState = MovementState.DECCELERATE;
                junctionVehicles.Add(_vehicleAgent);
            }
            else
            {
                if (junctionQueue.Count > 0 || junctionVehicles.Count > 0)
                {
                    vehicleAgent.vehicleState = MovementState.STOP;
                    junctionQueue.Add(_vehicleAgent);
                }
                else
                {
                    junctionVehicles.Add(_vehicleAgent);
                    vehicleAgent.vehicleState = MovementState.DECCELERATE;                   
                }
            }
        }

        private void StopWait(GameObject _vehicleAgent)
        {
            _vehicleAgent.GetComponent<VehicleAgent>().vehicleState = MovementState.GO;            
            junctionVehicles.Remove(_vehicleAgent);
            junctionQueue.Remove(_vehicleAgent);

            if (junctionQueue.Count > 0 && junctionVehicles.Count == 0)
            {
                junctionQueue[0].GetComponent<VehicleAgent>().vehicleState = MovementState.GO;
            }
        }

        // don't need 
        private bool IsLane(int _vehicleLane)
        {
            foreach (LaneSegmentation lane in lanes)
            {
                if (_vehicleLane == lane.id)
                {
                    return true;
                }                    
            }
            return false;
        }

        private void WaitTrafficLight(GameObject _vehicleAgent)
        {
            VehicleAgent vehicleAgent = _vehicleAgent.GetComponent<VehicleAgent>();           
            if (IsEnterRedLight(vehicleAgent.GetCurrentLane()))
            {
                vehicleAgent.vehicleState = MovementState.STOP;
                junctionQueue.Add(_vehicleAgent);
            }
            else
            {
                vehicleAgent.vehicleState = MovementState.GO;
            }
        }

        private bool IsEnterRedLight(int _vehicleLane)
        {
            if (redLight)
            {
                foreach (LaneSegmentation lane in lanesToLights)
                {
                    if (lane.id == _vehicleLane)
                    {
                        return true;
                    }                       
                }
            }
            return false;
        }        

        void AdjustJunctionQueue()
        {            
            List<GameObject> junctionQueues = new List<GameObject>(junctionQueue);
            foreach (GameObject vehicleAgent in junctionQueue)
            {           
                if (!IsEnterRedLight(vehicleAgent.GetComponent<VehicleAgent>().GetCurrentLane()))
                {
                    vehicleAgent.GetComponent<VehicleAgent>().vehicleState = MovementState.GO;
                    junctionQueues.Remove(vehicleAgent);
                }
            }
            junctionQueue = junctionQueues;
        }

        List<GameObject> oldJunctionQueue = new List<GameObject>();
        List<GameObject> oldJunctionVehicles = new List<GameObject>();

        public void JunctionSnapshot()
        {
            oldJunctionQueue = junctionQueue;
            oldJunctionVehicles = junctionVehicles;
        }

        public void JunctionResume()
        {
            foreach (GameObject vehicleAgent in junctionVehicles)
            {
                foreach (GameObject oldVehicleAgent in oldJunctionVehicles)
                {
                    if (vehicleAgent.GetInstanceID() == oldVehicleAgent.GetInstanceID())
                    {
                        vehicleAgent.GetComponent<VehicleAgent>().vehicleState = oldVehicleAgent.GetComponent<VehicleAgent>().vehicleState;
                        break;
                    }
                }
            }
            foreach (GameObject vehicleAgent in junctionQueue)
            {
                foreach (GameObject oldVehicleAgent in oldJunctionQueue)
                {
                    if (vehicleAgent.GetInstanceID() == oldVehicleAgent.GetInstanceID())
                    {
                        vehicleAgent.GetComponent<VehicleAgent>().vehicleState = oldVehicleAgent.GetComponent<VehicleAgent>().vehicleState;
                        break;
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider _other)
        {
            if (IsInJunction(_other.gameObject) || Time.timeSinceLevelLoad < .5f)
            {
                return;
            }

            if (_other.tag == "VehicleAgent" && junctionState == JunctionState.WAIT)
            {
                BeginWait(_other.gameObject);
            }
            else if (_other.tag == "VehicleAgent" && junctionState == JunctionState.TRAFFIC_LIGHTS)
            {
                WaitTrafficLight(_other.gameObject);
            }
        }

        private void OnTriggerExit(Collider _other)
        {
            if (_other.tag == "VehicleAgent" && junctionState == JunctionState.WAIT)
            {
                StopWait(_other.gameObject);
            }                
            else if (_other.tag == "VehicleAgent" && junctionState == JunctionState.TRAFFIC_LIGHTS)
            {
                _other.gameObject.GetComponent<VehicleAgent>().vehicleState = MovementState.GO;
            }                
        }
    }
}
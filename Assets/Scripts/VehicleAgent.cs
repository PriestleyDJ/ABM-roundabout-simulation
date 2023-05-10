using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABMTrafficSimulation
{
    public struct MovementTarget
    {
        public int lane;
        public int waypoint;
    }
    public enum MovementState
    {
        GO,
        STOP,
        DECCELERATE
    }
    public class VehicleAgent : MonoBehaviour
    {
        public TrafficNetwork trafficNetwork;
        public Transform raycastPos;
        public float raycastLength = 3;
        public int raycastDistance = 6;
        public int raycastNumber = 10;
        public float brakeDistance = 0.5f;
        public float deccelerationDistance = 5f;

        public MovementState vehicleState = MovementState.GO;
        MovementTarget currentMovementTarget;
        MovementTarget nextMovementTarget;
        Wheels wheels;
        float waypointThreshold = 5;
        float maxSpeed = 0;
        int prevMovementTarget = -1;

        // Start is called before the first frame update
        private void Start()
        {
            wheels = this.GetComponent<Wheels>();
            if (trafficNetwork != null)
            {
                maxSpeed = wheels.maxSpeed;
                SetCurrentWaypoint();
            }
        }

        private void Update()
        {
            if (trafficNetwork != null)
            {
                FindWaypoints();
                DriveVehicleAgent();
            }                           
        }

        private void FindWaypoints()
        {
            GameObject currentWaypoint = trafficNetwork.lanes[currentMovementTarget.lane].waypoints[currentMovementTarget.waypoint].gameObject;
            Vector3 nextWaypointPos = transform.InverseTransformPoint(new Vector3(currentWaypoint.transform.position.x, transform.position.y, currentWaypoint.transform.position.z));
            
            if (nextWaypointPos.magnitude < waypointThreshold)
            {
                currentMovementTarget.waypoint++;                
                if (currentMovementTarget.waypoint >= trafficNetwork.lanes[currentMovementTarget.lane].waypoints.Count)
                {
                    prevMovementTarget = currentMovementTarget.lane;
                    currentMovementTarget.lane = nextMovementTarget.lane;
                    currentMovementTarget.waypoint = 0;
                }
               
                nextMovementTarget.waypoint = 1 + currentMovementTarget.waypoint;
                if (nextMovementTarget.waypoint >= trafficNetwork.lanes[currentMovementTarget.lane].waypoints.Count)
                {
                    nextMovementTarget.waypoint = 0;
                    nextMovementTarget.lane = GetNextLane();
                }
            }
        }

        private int GetNextLane()
        {
            if (trafficNetwork.lanes[currentMovementTarget.lane].nextLanes.Count != 0)
            {
                int laneID = Random.Range(0, trafficNetwork.lanes[currentMovementTarget.lane].nextLanes.Count);
                int nextLaneID = trafficNetwork.lanes[currentMovementTarget.lane].nextLanes[laneID].id;
                return nextLaneID;
            }
            else
            {
                return 0;
            }
        }

        private void DriveVehicleAgent()
        {
            Vector3 nextVelocity = trafficNetwork.lanes[nextMovementTarget.lane].waypoints[nextMovementTarget.waypoint].transform.position - trafficNetwork.lanes[currentMovementTarget.lane].waypoints[currentMovementTarget.waypoint].transform.position;
            float nextTurn = Mathf.Clamp(transform.InverseTransformDirection(nextVelocity.normalized).x, -1, 1);            
            wheels.maxSpeed = maxSpeed;
            float turning = 0f;
            float braking = 0f;
            float acceleration = 1f;

            if (vehicleState == MovementState.STOP)
            {                
                braking = 1f;
                acceleration = 0f;
                wheels.maxSpeed = Mathf.Min(wheels.maxSpeed / 2f, 5f); ;
            }
            else
            {
                if (vehicleState == MovementState.DECCELERATE)
                {
                    braking = 0f;
                    acceleration = 0.3f;
                }
                if (nextTurn < -0.3f || nextTurn > 0.3f)
                {
                    wheels.maxSpeed = Mathf.Min(wheels.maxSpeed, 8f); ;
                }
            }
            
            float hitDistance;
            GameObject detectedObject = GetRayDetection(out hitDistance);

            if(detectedObject != null)
            {
                Wheels detectedVehiclAgent = null;
                detectedVehiclAgent = detectedObject.GetComponent<Wheels>();

                ///////////////////////////////////////////////////////////////
                //Differenciate between other vehicles AI and generic obstacles (including controlled vehicle, if any)
                if (detectedVehiclAgent != null)
                {
                    //Check if it's front vehicle
                    float dotFront = Vector3.Dot(this.transform.forward, detectedVehiclAgent.transform.forward);

                    //If detected front vehicle max speed is lower than ego vehicle, then decrease ego vehicle max speed
                    if (detectedVehiclAgent.maxSpeed < 30f && dotFront > .8f)
                    {
                        float ms = Mathf.Max((detectedVehiclAgent.maxSpeed / 2.237f) - .5f, .1f);
                        wheels.maxSpeed = ms * 2.237f;
                    }

                    //If the two vehicles are too close, and facing the same direction, brake the ego vehicle
                    if (hitDistance <  brakeDistance && dotFront > .8f)
                    {
                        acceleration = 0;
                        braking = 1;
                        wheels.maxSpeed = Mathf.Max(wheels.maxSpeed / 2f, wheels.minSpeed);
                    }

                    //If the two vehicles are too close, and not facing same direction, slight make the ego vehicle go backward
                    else if (hitDistance < brakeDistance && dotFront <= .8f)
                    {
                        acceleration = -.3f;
                        braking = 0f;
                        wheels.maxSpeed = Mathf.Max(wheels.maxSpeed / 2f, wheels.minSpeed);

                        //Check if the vehicle we are close to is located on the right or left then apply according steering to try to make it move
                        float dotRight = Vector3.Dot(this.transform.forward, detectedVehiclAgent.transform.right);
                        //Right
                        if (dotRight > 0.1f) turning = -.3f;
                        //Left
                        else if (dotRight < -0.1f) turning = .3f;
                        //Middle
                        else turning = -.7f;
                    }

                    //If the two vehicles are getting close, slow down their speed
                    else if (hitDistance < deccelerationDistance)
                    {
                        acceleration = .5f;
                        braking = 0f;
                        //wheelDrive.maxSpeed = Mathf.Max(wheelDrive.maxSpeed / 1.5f, wheelDrive.minSpeed);
                    }
                }

                ///////////////////////////////////////////////////////////////////
                // Generic obstacles
                else
                {
                    //Emergency brake if getting too close
                    if (hitDistance < brakeDistance)
                    {
                        acceleration = 0;
                        braking = 1;
                        wheels.maxSpeed = Mathf.Max(wheels.maxSpeed / 2f, wheels.minSpeed);
                    }

                    //Otherwise if getting relatively close decrease speed
                    else if (hitDistance < deccelerationDistance)
                    {
                        acceleration = .5f;
                        braking = 0f;
                    }
                }
            }

            //Check if we need to steer to follow path
            if (acceleration > 0f)
            {
                Vector3 desiredVel = trafficNetwork.lanes[currentMovementTarget.lane].waypoints[currentMovementTarget.waypoint].transform.position - transform.position;
                turning = Mathf.Clamp(transform.InverseTransformDirection(desiredVel.normalized).x, -1f, 1f);
            }               

            wheels.Drive(turning, acceleration, braking);
        }

        GameObject GetRayDetection(out float _contactDistance)
        {
            GameObject detectedGO = null;
            float minDistance = 500f;

            float raycast = (raycastNumber / 2f) * raycastDistance;
            float contactDistance = -1f;
            for (float i = -raycast; i <= raycast; i += raycastDistance)
            {
                RayCast(raycastPos.transform.position, i, transform.forward, raycastLength, out detectedGO, out contactDistance);

                if (detectedGO != null)
                {
                    float distance1 = Vector3.Distance(transform.position, detectedGO.transform.position);
                    if (distance1 < minDistance)
                    {
                        minDistance = distance1;
                        break;
                    }
                }                
            }
            _contactDistance = contactDistance;
            return detectedGO;
        }


        void RayCast(Vector3 _position, float _angle, Vector3 _direction, float _length, out GameObject _outObstacle, out float _outContactDistance)
        {
            _outObstacle = null;
            _outContactDistance = -1f;         
            
            Debug.DrawRay(_position, Quaternion.Euler(0, _angle, 0) * _direction * _length, new Color(0, 1, 0, 0.8f));            
            int agentLayer = 1 << LayerMask.NameToLayer("VehicleAgent");
            int finalMask = agentLayer;

            foreach (string layer in trafficNetwork.objectLayers)
            {
                int id = 1 << LayerMask.NameToLayer(layer);
                finalMask = finalMask | id;
            }

            RaycastHit contact;
            if (Physics.Raycast(_position, Quaternion.Euler(0, _angle, 0) * _direction, out contact, _length, finalMask))
            {
                _outObstacle = contact.collider.gameObject;
                _outContactDistance = contact.distance;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Out Of Bounds")
            {
                Destroy(this.gameObject);
            }
        }

        public int GetCurrentLane()
        {
            int lane = currentMovementTarget.lane;
            if (!trafficNetwork.lanes[currentMovementTarget.lane].IsInLane(transform.position))
            {
                if(trafficNetwork.lanes[prevMovementTarget].IsInLane(transform.position))
                {
                    lane =  prevMovementTarget;
                }                                        
            }
            Debug.Log("Lane: " + lane);
            return lane;
        }

        private void SetCurrentWaypoint()
        {
            foreach (LaneSegmentation lane in trafficNetwork.lanes)
            {
                if (lane.IsInLane(transform.position))
                {
                    float minDistance = 1000000000000000000000000000000f;
                    currentMovementTarget.lane = lane.id;
                    for (int i = 0; i < trafficNetwork.lanes[currentMovementTarget.lane].waypoints.Count; i++)
                    {
                        float distance1 = Vector3.Distance(transform.position, trafficNetwork.lanes[currentMovementTarget.lane].waypoints[i].transform.position);
                        Vector3 localPos = transform.InverseTransformPoint(trafficNetwork.lanes[currentMovementTarget.lane].waypoints[i].transform.position);
                        if (distance1 < minDistance && localPos.z > 0)
                        {
                            minDistance = distance1;
                            currentMovementTarget.waypoint = i;
                        }
                    }
                    break;
                }
            }


            //Get future target
            nextMovementTarget.waypoint = currentMovementTarget.waypoint + 1;
            nextMovementTarget.lane = currentMovementTarget.lane;

            if (nextMovementTarget.waypoint >= trafficNetwork.lanes[currentMovementTarget.lane].waypoints.Count)
            {
                nextMovementTarget.waypoint = 0;
                nextMovementTarget.lane = GetNextLane();
            }
        }
    }
}

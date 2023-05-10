using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABMTrafficSimulation
{
    public class Wheels : MonoBehaviour
    {
        public GameObject wheelL;
        public GameObject wheelR;
        public float maxSpeed = 30;
        public float minSpeed = 1;

        WheelCollider[] wheelColliders;
        float currentTurning = 0f;

        private void OnEnable()
        {
            wheelColliders = GetComponentsInChildren<WheelCollider>();
            int i = 0;
            while (i < wheelColliders.Length)
            {
                var wheelCollider = wheelColliders[i];
                if (wheelCollider.transform.localPosition.x < 0 && wheelL != null)
                {
                    var wheel = Instantiate(wheelL);
                    wheel.transform.parent = wheelCollider.transform;
                }
                if (wheelCollider.transform.localPosition.x > 0 && wheelR != null)
                {
                    var wheel = Instantiate(wheelR);
                    wheel.transform.parent = wheelCollider.transform;
                }
                wheelCollider.ConfigureVehicleSubsteps(10, 1, 1);
                i++;
            }
        }

        public void Drive(float _turning, float _acceleration, float _brake)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            currentTurning = Mathf.Lerp(currentTurning, _turning, Time.deltaTime * 5);
            foreach (WheelCollider wheelCollider in wheelColliders)
            {
                float localZPos = wheelCollider.transform.localPosition.z;
                if (localZPos >= 0)
                {
                    if (_brake > 0)
                    {
                        wheelCollider.brakeTorque = 100000;
                    }
                    else
                    {
                        wheelCollider.brakeTorque = 0;
                    }                    
                }
                if (localZPos < 0)
                {
                    wheelCollider.steerAngle = 30 * currentTurning;
                }
                wheelCollider.motorTorque = 100 * _acceleration;
            }

            if (rb.velocity.magnitude * 2.237f > maxSpeed)
            {
                rb.velocity = maxSpeed * 2.237f * rb.velocity.normalized;
            }
            rb.AddForce(-transform.up * 100 * rb.velocity.magnitude);
        }
    }
}

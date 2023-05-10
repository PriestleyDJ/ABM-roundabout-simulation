using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ABMTrafficSimulation;

public class RedLightStatus : MonoBehaviour
{    
    public Junction junction;
    
    Light pointLight;

    void Start(){
        pointLight = this.transform.GetChild(0).GetComponent<Light>();
        SetTrafficLightColor();
    }

    // Update is called once per frame
    void Update(){
        SetTrafficLightColor();
    }

    void SetTrafficLightColor(){
        if(junction.redLight)
            pointLight.color = new Color(1, 0, 0);
        else
            pointLight.color = new Color(0, 1, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TrafficSimulation
{
    public class SpawnVehicle : MonoBehaviour
    {
        public GameObject carPrefab;
        public GameObject cars;
        public TrafficSystem trafficSystem;

        [Tooltip("Broad lane vehicle seconds per spawn")]
        [SerializeField] public float broadLaneSPS = 5;
        [Tooltip("Hanover street vehicle seconds per spawn")]
        [SerializeField] public float hanoverStreetSPS = 5;
        [Tooltip("Brook Hill vehicle seconds per spawn")]
        [SerializeField] public float brookHillSPS = 5;
        [Tooltip("Bolsover street vehicle seconds per spawn")]
        [SerializeField] public float bolsoverStreetSPS = 5;
        [Tooltip("Netherthorpe road vehicle seconds per spawn")]
        [SerializeField] public float netherthorpeRoadSPS = 5;

        [HideInInspector, SerializeField]  List<Waypoint> broadLane = new List<Waypoint>();
        [HideInInspector, SerializeField]  List<Waypoint> hanover = new List<Waypoint>();
        [HideInInspector, SerializeField]  List<Waypoint> brookHill = new List<Waypoint>();
        [HideInInspector, SerializeField]  List<Waypoint> bolsover = new List<Waypoint>();
        [HideInInspector, SerializeField]  List<Waypoint> netherthorpe = new List<Waypoint>();

        private float tempBroadLaneSPS;
        private float tempHanoverStreetSPS;
        private float tempBrookHillSPS;
        private float tempBolsoverStreetSPS;
        private float tempNetherthorpeRoadSPS;

        bool showSpawnpoints = false;

        private void Start()
        {
            tempBroadLaneSPS = 0;
            tempHanoverStreetSPS = 0;
            tempBrookHillSPS = 0;
            tempBolsoverStreetSPS = 0;
            tempNetherthorpeRoadSPS = 0;

            GameObject car1 = Instantiate(carPrefab, broadLane[0].transform.parent.gameObject.transform.position, Quaternion.Euler(0, -90, 0), cars.transform);
            car1.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
            GameObject car2 = Instantiate(carPrefab, broadLane[1].transform.parent.gameObject.transform.position, Quaternion.Euler(0, -90, 0), cars.transform);
            car2.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
            GameObject car3 = Instantiate(carPrefab, hanover[0].transform.parent.gameObject.transform.position, Quaternion.identity, cars.transform);
            car3.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
            GameObject car4 = Instantiate(carPrefab, hanover[1].transform.parent.gameObject.transform.position, Quaternion.identity, cars.transform);
            car4.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
            GameObject car5 = Instantiate(carPrefab, brookHill[0].transform.parent.gameObject.transform.position, Quaternion.identity, cars.transform);
            car5.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
            GameObject car6 = Instantiate(carPrefab, brookHill[1].transform.parent.gameObject.transform.position, Quaternion.identity, cars.transform);
            car6.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
            GameObject car7 = Instantiate(carPrefab, bolsover[0].transform.parent.gameObject.transform.position, Quaternion.identity, cars.transform);
            car7.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
            GameObject car8 = Instantiate(carPrefab, bolsover[1].transform.parent.gameObject.transform.position, Quaternion.identity, cars.transform);
            car8.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
            GameObject car9 = Instantiate(carPrefab, netherthorpe[0].transform.parent.gameObject.transform.position, Quaternion.identity, cars.transform);
            car9.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
            GameObject car10 = Instantiate(carPrefab, netherthorpe[1].transform.parent.gameObject.transform.position, Quaternion.identity, cars.transform);
            car10.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
        }

        private void Update()
        {
            tempBroadLaneSPS += Time.deltaTime;
            tempHanoverStreetSPS += Time.deltaTime;
            tempBrookHillSPS += Time.deltaTime;
            tempBolsoverStreetSPS += Time.deltaTime;
            tempNetherthorpeRoadSPS += Time.deltaTime;

            if (tempBroadLaneSPS >= broadLaneSPS)
            {
                tempBroadLaneSPS = 0;
                GameObject car1 = Instantiate(carPrefab, broadLane[0].transform.parent.gameObject.transform.position, Quaternion.Euler(0, -90, 0), cars.transform);
                car1.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
                GameObject car2 = Instantiate(carPrefab, broadLane[1].transform.parent.gameObject.transform.position, Quaternion.Euler(0, -90, 0), cars.transform);
                car2.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
            }
            if (tempHanoverStreetSPS >= hanoverStreetSPS)
            {
                tempHanoverStreetSPS = 0;
                GameObject car3 = Instantiate(carPrefab, hanover[0].transform.parent.gameObject.transform.position, Quaternion.identity, cars.transform);
                car3.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
                GameObject car4 = Instantiate(carPrefab, hanover[1].transform.parent.gameObject.transform.position, Quaternion.identity, cars.transform);
                car4.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
            }
            if (tempBrookHillSPS >= brookHillSPS)
            {
                tempBrookHillSPS = 0;
                GameObject car5 = Instantiate(carPrefab, brookHill[0].transform.parent.gameObject.transform.position, Quaternion.identity, cars.transform);
                car5.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
                GameObject car6 = Instantiate(carPrefab, brookHill[1].transform.parent.gameObject.transform.position, Quaternion.identity, cars.transform);
                car6.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
            }
            if (tempBolsoverStreetSPS >= bolsoverStreetSPS)
            {
                tempBolsoverStreetSPS = 0;
                GameObject car7 = Instantiate(carPrefab, bolsover[0].transform.parent.gameObject.transform.position, Quaternion.identity, cars.transform);
                car7.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
                GameObject car8 = Instantiate(carPrefab, bolsover[1].transform.parent.gameObject.transform.position, Quaternion.identity, cars.transform);
                car8.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
            }
            if (tempNetherthorpeRoadSPS >= netherthorpeRoadSPS)
            {
                tempNetherthorpeRoadSPS = 0;
                GameObject car9 = Instantiate(carPrefab, netherthorpe[0].transform.parent.gameObject.transform.position, Quaternion.identity, cars.transform);
                car9.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
                GameObject car10 = Instantiate(carPrefab, netherthorpe[1].transform.parent.gameObject.transform.position, Quaternion.identity, cars.transform);
                car10.GetComponent<VehicleAI>().trafficSystem = trafficSystem;
            }
        }

        #region Editor
#if UNITY_EDITOR

        [CustomEditor(typeof(SpawnVehicle))]
        public class SpawnVehicleEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                serializedObject.Update();
                base.OnInspectorGUI();

                SpawnVehicle spawnVehicle = (SpawnVehicle)target;

                EditorGUILayout.Space();                

                spawnVehicle.showSpawnpoints = EditorGUILayout.Foldout(spawnVehicle.showSpawnpoints, "Spawn Points", true);

                if (spawnVehicle.showSpawnpoints)
                {
                    EditorGUILayout.LabelField("Broad Lane");
                    EditorGUI.indentLevel++;
                    List<Waypoint> broadLane = spawnVehicle.broadLane;
                    int size1 = Mathf.Max(0, EditorGUILayout.IntField("Size", broadLane.Count));

                    while (size1 > broadLane.Count)
                    {
                        broadLane.Add(null);
                    }

                    while (size1 < broadLane.Count)
                    {
                        broadLane.RemoveAt(broadLane.Count - 1);
                    }

                    for (int i = 0; i < broadLane.Count; i++)
                    {
                        broadLane[i] = EditorGUILayout.ObjectField("Spawn point " + i, broadLane[i], typeof(Waypoint), true) as Waypoint;
                    }
                    EditorGUI.indentLevel--;

                    EditorGUILayout.LabelField("Upper Hanover Street");
                    EditorGUI.indentLevel++;
                    List<Waypoint> hanover = spawnVehicle.hanover;
                    int size2 = Mathf.Max(0, EditorGUILayout.IntField("Size", hanover.Count));

                    while (size2 > hanover.Count)
                    {
                        hanover.Add(null);
                    }

                    while (size2 < hanover.Count)
                    {
                        hanover.RemoveAt(hanover.Count - 1);
                    }

                    for (int i = 0; i < hanover.Count; i++)
                    {
                        hanover[i] = EditorGUILayout.ObjectField("Spawn point " + i, hanover[i], typeof(Waypoint), true) as Waypoint;
                    }
                    EditorGUI.indentLevel--;

                    EditorGUILayout.LabelField("Brook Hill");
                    EditorGUI.indentLevel++;
                    List<Waypoint> brookHill = spawnVehicle.brookHill;
                    int size3 = Mathf.Max(0, EditorGUILayout.IntField("Size", brookHill.Count));

                    while (size3 > brookHill.Count)
                    {
                        brookHill.Add(null);
                    }

                    while (size3 < brookHill.Count)
                    {
                        brookHill.RemoveAt(brookHill.Count - 1);
                    }

                    for (int i = 0; i < brookHill.Count; i++)
                    {
                        brookHill[i] = EditorGUILayout.ObjectField("Spawn point " + i, brookHill[i], typeof(Waypoint), true) as Waypoint;
                    }
                    EditorGUI.indentLevel--;

                    EditorGUILayout.LabelField("Bolsover Street");
                    EditorGUI.indentLevel++;
                    List<Waypoint> bolsover = spawnVehicle.bolsover;
                    int size4 = Mathf.Max(0, EditorGUILayout.IntField("Size", bolsover.Count));

                    while (size4 > bolsover.Count)
                    {
                        bolsover.Add(null);
                    }

                    while (size4 < bolsover.Count)
                    {
                        bolsover.RemoveAt(bolsover.Count - 1);
                    }

                    for (int i = 0; i < bolsover.Count; i++)
                    {
                        bolsover[i] = EditorGUILayout.ObjectField("Spawn point " + i, bolsover[i], typeof(Waypoint), true) as Waypoint;
                    }
                    EditorGUI.indentLevel--;

                    EditorGUILayout.LabelField("Netherthorpe Road");
                    EditorGUI.indentLevel++;
                    List<Waypoint> netherthorpe = spawnVehicle.netherthorpe;
                    int size5 = Mathf.Max(0, EditorGUILayout.IntField("Size", netherthorpe.Count));

                    while (size5 > netherthorpe.Count)
                    {
                        netherthorpe.Add(null);
                    }

                    while (size5 < netherthorpe.Count)
                    {
                        netherthorpe.RemoveAt(netherthorpe.Count - 1);
                    }

                    for (int i = 0; i < netherthorpe.Count; i++)
                    {
                        netherthorpe[i] = EditorGUILayout.ObjectField("Spawn point " + i, netherthorpe[i], typeof(Waypoint), true) as Waypoint;
                    }
                    EditorGUI.indentLevel--;
                }
                serializedObject.ApplyModifiedProperties();
            }
        }

#endif
        #endregion
    }
}

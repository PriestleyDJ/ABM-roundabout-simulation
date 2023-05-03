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
        List<Waypoint> broadLane = new List<Waypoint>();
        List<Waypoint> hanover = new List<Waypoint>();
        List<Waypoint> brookHill = new List<Waypoint>();
        List<Waypoint> bolsover = new List<Waypoint>();
        List<Waypoint> netherthorpe = new List<Waypoint>();

        bool showSpawnpoints = false;

        #region Editor
#if UNITY_EDITOR

        [CustomEditor(typeof(SpawnVehicle))]
        public class SpawnVehicleEditor : Editor
        {
            public override void OnInspectorGUI()
            {
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
                        bolsover.Add(null);
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
            }
        }

#endif
        #endregion
    }
}

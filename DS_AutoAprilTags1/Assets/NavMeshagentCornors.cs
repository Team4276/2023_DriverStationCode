using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FRC.NetworkTables;
using FRC.NativeLibraryUtilities;
using FRC.ILGeneration;
using System;
using FRC.NetworkTables.Interop;
using System.Threading;




public class NavMeshagentCornors : MonoBehaviour
{
    public Transform destination;
    public Color pathColor = Color.red;
    public NetworkTable nt;

    readonly public string tableName = "PathCorners";
    readonly public string entryName = "Corners";
    readonly public string DisEntryName = "Distance";
    readonly public string PathSent = "PathSent";

    readonly public int port = 1735; // default port number
    readonly public int TeamNum = 4276;
    float distance = 0;
    Boolean lineRendered = false;
    Boolean SendPath;
    Boolean disntanceNotUpdadated = true;

    private NavMeshAgent agent;
    private LineRenderer lineRenderer;

      
    void Update() {
        NetworkTableInstance nt = NetworkTableInstance.Default;
        nt.StartDSClient();
        nt.StartClient();
        nt.StartClientTeam(TeamNum, port);
        nt.StartServer();
        NetworkTable table = nt.GetTable(tableName);
        //Debug.Log(SendPath);


            distance = 0;

            agent = GetComponent<NavMeshAgent>();

            // get the path to the destination
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(destination.position, path);

            // send the path corners to NetworkTables
            Vector3[] pathCorners = path.corners;
            List<double> cornerValues = new List<double>();

            // seting up the line render in the middle here in case no connection is made to roborio
            if (lineRendered == false)
            {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
                lineRenderer.startWidth = 0.3f;
                lineRenderer.endWidth = 0.3f;
                lineRenderer.material.color = pathColor;
                lineRenderer.positionCount = pathCorners.Length;
                lineRenderer.SetPositions(pathCorners);
                //Debug.Log(lineRenderer.isVisible);
                lineRendered = true;
            }


            //sends the path only when connected 


            for (int i = 0; i < pathCorners.Length; i++)
            {
                cornerValues.Add(Math.Round(pathCorners[i].x, 4));
                cornerValues.Add(Math.Round(pathCorners[i].y, 4));
                cornerValues.Add(Math.Round(pathCorners[i].z, 4));
                if (i + 1 < pathCorners.Length) { 
                
                 distance = distance + Vector3.Distance(pathCorners[i], pathCorners[i + 1]);
           
                }

            }
            //Debug.Log(distance);
            //Debug.Log(cornerValues[3]);
            //Debug.Log(pathCorners[1]);

            //Send the corner values to NetworkTables
            table.GetEntry(entryName).SetDoubleArray(cornerValues.ToArray());

            table.GetEntry(DisEntryName).SetDouble(distance);

            table.GetEntry(PathSent).SetBoolean(true);





            SendPath = false;
        




        

 
    }
}

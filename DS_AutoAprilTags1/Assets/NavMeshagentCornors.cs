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

    readonly int port = 1735; // default port number
    readonly int TeamNum = 4276;
    Boolean lineRendered = false;

    private NavMeshAgent agent;
    private LineRenderer lineRenderer;

    void Start() { 
   /* 



        NetworkTableInstance nt = NetworkTableInstance.Default;
        nt.StartDSClient();
        nt.StartClient();
        nt.StartClientTeam(TeamNum, port);
        nt.StartServer();



        agent = GetComponent<NavMeshAgent>();
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // get the path to the destination
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(destination.position, path);

        // send the path corners to NetworkTables
        Vector3[] pathCorners = path.corners;
        List<double> cornerValues = new List<double>();

        // seting up the line render in the middle here in case no connection is made to roborio
        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.3f;
        lineRenderer.material.color = pathColor;
        lineRenderer.positionCount = pathCorners.Length;
        lineRenderer.SetPositions(pathCorners);
        Debug.Log(lineRenderer.isVisible);
        
        
        //sends the path only when connected 
       

        for (int i = 0; i < pathCorners.Length; i++)
        {
            cornerValues.Add(Math.Round(pathCorners[i].x, 4));
            cornerValues.Add(Math.Round(pathCorners[i].y, 4));
            cornerValues.Add(Math.Round(pathCorners[i].z, 4));
        }

        Debug.Log(cornerValues[3]);
        Debug.Log(pathCorners[1]);

         //Send the corner values to NetworkTables
        NetworkTable table = nt.GetTable(tableName);
        table.GetEntry(entryName).SetDoubleArray(cornerValues.ToArray());
   */
   }

        

    


    void Update()
    {
        {



            NetworkTableInstance nt = NetworkTableInstance.Default;
            nt.StartDSClient();
            nt.StartClient();
            nt.StartClientTeam(TeamNum, port);
            nt.StartServer();



            agent = GetComponent<NavMeshAgent>();
            lineRenderer = gameObject.AddComponent<LineRenderer>();

            // get the path to the destination
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(destination.position, path);

            // send the path corners to NetworkTables
            Vector3[] pathCorners = path.corners;
            List<double> cornerValues = new List<double>();

            // seting up the line render in the middle here in case no connection is made to roborio
            if (lineRendered == false)
            {
                lineRenderer.startWidth = 0.3f;
                lineRenderer.endWidth = 0.3f;
                lineRenderer.material.color = pathColor;
                lineRenderer.positionCount = pathCorners.Length;
                lineRenderer.SetPositions(pathCorners);
                Debug.Log(lineRenderer.isVisible);
                lineRendered = true;
            }


            //sends the path only when connected 


            for (int i = 0; i < pathCorners.Length; i++)
            {
                cornerValues.Add(Math.Round(pathCorners[i].x, 4));
                cornerValues.Add(Math.Round(pathCorners[i].y, 4));
                cornerValues.Add(Math.Round(pathCorners[i].z, 4));
            }

            Debug.Log(cornerValues[3]);
            Debug.Log(pathCorners[1]);

            //Send the corner values to NetworkTables
            NetworkTable table = nt.GetTable(tableName);
            table.GetEntry(entryName).SetDoubleArray(cornerValues.ToArray());




        }


        if (agent.remainingDistance < agent.stoppingDistance)
        {
            // Path complete
            Debug.Log("Path completed");
        }
    }
}

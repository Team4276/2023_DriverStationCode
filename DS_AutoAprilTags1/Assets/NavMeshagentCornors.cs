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

    public List<Vector3> pointsList = new List<Vector3>();
    public static List<double> cornerValues;
    
    public static Vector3[] pathCorners;
    void Start()
    {
        pointsList.Add(transform.position); // Add the current position as the starting point
        pointsList.Add (new Vector3((float)-5.15, 0, (float)2.45));
        pointsList.Add(new Vector3((float)6.76, 0, (float)-1.14));
        pointsList.Add(new Vector3((float)-0.74, 0, (float)-4.15));
        Debug.Log(pointsList.Count);
        pathCorners = CalculatePath().ToArray();

    }

    void Update() {
        NetworkTableInstance nt = NetworkTableInstance.Default;
        nt.StartDSClient();
        nt.StartClient();
        nt.StartClientTeam(TeamNum, port);
        nt.StartServer();
        NetworkTable table = nt.GetTable(tableName);
        //Debug.Log(SendPath);
        Vector3 Target = new Vector3((float)table.GetEntry("X target").GetDouble(0), (float)table.GetEntry("Y target").GetDouble(0), (float)table.GetEntry("Z target").GetDouble(0));
        
            distance = 0;

        // send the path corners to NetworkTables
          
         cornerValues = new List<double>();

            // seting up the line render in the middle here in case no connection is made to roborio
            if (lineRendered == false)
            {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
                lineRenderer.startWidth = 0.3f;
                lineRenderer.endWidth = 0.3f;
                lineRenderer.material.color = pathColor;
                lineRenderer.positionCount = pathCorners.Length;
                //Debug.Log(lineRenderer.isVisible);
                lineRendered = true;
            }
            lineRenderer.SetPositions(pathCorners);


        //networktables does not support the vector3 type so changing it to a list of vaules (x1, y1, z1, x2, y2, z2) also gets the distance of the whole path 
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
            Debug.Log(cornerValues[3]);
            Debug.Log(pathCorners[1]);

        //Send the corner values to the table and the distance 

            table.GetEntry(entryName).SetDoubleArray(cornerValues.ToArray());

            table.GetEntry(DisEntryName).SetDouble(distance);

            table.GetEntry(PathSent).SetBoolean(true);





            SendPath = false;
        

 
    }

    List<Vector3> CalculatePath()
    {
        // Make sure we have at least two points in the list
       agent = GetComponent<NavMeshAgent>();
        if (pointsList.Count < 2)
        {
            Debug.LogError("Pathfinding: Not enough points to calculate path.");
        }

        // Create a list of points to visit that excludes the starting point
        List<Vector3> remainingPoints = new List<Vector3>(pointsList);
        remainingPoints.RemoveAt(0);

        // Calculate the path from the starting point to the first point in the list
        NavMeshPath path = new NavMeshPath();

        agent.CalculatePath(pointsList[0], path);

        // Create a list to hold the corners of the path
        List<Vector3> corners = new List<Vector3>(path.corners);

        // Loop through the remaining points and add them to the path
        foreach (Vector3 point in remainingPoints)
        {
            NavMeshPath segmentPath = new NavMeshPath();
            agent.CalculatePath(point, segmentPath);
            agent.Warp(segmentPath.corners[segmentPath.corners.Length - 2]);

            // Add the corners of the segment path to the main path
            for (int i = 1; i < segmentPath.corners.Length; i++)
            {
                corners.Add(segmentPath.corners[i]);
            }
        }

        // Print the corners of the final path
        /* foreach (Vector3 corner in corners)
         {
             Debug.Log(corner);
         }*/

        return (corners);
    }
}



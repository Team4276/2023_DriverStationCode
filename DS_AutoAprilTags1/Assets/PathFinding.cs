using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    public List<Vector3> pointsList = new List<Vector3>(); // List of points to visit
    private NavMeshAgent agent; // NavMeshAgent component reference

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        pointsList.Add(transform.position); // Add the current position as the starting point
        pointsList.Add(transform.position + Vector3.right * 0.25f);
        pointsList.Add(transform.position + Vector3.forward * 0.25f);
        pointsList.Add(transform.position + Vector3.right * 0.25f + Vector3.forward * 0.25f);
        CalculatePath();
    }

    void CalculatePath()
    {
        // Make sure we have at least two points in the list
        if (pointsList.Count < 2)
        {
            Debug.LogError("Pathfinding: Not enough points to calculate path.");
            return;
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

            // Add the corners of the segment path to the main path
            for (int i = 1; i < segmentPath.corners.Length; i++)
            {
                corners.Add(segmentPath.corners[i]);
            }
        }

        // Print the corners of the final path
        foreach (Vector3 corner in corners)
        {
            Debug.Log(corner);
        }
    }
}

using UnityEngine;
using FRC.NetworkTables;

public class NetworkTablesServer : MonoBehaviour
{
    private NetworkTableInstance instance;

    void Start()
    {
        try
        {
            // Initialize the NetworkTables instance
            instance = NetworkTableInstance.Default;
            instance.StartServer("UnityServer", "127.0.0.1", 1735);

            // Connect to the RoboRIO
            instance.StartClient("roboRIO-4276-FRC.server");
            instance.SetServerTeam(4276);

            // Send a number to the RoboRIO
            var table = instance.GetTable("myTable").GetEntry("num").SetDouble(2);
            Debug.Log(table);
        }
        catch 
        {
            Debug.Log("unkwon error");
        }
    }

    private void Update()
    {
        try
        {
            var table = instance.GetTable("myTable").GetEntry("num").SetDouble(2);
            Debug.Log(table);
        }
        catch
        {
            Debug.Log("unkwon error (10233)");
        }

    }

    void OnApplicationQuit()
    {
        // Stop the NetworkTables instance when the application quits
        instance.StopServer();
        instance.StopClient();
    }
}

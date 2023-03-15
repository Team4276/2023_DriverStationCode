using UnityEngine;
using FRC.NetworkTables;

public class comms : MonoBehaviour
    

{
    public double num = 1;
    void Start()
    {
        NetworkTableInstance inst = NetworkTableInstance.Default;
    }

    void Update()
    {
        NetworkTableInstance inst = NetworkTableInstance.Default;
        inst.StartDSClient();
        inst.StartClient();
        inst.StartClientTeam(4276, 1735);
        inst.StartServer();
       
        NetworkTable table = inst.GetTable("MyTable"); // get table named "MyTable"
        table.GetEntry("Number").SetDouble(num); // get value of "Number" key, default value is 0
        num++;
    }
}

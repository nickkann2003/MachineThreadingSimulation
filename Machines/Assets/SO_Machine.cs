using UnityEngine;

[CreateAssetMenu(fileName = "SO_Machine", menuName = "Scriptable Objects/SO_Machine")]
public class SO_Machine : ScriptableObject
{
    public Machines machineType;
    public MachineInputs machineInput;
    public MachineOutputs machineOutput;
}

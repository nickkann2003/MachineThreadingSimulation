using UnityEngine;

public class MachineVisualController : MonoBehaviour
{
    [SerializeField] private IMachine machine;
    [SerializeField] private IMachineInput machineInput;
    [SerializeField] private IMachineOutput machineOutput;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        machine = new MachineBase();
        machineInput = new MachineInputBase();
        machineOutput = new MachineOutputBase();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

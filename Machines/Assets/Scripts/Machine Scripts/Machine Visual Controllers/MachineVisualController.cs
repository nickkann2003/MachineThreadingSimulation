using UnityEngine;

public class MachineVisualController : MonoBehaviour
{
    [SerializeField] private IMachine machine;
    [SerializeField] public IMachineInput machineInput;
    [SerializeField] public IMachineOutput machineOutput;

    // Normalized direction vector, which way is this machine pointing
    [SerializeField] public Vector2 direction;

    // -------------- Public Functions ----------------
    /// <summary>
    /// Creates and sets up the underlying machine functions, hooking them up to nearby machines
    /// </summary>
    /// <param name="previousMachineOutput">Output from the previous connected machine, NULL if not there</param>
    /// <param name="nextMachineInput">Input for the next connected machine, NULL if not there</param>
    public void InitializeMachine(MachineVisualController previousMachineOutput, MachineVisualController nextMachineInput)
    {
        machine = new MachineBase();
        machineInput = new MachineInputBase();
        machineOutput = new MachineOutputBase();

        // Connect internals
        ConnectMachine();

        // Connect to pervious if it exists
        if(previousMachineOutput != null)
            ConnectToPreviousMachine(previousMachineOutput);
        
        // Connect to next if exists
        if(nextMachineInput != null)
            ConnectToNextMachine(nextMachineInput);
    }

    /// <summary>
    /// Connects this machine's output to a machine input
    /// </summary>
    /// <param name="next">Input for the next connceted machine</param>
    public void ConnectToNextMachine(MachineVisualController next)
    {
        if (next.direction.Equals(direction))
        {
            machineOutput.SetInputReference(next.machineInput);     // Set this to push to next machine
            next.machineInput.SetOutputReference(machineOutput);    // Set next machine to pull from this
        }
    }

    /// <summary>
    /// Connect this machine's input to a previous machines output
    /// </summary>
    /// <param name="previous">Previous machine's output to connect</param>
    public void ConnectToPreviousMachine(MachineVisualController previous)
    {
        if (previous.direction.Equals(direction))
        {
            machineInput.SetOutputReference(previous.machineOutput);    // Set this to pull from previous
            previous.machineOutput.SetInputReference(machineInput);     // Set previous to push to this
        }
    }

    // -------------- Private Functions ----------------
    private void ConnectMachine()
    {
        machine.SetOutputReference(machineOutput);
        machine.SetInputReference(machineInput);
        machineInput.SetMachineReference(machine);
        machineOutput.SetMachineReference(machine);
    }
}

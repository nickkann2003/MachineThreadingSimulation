using UnityEngine;

public class MachineVisualController : MonoBehaviour
{
    [Header("Machine script references")]
    [SerializeField] private IMachine machine;
    [SerializeField] public IMachineInput machineInput;
    [SerializeField] public IMachineOutput machineOutput;

    [Header("Visual controller references")]
    [SerializeField] public BarVisualController inputController;
    [SerializeField] public BarVisualController outputController;
    [SerializeField] public BarVisualController machineController;

    [Header("Output visual references")]
    [SerializeField] public GameObject rightOutputVisual;
    [SerializeField] public GameObject upOutputVisual;
    [SerializeField] public GameObject leftOutputVisual;
    [SerializeField] public GameObject downOutputVisual;

    // Normalized direction vector, which way is this machine pointing
    [SerializeField] public Vector2 outputDirection;
    [SerializeField] public Vector2 inputDirection;

    [SerializeField] public SO_Machine machineSO;

    // -------------- Public Functions ----------------
    /// <summary>
    /// Creates and sets up the underlying machine functions, hooking them up to nearby machines
    /// </summary>
    /// <param name="previousMachineOutput">Output from the previous connected machine, NULL if not there</param>
    /// <param name="nextMachineInput">Input for the next connected machine, NULL if not there</param>
    public void InitializeMachine(MachineVisualController previousMachineOutput, MachineVisualController nextMachineInput)
    {
        // Create machine pieces from Factory
        machine = MachineFactory.CreateMachine(machineSO.machineType);

        // Machine input and output
        machineInput = MachineFactory.CreateMachineInput(machineSO.machineInput);
        machineOutput = MachineFactory.CreateMachineOutput(machineSO.machineOutput);
        SetBarPositionAndRotation(inputController, inputDirection);
        SetBarPositionAndRotation(outputController, outputDirection);

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
        machineOutput.SetInputReference(next.machineInput);     // Set this to push to next machine
        next.machineInput.SetOutputReference(machineOutput);    // Set next machine to pull from this
    }

    /// <summary>
    /// Connect this machine's input to a previous machines output
    /// </summary>
    /// <param name="previous">Previous machine's output to connect</param>
    public void ConnectToPreviousMachine(MachineVisualController previous)
    {
        if (previous.outputDirection.Equals(outputDirection))
        {
            machineInput.SetOutputReference(previous.machineOutput);    // Set this to pull from previous
            previous.machineOutput.SetInputReference(machineInput);     // Set previous to push to this
        }
    }

    /// <summary>
    /// Connect output to another MachineVisualController in a specific direction
    /// </summary>
    /// <param name="direction">Direction output is going to</param>
    /// <param name="other">Other machine to connect to</param>
    public void ConnectToDirection(Vector2 direction, MachineVisualController other)
    {
        SetOutputDirection(direction);
        other.SetInputDirection(direction * -1);
        ConnectToNextMachine(other);
    }

    /// <summary>
    /// Calls notify input to kick off machine processing
    /// </summary>
    public void KickoffFunctionality()
    {
        machine.NotifyInput();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction"></param>
    public void SetInputDirection(Vector2 direction)
    {
        inputDirection = direction;
        //SetBarPositionAndRotation(inputController, direction);
    }

    public void SetOutputDirection(Vector2 direction)
    {
        outputDirection = direction;
        SetOutputVisual(direction);
    }

    // -------------- Private Functions ----------------
    private void ConnectMachine()
    {
        machine.SetOutputReference(machineOutput);
        machine.SetInputReference(machineInput);
        machineInput.SetMachineReference(machine);
        machineOutput.SetMachineReference(machine);

        machineInput.SetDisplayReference(inputController);
        machineOutput.SetDisplayReference(outputController);
        machine.SetDisplayReference(machineController);
    }

    /// <summary>
    /// Private helper function for setting physical position and rotation of a meter
    /// </summary>
    /// <param name="bar">BarVisualController object script</param>
    /// <param name="direction">Direction it faces</param>
    private void SetBarPositionAndRotation(BarVisualController bar, Vector2 direction)
    {
        //RectTransform barTransform = bar.GetComponent<RectTransform>();
        //barTransform.localPosition = new Vector3(direction.x * 0.45f, direction.y * -0.45f, -0.05f);
        //if(direction.y != 0)
        //{
        //    barTransform.Rotate(new Vector3(0, 0, 90));
        //}
        // REMOVED, changed to output pipes visuals
    }

    /// <summary>
    /// Activates the given output visual based on the direction given
    /// </summary>
    /// <param name="direction">Direction of output for this machine</param>
    private void SetOutputVisual(Vector2 direction)
    {
        disableAllOutputVisuals();
        if (direction.x == 1)
            upOutputVisual.SetActive(true);
        if (direction.x == -1)
            downOutputVisual.SetActive(true);
        if (direction.y == 1)
            leftOutputVisual.SetActive(true);
        if (direction.y == -1)
            rightOutputVisual.SetActive(true);
    }

    private void disableAllOutputVisuals()
    {
        upOutputVisual.SetActive(false); 
        downOutputVisual.SetActive(false); 
        leftOutputVisual.SetActive(false); 
        rightOutputVisual.SetActive(false);
    }

    // -------------------- Unity Functions ---------------------
    private void OnApplicationQuit()
    {
        machine.Stop();
        machineInput.Stop();
        machineOutput.Stop();
    }
}

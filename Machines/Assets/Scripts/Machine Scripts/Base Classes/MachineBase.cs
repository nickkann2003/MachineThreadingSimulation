using System.Threading;

public class MachineBase : IMachine
{
    // Basic machine references
    private IMachineInput _input;
    private IMachineOutput _output;
    private IProcessable? currentItem;

    // Processing variables - used to avoid stalling
    private bool busy;
    private bool outputFull;

    // Threading variables - each processing function needs a thread
    Thread t_Process;

    // ------------ Constructors ------------
    public MachineBase()
    {
        t_Process = new Thread(Process);
    }

    // ------------ Machine Base Functions -----------
    /// <summary>
    /// Basic awake checks function, performs checks on current state,
    /// available output, available input, then calls process
    /// </summary>
    protected virtual void AwakeChecks()
    {
        // If no current item
        if (currentItem == null)
        {
            // Try and grab one
            currentItem = _input.GetNextInput();

            // If successfully got one, Process
            if (currentItem != null)
                t_Process.Start();
        }
        else // Already had an item
        {
            if (outputFull) // Output full, try to end process
            {
                EndProcess();
            }
            else if (!busy) // Output not full, not busy, have item, Start Process
            {
                t_Process.Start();
            }
        }
    }

    protected virtual void Process()
    {
        busy = true;
        for (int i = 0; i < 10; i++)
        {
            Thread.Sleep(100);
        }
        EndProcess();
    }

    /// <summary>
    /// Handles end of process vals
    /// </summary>
    private void EndProcess()
    {
        if (_output.GiveOutput(currentItem))
        {
            currentItem = null;
            busy = false;
            outputFull = false;
        }
        else
        {
            outputFull = true;
        }
    }

    // ----------- IMachine Members -----------
    public virtual void NotifyInput()
    {
        AwakeChecks();
    }

    public virtual void NotifyOutput()
    {
        AwakeChecks();
    }
}

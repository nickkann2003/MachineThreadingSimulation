using System.Threading;
using UnityEngine;

public class MachineBase : IMachine
{
    // Basic machine references
    protected IMachineInput connectedInput;
    protected IMachineOutput connectedOutput;
    protected IProcessable? currentItem;

    // Processing variables - used to avoid stalling
    protected bool busy;
    private bool done = false;
    protected bool outputFull;

    // Threading variables - each processing function needs a thread
    protected Thread t_Process;

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
        // Before performing awake checks, see if the previous process completed
        // Once the process completes, a new Thread must be create for processing the next item
        if (done)
        {
            if (t_Process.IsAlive)
            {
                t_Process.Join();
            }
            t_Process = new Thread(Process);
            done = false;
        }

        // If no current item
        if (currentItem == null)
        {
            // Try and grab one
            currentItem = connectedInput.GetNextInput();

            // If successfully got one
            if (currentItem != null)
            {
                // If process reference is alive, its pointing to old process, Join and remake thread
                if (t_Process.IsAlive)
                {
                    t_Process.Join();
                    t_Process = new Thread(Process);
                    t_Process.Start();
                }
                t_Process.Start();
            }
        }
        else // Already had an item
        {
            if (!busy && !outputFull && !t_Process.IsAlive) // Output not full, not busy, have item, Start Process
            {
                t_Process.Start();
            }
            else
            {
                if (GiveToOutput())
                {
                    AwakeChecks();
                }
            }
        }
    }

    protected virtual void Process()
    {
        busy = true;
        for (int i = 0; i < 3; i++)
        {
            Thread.Sleep(200);
        }
        busy = false;
        EndProcess();
    }

    /// <summary>
    /// Handles end of process vals
    /// </summary>
    protected void EndProcess()
    {
        if (done)
            return;

        GiveToOutput();
    }

    /// <summary>
    /// Private function that handles the process of giving output to connected output
    /// Sets necessary values and returns false if output is full
    /// </summary>
    private bool GiveToOutput()
    {
        if (currentItem == null || busy)
            return false;

        bool success = connectedOutput.GiveOutput(currentItem);
        if (success)
        {
            currentItem = null;
            outputFull = false;
            done = true;
            return true;
        }
        else
        {
            outputFull = true;
            return false;
        }
    }

    public void SetInputReference(IMachineInput input)
    {
        this.connectedInput = input;
    }

    public void SetOutputReference(IMachineOutput output)
    {
        this.connectedOutput = output;
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

    public void Stop()
    {
        t_Process.Abort();
    }
}

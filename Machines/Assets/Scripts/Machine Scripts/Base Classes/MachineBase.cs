using System.Threading;
using UnityEngine;

public class MachineBase : IMachine
{
    // Basic machine references
    protected IMachineInput connectedInput;
    protected IMachineOutput connectedOutput;
    protected IProcessable? currentItem;
    protected BarVisualController visualController;

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
        t_Process.IsBackground = true;
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
            t_Process.IsBackground = true;
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
                    Debug.Log("Machine process was alive, joining and then starting new proces");
                    t_Process.Join();
                    Debug.Log("JOIN SUCCESS, starting new process");
                    t_Process = new Thread(Process);
                    t_Process.IsBackground = true;
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
            else // Ouput was full, try giving to output again
            {
                if (GiveToOutput())
                {
                    Thread t_awakeChecks = new Thread(AwakeChecks);
                    t_awakeChecks.Start();
                }
            }
        }
    }

    protected virtual void Process()
    {
        int progress = 0;
        busy = true;
        for (int i = 0; i < 7; i++)
        {
            Thread.Sleep(300);
            progress++;
            visualController.SetProgress((float)progress / 7f, 0, 1);
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
        visualController.SetProgress(currentItem == null ? 0 : 1, currentItem == null ? 0 : 1, 1);
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

    public void SetDisplayReference(BarVisualController controller)
    {
        this.visualController = controller;
    }

    // ----------- IMachine Members -----------
    public virtual void NotifyInput()
    {
        Thread t_awakeChecks = new Thread(AwakeChecks);
        t_awakeChecks.Start();
    }

    public virtual void NotifyOutput()
    {
        Thread t_awakeChecks = new Thread(AwakeChecks);
        t_awakeChecks.Start();
    }

    public void Stop()
    {
        t_Process.Abort();
    }
}

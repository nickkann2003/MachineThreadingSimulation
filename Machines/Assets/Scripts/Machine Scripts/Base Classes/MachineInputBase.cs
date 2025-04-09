using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MachineInputBase : IMachineInput
{
    // Queue of items and storage info
    private Queue<IProcessable> buffer =  new Queue<IProcessable>();
    private int slotsUsed = 0;
    private int bufferSize = 5;
    private bool busy = false; // Is thread currently running

    private IMachineOutput connectedOutput;
    private IMachine connectedMachine;
    private BarVisualController displayController;

    // -------------- IMachineInput members ----------------
    /// <summary>
    /// Returns the next available item from this input, null if none available
    /// </summary>
    /// <returns>Next item or null</returns>
    public IProcessable? GetNextInput()
    {
        IProcessable returnedItem = null;
        // If full, dequeue and pull from output
        if (buffer.Count >= bufferSize)
        {
            returnedItem = buffer.Dequeue();
            slotsUsed--;
            PullFromOutput();
        }
        else if (buffer.Count > 0)
        {
            returnedItem = buffer.Dequeue();
            slotsUsed--;
        } 
        else if(slotsUsed == 0)
        {
            PullFromOutput();
        }

        UpdateVisual();

        // Return null if empty
        return returnedItem;
    }

    /// <summary>
    /// Gives this input an item, if possible, and spins off a thread to perform the add
    /// </summary>
    /// <param name="input">Item to add</param>
    /// <returns>True if successful, false if not</returns>
    public bool GiveInput(IProcessable input)
    {
        if (buffer.Contains(input))
        {
            UpdateVisual();
            return true;
        }

        if (slotsUsed < bufferSize && !busy)
        {
            Thread t_enqueue = new Thread(() => PerformEnqueue(input));
            t_enqueue.Start();
            slotsUsed++;
            busy = true;
            return true;
        }
        else
        {
            Debug.Log("Attempted to give to input, but slotsUsed was full, or was busy");
            return false;
        }
    }

    public void SetMachineReference(IMachine machine)
    {
        this.connectedMachine = machine;
    }

    public void SetOutputReference(IMachineOutput output)
    {
        this.connectedOutput = output;
    }

    public void SetDisplayReference(BarVisualController displayController)
    {
        this.displayController = displayController;
        UpdateVisual();
    }

    public void Stop()
    {
        // No threads in input to stop
    }

    // ------------------ Public functions ---------------
    public void SetOutputAndMachine(IMachineOutput _output, IMachine _machine)
    {
        connectedOutput = _output;
        connectedMachine = _machine;
    }

    // ------------------ Private functions ---------------

    /// <summary>
    /// Asynchronous function that performs the Enqueue of a given item
    /// </summary>
    /// <param name="item"></param>
    private void PerformEnqueue(IProcessable item)
    {
        buffer.Enqueue(item);
        UpdateVisual();

        Thread.Sleep(500);
        busy = false;

        connectedMachine.NotifyInput();
        // If more openings available, attempt pull from Output
        if (slotsUsed < bufferSize)
        {
            PullFromOutput();
        }
    }

    /// <summary>
    /// Attempts to pull an item from connected output
    /// </summary>
    private void PullFromOutput()
    {
        if(connectedOutput != null)
        {
            IProcessable nextItem = connectedOutput.GetNextOutput();
            if(nextItem != null)
            {
                Thread t_enqueue = new Thread(() => PerformEnqueue(nextItem));
                t_enqueue.Start();
                slotsUsed++;
            }
        }
    }

    /// <summary>
    /// Updates the visuals for this input bar
    /// </summary>
    private void UpdateVisual()
    {
        if(displayController != null)
        {
            displayController.SetProgress((float)buffer.Count / (float)bufferSize, buffer.Count, bufferSize);
        }
    }
}

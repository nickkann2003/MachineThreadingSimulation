using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MachineOutputBase : IMachineOutput
{
    // Queue of items and storage info
    private Queue<IProcessable> buffer = new Queue<IProcessable>();
    private int slotsUsed = 0; // Independent counter, includes items in process of being added by a thread
    private int bufferSize = 5;
    private bool busy = false; // Shows if thread is running

    // Whether or not this Output is currently Active
    public bool active = true;

    private IMachine connectedMachine;
    private IMachineInput connectedInput;
    private BarVisualController displayController;

    // ----------------- IMachineOutput members --------------------
    /// <summary>
    /// Returns the next item available from this output, null if none available
    /// </summary>
    /// <returns>Next output item or null</returns>
    public IProcessable GetNextOutput()
    {
        // If inactive, return null
        if (!active)
            return null;

        IProcessable returnedItem = null;
        // If full, dequeue and notify machine
        if (buffer.Count >= bufferSize)
        {
            returnedItem = buffer.Dequeue();
            slotsUsed--;
            connectedMachine.NotifyOutput();
        } 
        else if (buffer.Count > 0)
        {
            returnedItem = buffer.Dequeue();
            slotsUsed--;
        }

        UpdateVisual();

        // Return null if empty
        return returnedItem;
    }

    /// <summary>
    /// Gives this output an item, returning based on success
    /// </summary>
    /// <param name="output">Item to be given to this output</param>
    /// <returns>False if unable to add, true if added successfully</returns>
    public bool GiveOutput(IProcessable output)
    {
        // Return false if not active
        if (!active)
            return false;

        if (buffer.Contains(output))
        {
            UpdateVisual();
            return true;
        }

        if (slotsUsed < bufferSize && !busy)
        {
            // Threaded enqueue and return true for adding
            Thread t_enqueue = new Thread(() => { PerformEnqueue(output); });
            t_enqueue.Start();
            slotsUsed++;
            busy = true;
            return true;
        }
        else
        {
            Debug.Log("Attempted to give to OUTPUT, but slotsUsed was full, or was busy");
            return false;
        }
    }

    // ------------------ Public reference setters -----------------------
    public void SetInputReference(IMachineInput input)
    {
        connectedInput = input;
        ActiveCheck();
    }

    public void SetMachineReference(IMachine machine)
    {
        connectedMachine = machine;
        ActiveCheck();
    }

    public void SetDisplayReference(BarVisualController displayController)
    {
        this.displayController = displayController;
        UpdateVisual();
    }

    public void Stop()
    {
        // Machine output has no threads to stop
    }

    // --------------- Private Functions --------------
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

        // After processing, attempt a push to input
        PushToInput();

        // If more openings available, let the machine know
        if(slotsUsed < bufferSize)
        {
            connectedMachine.NotifyOutput();
        }
    }

    /// <summary>
    /// Pushes next item to connected IMachineInput, if possible
    /// </summary>
    private void PushToInput()
    {
        if(connectedInput != null)
        {
            if (buffer.Count > 0 && connectedInput.GiveInput(buffer.Peek()))
            {
                buffer.Dequeue();
                slotsUsed--;
                UpdateVisual();
                PushToInput();
            }
        }
    }
    
    /// <summary>
    /// Updates the visual display for this output
    /// </summary>
    private void UpdateVisual()
    {
        if (displayController != null)
        {
            displayController.SetProgress((float)buffer.Count / (float)bufferSize, buffer.Count, bufferSize);
        }
    }

    /// <summary>
    /// Checks if this MachineOutput has valid references and should be active
    /// </summary>
    private void ActiveCheck()
    {
        if(connectedInput != null && connectedMachine != null)
        {
            active = true;
        }
        else
        {
            active = false;
        }
    }



}

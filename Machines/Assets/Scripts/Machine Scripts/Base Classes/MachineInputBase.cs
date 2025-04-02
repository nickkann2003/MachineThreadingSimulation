using System.Collections.Generic;
using UnityEngine;

public class MachineInputBase : IMachineInput
{
    // Queue of items and storage info
    private Queue<IProcessable> buffer =  new Queue<IProcessable>();
    private int bufferSize = 5;

    private IMachineOutput connectedOutput;
    private IMachine connectedMachine;
    private BarVisualController displayController;

    // -------------- IMachineInput members ----------------
    public IProcessable? GetNextInput()
    {
        IProcessable returnedItem = null;
        // If full, dequeue and pull from output
        if (buffer.Count >= bufferSize)
        {
            returnedItem = buffer.Dequeue();
            PullFromOutput();
        }
        else if (buffer.Count > 0)
        {
            returnedItem = buffer.Dequeue();
        } 
        else if(buffer.Count == 0)
        {
            PullFromOutput();
            if(buffer.Count > 0)
            {
                returnedItem = buffer.Dequeue();
            }
        }

        UpdateVisual();

        // Return null if empty
        return returnedItem;
    }

    public bool GiveInput(IProcessable input)
    {
        if(buffer.Count < bufferSize)
        {
            // If no items, perform add and notify machine
            if(buffer.Count == 0)
            {
                buffer.Enqueue(input);
                connectedMachine.NotifyInput();
                return true;
            }

            // Perform add
            buffer.Enqueue(input);
            return true;
        }
        else
        {
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
    private void PullFromOutput()
    {
        if(connectedOutput != null)
        {
            IProcessable nextItem = connectedOutput.GetNextOutput();
            if(nextItem != null)
            {
                buffer.Enqueue(nextItem);
            }
        }
    }

    private void UpdateVisual()
    {
        if(displayController != null)
        {
            displayController.SetProgress((float)buffer.Count / (float)bufferSize);
        }
    }
}

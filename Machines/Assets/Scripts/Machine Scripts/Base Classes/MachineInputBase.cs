using System.Collections.Generic;
using UnityEngine;

public class MachineInputBase : IMachineInput
{
    // Queue of items and storage info
    private Queue<IProcessable> buffer;
    private int bufferSize = 5;

    private IMachineOutput connectedOutput;
    private IMachine connectedMachine;

    // -------------- IMachineInput members ----------------
    public IProcessable? GetNextInput()
    {
        // If full, dequeue and pull from output
        if(buffer.Count >= bufferSize)
        {
            IProcessable returnedItem = buffer.Dequeue();
            PullFromOutput();
            return returnedItem;
        }
        
        // If not full, return top item if exists
        if (buffer.Count > 0)
            return buffer.Dequeue();

        // Return null if empty
        return null;
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

    // ------------------ Public functions ---------------
    public void SetOutputAndMachine(IMachineOutput _output, IMachine _machine)
    {
        connectedOutput = _output;
        connectedMachine = _machine;
    }

    // ------------------ Private functions ---------------
    private void PullFromOutput()
    {
        IProcessable nextItem = connectedOutput.GetNextOutput();
        if(nextItem != null)
        {
            buffer.Enqueue(nextItem);
        }
    }
}

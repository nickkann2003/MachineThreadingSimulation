using System.Collections.Generic;
using UnityEngine;

public class MachineOutputBase : IMachineOutput
{
    // Queue of items and storage info
    private Queue<IProcessable> buffer;
    private int bufferSize = 5;

    private IMachine connectedMachine;
    private IMachineInput connectedInput;

    // ----------------- IMachineOutput members --------------------
    public IProcessable GetNextOutput()
    {
        // If full, dequeue and notify machine
        if (buffer.Count >= bufferSize)
        {
            IProcessable returnedItem = buffer.Dequeue();
            connectedMachine.NotifyOutput();
            return returnedItem;
        }

        // If not full, return top item if exists
        if (buffer.Count > 0)
            return buffer.Dequeue();

        // Return null if empty
        return null;
    }

    public bool GiveOutput(IProcessable output)
    {
        if (buffer.Count < bufferSize)
        {
            // If no items, perform add and notify machine
            if (buffer.Count == 0)
            {
                buffer.Enqueue(output);
                PushToInput();
                return true;
            }

            // Perform add
            buffer.Enqueue(output);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetInputReference(IMachineInput input)
    {
        connectedInput = input;
    }

    public void SetMachineReference(IMachine machine)
    {
        connectedMachine = machine;
    }

    // --------------- Public Functions --------------
    public void SetInputAndMachine(IMachineInput _input, IMachine _machine)
    {
        connectedInput = _input;
        connectedMachine = _machine;
    }

    // --------------- Private Functions --------------
    private void PushToInput()
    {
        if (connectedInput.GiveInput(buffer.Peek()))
            buffer.Dequeue();
    }
}

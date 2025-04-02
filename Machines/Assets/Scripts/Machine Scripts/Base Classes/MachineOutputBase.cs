using System.Collections.Generic;
using UnityEngine;

public class MachineOutputBase : IMachineOutput
{
    // Queue of items and storage info
    private Queue<IProcessable> buffer = new Queue<IProcessable>();
    private int bufferSize = 5;

    private IMachine connectedMachine;
    private IMachineInput connectedInput;
    private BarVisualController displayController;

    // ----------------- IMachineOutput members --------------------
    public IProcessable GetNextOutput()
    {
        IProcessable returnedItem = null;
        // If full, dequeue and notify machine
        if (buffer.Count >= bufferSize)
        {
            returnedItem = buffer.Dequeue();
            connectedMachine.NotifyOutput();
        } 
        else if (buffer.Count > 0)
        {
            returnedItem = buffer.Dequeue();
        }

        UpdateVisual();

        // Return null if empty
        return returnedItem;
    }

    public bool GiveOutput(IProcessable output)
    {
        if (buffer.Contains(output))
        {
            UpdateVisual();
            return true;
        }

        if (buffer.Count < bufferSize)
        {
            // If no items, perform add and notify machine
            if (buffer.Count == 0)
            {
                buffer.Enqueue(output);
                PushToInput();
                UpdateVisual();
                return true;
            }

            // Perform add
            buffer.Enqueue(output);
            UpdateVisual();
            return true;
        }
        else
        {
            UpdateVisual();
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

    public void SetDisplayReference(BarVisualController displayController)
    {
        this.displayController = displayController;
    }

    public void Stop()
    {
        // Machine output has no threads to stop
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
        if(connectedInput != null)
        {
            if (connectedInput.GiveInput(buffer.Peek()))
                buffer.Dequeue();
        }
    }
    
    private void UpdateVisual()
    {
        if (displayController != null)
        {
            displayController.SetProgress((float)buffer.Count / (float)bufferSize);
        }
    }



}

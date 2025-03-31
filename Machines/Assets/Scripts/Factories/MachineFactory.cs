using System.Collections.Generic;
using UnityEngine;
public class MachineFactory
{
    /// <summary>
    /// Given a machine enum, create an instance of the necessary underlying machine script and return it
    /// </summary>
    /// <param name="machineType">Machine enum being created</param>
    /// <returns>Specific machine script for the given machine type</returns>
    public static IMachine CreateMachine(Machines machineType)
    {
        switch (machineType)
        {
            case Machines.MachineBase: return new MachineBase();
            case Machines.InfiniteProducer: return new InfiniteProducer();
        }

        // Default case, return base machine
        return new MachineBase();
    }

    /// <summary>
    /// Given an input enum, create the corresponding input script and return it
    /// </summary>
    /// <param name="machineType">Input enum being created</param>
    /// <returns>Created input script matching enum</returns>
    public static IMachineInput CreateMachineInput(MachineInputs inputType)
    {
        switch (inputType)
        {
            case MachineInputs.MachineInputBase: return new MachineInputBase();
        }

        // Default case, return base
        return new MachineInputBase();
    }

    /// <summary>
    /// Given an output type enum, creates the underlying output script and returns it
    /// </summary>
    /// <param name="outputType">Output type</param>
    /// <returns>Underlying machine output script</returns>
    public static IMachineOutput CreateMachineOutput(MachineOutputs outputType)
    {
        switch (outputType)
        {
            case MachineOutputs.MachineOutputBase: return new MachineOutputBase();
        }

        // Default case, return base
        return new MachineOutputBase();
    }
}

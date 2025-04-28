using UnityEngine;
using System.Threading;

public class InfiniteProducer : MachineBase
{
    public InfiniteProducer()
    {
        t_Process = new Thread(Process);
        t_Process.IsBackground = true;
    }

    protected override void Process()
    {
        busy = true;
        // Loop this thread for as long as the machine is alive, infinitely giving output to next machine
        while (busy)
        {
            IProcessable createdItem = new ItemBase();
            connectedOutput.GiveOutput(createdItem);
            Thread.Sleep(4000);
        }
    }

    protected override void AwakeChecks()
    {
        if (!busy)
        {
            t_Process.Start();
        }
    }
}

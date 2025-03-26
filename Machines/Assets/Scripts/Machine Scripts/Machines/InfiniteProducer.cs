using UnityEngine;
using System.Threading;

public class InfiniteProducer : MachineBase
{
    public InfiniteProducer()
    {
        t_Process = new Thread(Process);
    }

    protected override void Process()
    {
        // Loop this thread for as long as the machine is alive, infinitely giving output to next machine
        while (true)
        {
            IProcessable createdItem = new ItemBase();
            connectedOutput.GiveOutput(createdItem);
            Thread.Sleep(2000);
        }
    }
}

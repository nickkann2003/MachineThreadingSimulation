public interface IMachineOutput
{
    // Get next output item, null if none available
    public IProcessable? GetNextOutput();

    // Give an item to this output, returns false if full
    public bool GiveOutput(IProcessable output);

    public void SetInputReference(IMachineInput input);
    public void SetMachineReference(IMachine machine);
    public void SetDisplayReference(BarVisualController displayController);

    public void Stop();
}

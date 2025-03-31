public interface IMachineInput
{
    // Gets the next item from this input, null if none available
    public IProcessable? GetNextInput();

    // Give this input an item, returns false if full
    public bool GiveInput(IProcessable input);

    public void SetOutputReference(IMachineOutput output);
    public void SetMachineReference(IMachine machine);

    public void Stop();
}

/// <summary>
/// Machine interface, defines connecting functionality
/// 
/// Machines must pull input from an IMachineInput and push output to a IMachineOutput
/// Implementations will implement threading
/// </summary>
public interface IMachine
{
    // Notify that new input is available
    public void NotifyInput();

    // Notify that output has a new availability
    public void NotifyOutput();

    public void SetInputReference(IMachineInput input);
    public void SetOutputReference(IMachineOutput output);
}

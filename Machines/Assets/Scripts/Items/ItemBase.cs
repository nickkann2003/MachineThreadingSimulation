using UnityEngine;

public class ItemBase : IProcessable
{
    int value = 0;
    public void Process(float amount)
    {
        value++;
        Debug.Log("PROCESSED: " + value);
    }
}

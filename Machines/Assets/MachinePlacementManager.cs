using UnityEngine;

public class MachinePlacementManager : MonoBehaviour
{
    // Simple singleton
    public static MachinePlacementManager instance;

    [SerializeField] private GameObject machinePrefab;
    [SerializeField] private Transform machineParentReference;

    private void Awake()
    {
        instance = this;
    }

    public void CreateMachine(int gridX, int gridY)
    {
        GameObject newMachine = Instantiate(machinePrefab, machineParentReference);
        newMachine.transform.localPosition = new Vector3(gridX, 0, gridY);
    }
}

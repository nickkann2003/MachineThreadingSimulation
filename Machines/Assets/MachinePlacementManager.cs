using System.Collections.Generic;
using System;
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
        GameObject newMachine = Instantiate(machinePrefab, machineParentReference);         // Instantiate the visual
        newMachine.transform.localPosition = new Vector3(gridX, 0, gridY);                  // Set its world position
        MachineVisualController con = newMachine.GetComponent<MachineVisualController>();   // Grab a reference to the script component
        GridManager.instance.AddMachine(con, gridX, gridY);                                 // Store in GridManager

        // Grab references to previous and next machine
        // This is done by accessing spots in the grid based on the new machines location, its pointing direction, and the grid of machines
        MachineVisualController? previousMachine = GridManager.instance.GetPrevious(gridX, gridY, con.direction);
        MachineVisualController? nextMachine = GridManager.instance.GetNext(gridX, gridY, con.direction);

        // Initialize the machine with the found neighbors
        con.InitializeMachine(previousMachine, nextMachine);
    }
}

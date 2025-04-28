using System.Collections.Generic;
using System;
using UnityEngine;

public class MachinePlacementManager : MonoBehaviour
{
    // Simple singleton
    public static MachinePlacementManager instance;

    [SerializeField] private GameObject machinePrefab;

    [SerializeField] private SO_Machine selectedMachine;
    [SerializeField] private SO_Machine infiniteProducer;

    [SerializeField] private Transform machineParentReference;

    private bool first = true;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Creates the currently selected machine at a given grid coordinate
    /// </summary>
    /// <param name="gridX">X coord</param>
    /// <param name="gridY">Y coord</param>
    public void CreateMachine(int gridX, int gridY)
    {
        GameObject newMachine = Instantiate(machinePrefab, machineParentReference); // Instantiate the visual
        newMachine.transform.localPosition = new Vector3(gridX, 0, gridY);                  // Set its world position
        MachineVisualController con = newMachine.GetComponent<MachineVisualController>();   // Grab a reference to the script component

        con.inputDirection = new Vector2(0, 0);
        con.outputDirection = new Vector2(0, 0);
        
        con.machineSO = selectedMachine; // Machine SO is the selected machine

        GridManager.instance.AddMachine(con, gridX, gridY);                                 // Store in GridManager
    }

    /// <summary>
    /// Connects two machines, given a source machines GridX and GridY and the direction it outputs to
    /// If target or source machine doesn't exist, does nothing
    /// </summary>
    /// <param name="gridX">Grid X position of source machine</param>
    /// <param name="gridY">Grid Y position of source machine</param>
    /// <param name="direction">Direction source machine is outputting to</param>
    public void ConnectMachine(int gridX, int gridY, Vector2 direction)
    {
        GridManager.instance.ConnectMachine(gridX, gridY, direction);
    }

    public void SetSelectedMachine(SO_Machine soMachine)
    {
        this.selectedMachine = soMachine;
    }
}

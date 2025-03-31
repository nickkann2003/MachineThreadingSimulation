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

    public void CreateMachine(int gridX, int gridY)
    {
        GameObject newMachine = newMachine = Instantiate(machinePrefab, machineParentReference); // Instantiate the visual
        newMachine.transform.localPosition = new Vector3(gridX, 0, gridY);                  // Set its world position
        MachineVisualController con = newMachine.GetComponent<MachineVisualController>();   // Grab a reference to the script component

        if (first)
        {
            con.machineSO = infiniteProducer; // First machine should be producer
            first = false;
        }
        else
        {
            con.machineSO = selectedMachine; // Machine SO is the selected machine
        }

        GridManager.instance.AddMachine(con, gridX, gridY);                                 // Store in GridManager
    }
}

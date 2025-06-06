using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    [SerializeField] GameObject emptyGridPrefab;

    // Size of the grid, in ints
    [SerializeField] private Vector2 size;

    private MachineVisualController[][] allMachines;

    private bool creatingGrid = true;
    private Queue<System.Tuple<int, int>> readyToMake = new Queue<System.Tuple<int, int>>();

    // -------------- Untiy Functions --------------
    private void Awake()
    {
        instance = this;    
    }

    public void Start()
    {
        allMachines = new MachineVisualController[(int)size.x][];

        Thread gridCreator = new Thread(ThreadedGridCreation);
        gridCreator.IsBackground = true;
        gridCreator.Start();
    }

    /// <summary>
    /// Update function, runs every
    /// </summary>
    private void Update()
    {
        // If creating the grid
        if (readyToMake.Count > 0)
        {
            bool looping = true;
            while (looping)
            {
                if (readyToMake.Count <= 0)
                {
                    looping = false;
                    break;
                }
                else
                {
                    Tuple<int, int> t = readyToMake.Dequeue();
                    CreateEmptyGrid(t.Item1, t.Item2);
                }
            }
        }
    }

    // -------------- Public functions ---------------
    /// <summary>
    /// Adds a given machine to the grid at a given X Y location
    /// </summary>
    /// <param name="con">Machine controller to add</param>
    /// <param name="x">X location</param>
    /// <param name="y">Y location (Z location in world coords)</param>
    public void AddMachine(MachineVisualController con, int x, int y)
    {
        int adjustedX = (int)size.x / 2 + x;
        int adjustedY = (int)size.y / 2 + y;
        if ((adjustedX >= 0 && adjustedX < allMachines.Length) && (adjustedY >= 0 && adjustedY < allMachines[0].Length))
            allMachines[adjustedX][adjustedY] = con;

        //ConnectNeighbors(adjustedX, adjustedY); // Connect new machine to its neighbors

        // Initialize the machine with the found neighbors
        con.InitializeMachine(null, null);

        con.KickoffFunctionality(); // Kickoff functionality
    }

    /// <summary>
    /// Connects a machine at a given location to the machine in a given direction
    /// </summary>
    /// <param name="x">GridX position of source machine</param>
    /// <param name="y">GridY position of source machine</param>
    /// <param name="direction">Direction to connect to</param>
    public void ConnectMachine(int x, int y, Vector2 direction)
    {
        MachineVisualController sourceMachine = GetController(x, y);
        MachineVisualController targetMachine = GetController(x + (int)direction.x, y + (int)direction.y);

        if (targetMachine == null || sourceMachine == null)
        {
            return;
        }

        sourceMachine.ConnectToDirection(direction, targetMachine);
    }

    /// <summary>
    /// Given a location and a direction, gets the previous machine along that direction
    /// </summary>
    /// <param name="x">X location of the current machine</param>
    /// <param name="y">Y location of the current machine</param>
    /// <param name="direction">Normalized, cardinal direction Vector to iterate backwards on</param>
    /// <returns>Previous machine, null if none found</returns>
    public MachineVisualController? GetPrevious(int adjustedX, int adjustedY, Vector2 direction)
    {
        try
        {
            return allMachines[adjustedX + (int)direction.x][adjustedY + (int)direction.y];
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Given a location and a direction, gets the next machine along that direction
    /// </summary>
    /// <param name="x">X location of the current machine</param>
    /// <param name="y">Y location of the current machine</param>
    /// <param name="direction">Normalized, cardinal direction Vector to iterate forwards on</param>
    /// <returns>Previous machine, null if none found</returns>
    public MachineVisualController? GetNext(int adjustedX, int adjustedY, Vector2 direction)
    {
        try
        {
            return allMachines[adjustedX + (int)direction.x][adjustedY + (int)direction.y];
        }
        catch
        {
            return null;
        }
    }

    // -------------- Private functions --------------
    private void CreateEmptyGrid(int x, int y)
    {
        GameObject newGridSpot = Instantiate(emptyGridPrefab, transform);
        newGridSpot.transform.localPosition = new Vector3(x, 0, y);
    }

    /// <summary>
    /// Returns a controller at a given grid position, null if none found
    /// </summary>
    /// <param name="x">Grid X</param>
    /// <param name="y">Grid Y</param>
    /// <returns>MachineVisualController at the given position, or null</returns>
    private MachineVisualController GetController(int x, int y)
    {
        MachineVisualController con = null;

        int adjustedX = (int)(size.x / 2) + x;
        int adjustedY = (int)(size.y / 2) + y;
        if ((adjustedX >= 0 && adjustedX < allMachines.Length) && (adjustedY >= 0 && adjustedY < allMachines[0].Length))
            return allMachines[adjustedX][adjustedY];

        return con;
    }

    /// <summary>
    /// Connects a given position in the grid to its neighbors
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void ConnectNeighbors(int adjustedX, int adjustedY)
    {
        MachineVisualController? con = allMachines[adjustedX][adjustedY];

        MachineVisualController? previousMachine = null;
        MachineVisualController? nextMachine = null;

        // Grab references to previous and next machine
        // This is done by accessing spots in the grid based on the new machines location, its pointing direction, and the grid of machines
        previousMachine = GetPrevious(adjustedX, adjustedY, con.inputDirection);
        nextMachine = GetNext(adjustedX, adjustedY, con.outputDirection);
    }

    /// <summary>
    /// Threaded function designed to progressively add tiles to be loaded over time
    /// Designed to reduce lag time on program startup
    /// </summary>
    private void ThreadedGridCreation()
    {
        for(int i = 0; i < size.x; i++)
        {
            allMachines[i] = new MachineVisualController[(int)size.y];
            for(int j = 0; j < size.y; j++)
            {
                allMachines[i][j] = null;
                Thread.Sleep(2);
                readyToMake.Enqueue(new System.Tuple<int, int>(i - (int)(size.x / 2), j - (int)(size.x / 2)));
            }
        }
        creatingGrid = false;
    }
}

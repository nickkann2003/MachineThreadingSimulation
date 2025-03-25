using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    [SerializeField] GameObject emptyGridPrefab;

    // Size of the grid, in ints
    [SerializeField] private Vector2 size;

    // -------------- Untiy Functions --------------
    private void Awake()
    {
        instance = this;    
    }

    public void Start()
    {
        // Loop and create grid of size
        for(int i = (int)-size.x/2; i < size.x/2f; i++)
        {
            for(int j = (int)-size.y/2; j < size.y/2; j++)
            {
                CreateEmptyGrid(i, j);
            }
        }
    }

    // -------------- Private functions --------------
    private void CreateEmptyGrid(int x, int y)
    {
        GameObject newGridSpot = Instantiate(emptyGridPrefab, transform);
        newGridSpot.transform.localPosition = new Vector3(x, 0, y);
    }
}

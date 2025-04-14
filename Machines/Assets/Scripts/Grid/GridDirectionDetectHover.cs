using UnityEngine;

public class GridDirectionDetectHover : GridDetectHover
{
    [SerializeField] private Vector2 direction;
    public override void OnMouseDown()
    {
        MachinePlacementManager.instance.ConnectMachine((int)transform.localPosition.x, (int)transform.localPosition.z, direction);
        GetComponent<Collider>().enabled = false;
    }
}

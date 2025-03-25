using UnityEngine;

public class GridDetectHover : MonoBehaviour
{
    [SerializeField] private GameObject unhoveredImage;
    [SerializeField] private GameObject hoveredImage;

    /// <summary>
    /// When the mouse enters this grid, set to hovered
    /// </summary>
    private void OnMouseEnter()
    {
        unhoveredImage.SetActive(false);
        hoveredImage.SetActive(true);
    }

    /// <summary>
    /// When the mouse exits this grid, set back to unhovered
    /// </summary>
    private void OnMouseExit()
    {
        unhoveredImage.SetActive(true);
        hoveredImage.SetActive(false);
    }

    /// <summary>
    /// On mouse down, create a machine at this location
    /// </summary>
    private void OnMouseDown()
    {
        MachinePlacementManager.instance.CreateMachine((int)transform.localPosition.x, (int)transform.localPosition.z);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridDetectHover : MonoBehaviour
{
    [SerializeField] private GameObject unhoveredImage;
    [SerializeField] private GameObject hoveredImage;
    [SerializeField] private List<GameObject> childDetectors = new List<GameObject>();

    private void Start()
    {
        foreach(GameObject c in childDetectors)
        {
            c.SetActive(false);
        }
    }

    /// <summary>
    /// When the mouse enters this grid, set to hovered
    /// </summary>
    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            unhoveredImage.SetActive(false);
            hoveredImage.SetActive(true);
        }
    }

    /// <summary>
    /// When the mouse exits this grid, set back to unhovered
    /// </summary>
    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            unhoveredImage.SetActive(true);
            hoveredImage.SetActive(false);
        }
    }

    /// <summary>
    /// On mouse down, create a machine at this location
    /// </summary>
    public virtual void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            MachinePlacementManager.instance.CreateMachine((int)transform.localPosition.x, (int)transform.localPosition.z);
            GetComponent<Collider>().enabled = false;
            foreach(GameObject child in childDetectors)
            {
                child.SetActive(true);
            }
        }
    }
}

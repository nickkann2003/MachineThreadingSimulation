using System.Collections.Generic;
using UnityEngine;

public class BarVisualController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer display;
    [SerializeField] private List<Sprite> progressSprites;

    private void Start()
    {
        SetProgress(0);
    }

    /// <summary>
    /// Sets the displayed progress of this bar
    /// </summary>
    /// <param name="p">Float between 0 and 1 representing progress</param>
    public void SetProgress(float p)
    {
        int pAdjusted = Mathf.FloorToInt(p * (progressSprites.Count));
        display.sprite = progressSprites[pAdjusted];
    }
}

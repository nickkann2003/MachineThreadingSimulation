using System.Collections.Generic;
using UnityEngine;

public class BarVisualController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer display;
    [SerializeField] private List<Sprite> progressSprites;

    float progress = 0f;
    private bool update = false;

    private void Start()
    {
        SetProgress(0);
    }

    private void Update()
    {
        if (update)
        {
            performProgressUpdate();
            update = false;
        }
    }

    /// <summary>
    /// Sets the displayed progress of this bar
    /// </summary>
    /// <param name="p">Float between 0 and 1 representing progress</param>
    public void SetProgress(float p)
    {
        update = true;
        progress = p;
    }

    private void performProgressUpdate()
    {
        if(progress > 0.99f)
        {
            progress = 0.99f;
        }
        int pAdjusted = Mathf.FloorToInt(progress * (progressSprites.Count));
        if(pAdjusted == 0 && progress > 0)
        {
            pAdjusted += 1;
        }
        display.sprite = progressSprites[pAdjusted];
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BarVisualController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer display;
    [SerializeField] private List<Sprite> progressSprites;
    [SerializeField] private TextMeshPro textDisplay;

    float progress = 0f;
    private bool update = false;
    private int currentFill = 0;
    private int maxFill = 0;

    private void Start()
    {
        SetProgress(0, 0, 0);
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
    public void SetProgress(float p, int cFill, int mFill)
    {
        update = true;
        progress = p;
        this.currentFill = cFill;
        this.maxFill = mFill;
    }

    private void performProgressUpdate()
    {
        int pAdjusted = Mathf.FloorToInt(progress * (progressSprites.Count-2));
        pAdjusted += 1;
        if (progress == 0)
            pAdjusted = 0;
        if (progress == 1)
            pAdjusted = progressSprites.Count-1;
        textDisplay.text = string.Format("{0} / {1}", currentFill, maxFill);
        display.sprite = progressSprites[pAdjusted];
    }
}

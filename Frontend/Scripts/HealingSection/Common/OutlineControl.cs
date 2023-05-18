using UnityEngine;
using UnityEngine.UI;

public class OutlineControl : MonoBehaviour
{
    public float range = 100f;
    public Text actionText;
    private RaycastHit hitInfo, preHitInfo;

    private void Update()
    {
        ShootLayCast();
    }

    private void ShowObjectHighlight(Transform parent, int idx)
    {
        cakeslice.Outline outLine = parent.GetComponent<cakeslice.Outline>();

        if (outLine != null)
        {
            outLine.enabled = true;
            outLine.eraseRenderer = false;
            ShowActionText(parent);
        }

        foreach (Transform child in parent)
        {
            ShowObjectHighlight(child, idx + 1);
        }
    }

    private void ClearObjectHighlight(Transform parent)
    {
        cakeslice.Outline outLine = parent.GetComponent<cakeslice.Outline>();

        if (outLine != null)
        {
            outLine.enabled = false;
            outLine.eraseRenderer = true;
        }

        foreach (Transform child in parent)
        {
            ClearObjectHighlight(child);
        }

        // Hide the text when the outline is cleared
        actionText.gameObject.SetActive(false);
    }

    private void ShootLayCast()
    {
        if (preHitInfo.transform != null)
        {
            ClearObjectHighlight(preHitInfo.transform);
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range))
        {
            ShowObjectHighlight(hitInfo.transform, 0);
        }

        preHitInfo = hitInfo;
    }

    private void ShowActionText(Transform transform)
    {
        string layerName = LayerMask.LayerToName(transform.gameObject.layer);

        if (layerName == "SunBed")
        {
            actionText.text =  " 눕기 " + "<color=red>" + "(F)" + "</color>";

            // Transform object position to screen point
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

            // Set the position of actionText to screenPos
            actionText.transform.position = screenPos;

            actionText.gameObject.SetActive(true);
        }
    }
}
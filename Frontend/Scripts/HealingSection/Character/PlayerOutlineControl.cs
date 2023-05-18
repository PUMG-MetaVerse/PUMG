using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerOutlineControl : MonoBehaviourPunCallbacks
{
    public cakeslice.Outline currentOutline;
    private Text actionText;
    private bool isAbleToLayDown = false;
    private bool isAbleToSitDown = false;
    private bool isGetballoons = false;
    private bool isFire = false;
    public FireInteraction fireInteraction;

    private void Start()
    {
        actionText = GameObject.Find("OutlineText").GetComponent<Text>();
        actionText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (currentOutline != null && LayerMask.LayerToName(currentOutline.gameObject.layer) == "CampFire")
        {
            isFire = fireInteraction.IsFireOn();
            if (isFire)
            {
                DisplayActionText(currentOutline.transform, " 불 끄기" + "<color=red>" + "(F)" + "</color>");
            }
            else
            {
                DisplayActionText(currentOutline.transform, " 불 켜기" + "<color=red>" + "(F)" + "</color>");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return; 

        var outline = other.gameObject.GetComponent<cakeslice.Outline>();
        if (outline != null)
        {
            currentOutline = outline;
            currentOutline.enabled = true;
            currentOutline.eraseRenderer = false;

            fireInteraction = other.gameObject.GetComponentInParent<FireInteraction>();
            ShowActionText(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline.eraseRenderer = true;
            currentOutline = null;

            ClearActiveText(other.transform);
        }
    }

    public void ShowActionText(Transform transform)
    {
        string layerName = LayerMask.LayerToName(transform.gameObject.layer);

        if (layerName == "SunBed")
        {
            DisplayActionText(transform, " 눕기 " + "<color=red>" + "(F)" + "</color>");
            isAbleToLayDown = true;
        }

        if (layerName == "Fishing")
        {
            DisplayActionText(transform, " 앉기 " + "<color=red>" + "(F)" + "</color>");
            isAbleToSitDown = true;
        }

        if (layerName == "Balloon")
        {
            DisplayActionText(transform, " 풍선 흭득하기" + "<color=red>" + "(F)" + "</color>\n천천히 떨어질 수 있습니다.");
            isGetballoons = true;
        }

        if (layerName == "CampFire")
        {
            isFire = fireInteraction.IsFireOn();
            if (isFire)
            {
                DisplayActionText(transform, " 불 끄기" + "<color=red>" + "(F)" + "</color>");
            }
            else
            {
                DisplayActionText(transform, " 불 켜기" + "<color=red>" + "(F)" + "</color>");
            }
        }
    }

    public void DisplayActionText(Transform transform, string text)
    {
        actionText.text = text;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.GetComponent<Renderer>().bounds.center);
        actionText.transform.position = screenPos;

        actionText.gameObject.SetActive(true);
    }


    public  void ClearActiveText(Transform transform)
    {
        string layerName = LayerMask.LayerToName(transform.gameObject.layer);

        if (layerName == "SunBed")
        {
            isAbleToLayDown = false;
        }

        if (layerName == "Fishing")
        {
            isAbleToSitDown = false;
        }

        if (layerName == "Balloon")
        {
            isGetballoons = false;
        }

        if (layerName == "CampFire")
        {
            isFire = false;
        }

        actionText.gameObject.SetActive(false);
    }
     public void ClearOutline()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline.eraseRenderer = true;
            currentOutline = null;
        }

        actionText.gameObject.SetActive(false);
    }
}
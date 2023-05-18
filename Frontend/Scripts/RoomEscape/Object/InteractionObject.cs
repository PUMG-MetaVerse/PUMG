using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    public static bool objectDetailTextActivated;
    public string objectName;
    public string[] messages;

    // 서랍, 상자 같은 경우 자신에게 달려있는 자물쇠
    public GameObject myLock;

    private InteractionObject parentInteractionObject;

    void Start()
    {
        // 부모 오브젝트의 InteractionObject 객체를 가져옵니다.
        parentInteractionObject = GetComponent<InteractionObject>();

        // 자식 오브젝트에 부모 오브젝트의 InteractionObject 객체를 참조로 설정합니다.
        // 이렇게 하는 이유는 오브젝트가 여러 자식 오브젝트들로 이루어져 있을 때, 레이캐스팅을 하면
        // 자식 오브젝트가 가로채서 정상적으로 레이캐스팅이 안되는 경우가 있음
        // 따라서 자식 오브젝트들에도 모두 스크립트를 적용하도록함
        // 향후 필요하지는 모르겠음. 테스트 필요.
        AddScriptToChildren(transform);

        objectDetailTextActivated = false;
    }

    void AddScriptToChildren(Transform parent)
    {
        // 자식 오브젝트들에게 부모 오브젝트의 InteractionObject 객체를 참조로 설정하는 메소드입니다.
        foreach (Transform child in parent)
        {
            // 자식 오브젝트에 부모 오브젝트의 InteractionObject 객체를 참조로 설정합니다.
            child.gameObject.AddComponent<InteractionObject>();
            child.gameObject.GetComponent<InteractionObject>().objectName = parentInteractionObject.objectName;
            child.gameObject.GetComponent<InteractionObject>().messages = parentInteractionObject.messages;

            // 자식 오브젝트의 자식 오브젝트들에게도 부모 오브젝트의 ActionController 객체를 참조로 설정합니다.
            if (child.childCount > 0)
            {
                AddScriptToChildren(child);
            }
        }
    }
}

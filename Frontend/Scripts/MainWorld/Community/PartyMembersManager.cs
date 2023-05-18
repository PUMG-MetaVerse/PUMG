using System.Collections.Generic;
using UnityEngine;

public class PartyMembersManager : MonoBehaviour
{
    public static PartyMembersManager Instance;

    public Dictionary<int, string> partyMembers;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetPartyMembers(Dictionary<int, string> newPartyMembers)
    {
        partyMembers = newPartyMembers;
    }
    public void ClearPartyMembers()
    {
        partyMembers.Clear();
    }


    // PartyMembers를 관리하는 기타 메서드들...
}

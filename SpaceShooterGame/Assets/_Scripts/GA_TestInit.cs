using UnityEngine;
using GameAnalyticsSDK;

public class GA_TestInit : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Calling GameAnalytics.Initialize()");
        GameAnalytics.SetCustomId("myCustomUserId");
        GameAnalytics.Initialize();
        GameAnalytics.NewDesignEvent("Test:Initialization");
    }
}
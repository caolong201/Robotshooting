using GameAnalyticsSDK;
using UnityEngine;

public class GameAnalyticsManager : SingletonMonoAwake<GameAnalyticsManager>
{
    public override void OnAwake()
    {
        base.OnAwake();
        // Initialize GameAnalytics
        GameAnalytics.Initialize();
    }

    public void TrackEvent(string eventName)
    {
        GameAnalytics.NewDesignEvent(eventName);
    }
}
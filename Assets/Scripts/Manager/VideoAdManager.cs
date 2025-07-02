using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EVideoAd
{
    None = 0,
    Video1,
    Video2,
    Video3
}

public class VideoAdManager : MonoBehaviour
{
    public EVideoAd videoAd = EVideoAd.None;
   
}

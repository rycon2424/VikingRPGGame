using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class DebugMenu : MonoBehaviour
{
    public enum GraphicsSettings {fastest, fast, simple, good, beautifull, fantastic }
    public GraphicsSettings quality;

    [Button]
    void UpdateQuality()
    {
        switch (quality)
        {
            case GraphicsSettings.fastest:
                QualitySettings.SetQualityLevel(0);
                break;
            case GraphicsSettings.fast:
                QualitySettings.SetQualityLevel(1);
                break;
            case GraphicsSettings.simple:
                QualitySettings.SetQualityLevel(2);
                break;
            case GraphicsSettings.good:
                QualitySettings.SetQualityLevel(3);
                break;
            case GraphicsSettings.beautifull:
                QualitySettings.SetQualityLevel(4);
                break;
            case GraphicsSettings.fantastic:
                QualitySettings.SetQualityLevel(5);
                break;
            default:
                break;
        }
    }

}
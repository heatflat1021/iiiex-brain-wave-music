using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmotivUnityPlugin; 

public class CerebrumArea
{
    public enum CerebrumArea_t: int
    {
        LEFT_FRONTAL_LOBE, LEFT_OCCIPITAL_LOBE, RIGHT_FRONTAL_LOBE, RIGHT_OCCIPITAL_LOBE,
    }

    private static Dictionary<CerebrumArea_t, string> cerebrumAreaStringMap = new Dictionary<CerebrumArea_t, string>()
    {
        { CerebrumArea_t.LEFT_FRONTAL_LOBE, "leftFrontalLobe" },
        { CerebrumArea_t.LEFT_OCCIPITAL_LOBE, "leftOccipitalLobe" },
        { CerebrumArea_t.RIGHT_FRONTAL_LOBE, "rightFrontalLobe" },
        { CerebrumArea_t.RIGHT_OCCIPITAL_LOBE, "rightOccipitalLobe" },
    };

    public static CerebrumArea_t? ConvertChannelToCerebrumArea(Channel_t channel)
    {
        switch (channel)
        {
            case Channel_t.CHAN_AF3:
            case Channel_t.CHAN_F7:
            case Channel_t.CHAN_F3:
            case Channel_t.CHAN_FC5:
                return CerebrumArea_t.LEFT_FRONTAL_LOBE;
            case Channel_t.CHAN_T7:
            case Channel_t.CHAN_P7:
            case Channel_t.CHAN_O1:
                return CerebrumArea_t.LEFT_OCCIPITAL_LOBE;
            case Channel_t.CHAN_AF4:
            case Channel_t.CHAN_F8:
            case Channel_t.CHAN_F4:
            case Channel_t.CHAN_FC6:
                return CerebrumArea_t.RIGHT_FRONTAL_LOBE;
            case Channel_t.CHAN_T8:
            case Channel_t.CHAN_P8:
            case Channel_t.CHAN_O2:
                return CerebrumArea_t.RIGHT_OCCIPITAL_LOBE;
            default:
                return null;
        }
    }

    public static string ConvertCerebrumAreaTToString(CerebrumArea_t cerebrumArea)
    {
        foreach (KeyValuePair<CerebrumArea_t, string> kvp in cerebrumAreaStringMap)
        {
            if (kvp.Key == cerebrumArea)
            {
                return kvp.Value;
            }
        }

        return null;
    }
}

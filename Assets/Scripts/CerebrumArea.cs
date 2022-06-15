using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmotivUnityPlugin; 

public class CerebrumArea
{
    public enum CerebrumArea_t: int
    {
        LEFT_OUTER, LEFT_CENTER, RIGHT_OUTER, RIGHT_CENTER,
    }

    private static Dictionary<CerebrumArea_t, string> cerebrumAreaStringMap = new Dictionary<CerebrumArea_t, string>()
    {
        { CerebrumArea_t.LEFT_OUTER, "leftOuter" },
        { CerebrumArea_t.LEFT_CENTER, "leftCenter" },
        { CerebrumArea_t.RIGHT_OUTER, "rightOuter" },
        { CerebrumArea_t.RIGHT_CENTER, "rightCenter" },
    };

    public static CerebrumArea_t? ConvertChannelToCerebrumArea(Channel_t channel)
    {
        switch (channel)
        {
            case Channel_t.CHAN_AF3:
            case Channel_t.CHAN_F7:
            case Channel_t.CHAN_T7:
            case Channel_t.CHAN_P7:
                return CerebrumArea_t.LEFT_OUTER;
            case Channel_t.CHAN_F3:
            case Channel_t.CHAN_FC5:
            case Channel_t.CHAN_O1:
                return CerebrumArea_t.LEFT_CENTER;
            case Channel_t.CHAN_AF4:
            case Channel_t.CHAN_F8:
            case Channel_t.CHAN_T8:
            case Channel_t.CHAN_P8:
                return CerebrumArea_t.RIGHT_OUTER;
            case Channel_t.CHAN_F4:
            case Channel_t.CHAN_FC6:
            case Channel_t.CHAN_O2:
                return CerebrumArea_t.RIGHT_CENTER;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Band {
    public enum Band_t : int
    {
        THETA, ALPHA, LOW_BETA, HIGH_BETA
    }

    public string ConvertBandToString(Band_t band)
    {
        switch (band)
        {
            case Band_t.THETA:
                return "theta";
            case Band_t.ALPHA:
                return "alpha";
            case Band_t.LOW_BETA:
                return "lowbeta";
            case Band_t.HIGH_BETA:
                return "highbeta";
            default:
                return null;
        }
    }
}

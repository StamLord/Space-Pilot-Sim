using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Utility
{
    public static decimal FloatToDecimal(float number, int decimals)
    {
        decimal dec = Convert.ToDecimal(number);
        dec = Math.Round(dec, decimals);
        return dec;
    }
}

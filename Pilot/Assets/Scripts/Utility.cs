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

    public static Vector3 StringToVector3(string str)
    {
        int third = str.Length / 3;
        int x = int.Parse(str.Substring(0,third));
        int y = int.Parse(str.Substring(third,third));
        int z = int.Parse(str.Substring(third*2,third));

        return new Vector3(x,y,z);
    }
}

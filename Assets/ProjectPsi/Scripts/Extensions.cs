using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static float Remap(this float input, float inputMin, float inputMax, float min, float max)
    {
        return min + (input - inputMin) * (max - min) / (inputMax - inputMin);
    }
}

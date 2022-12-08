using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RangeF
{
    public float min;
    public float max;

    public RangeF(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public float GenerateNumber()
    {
        return Random.Range(min, max);
    }
}

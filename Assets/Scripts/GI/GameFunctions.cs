using UnityEngine;

public static class GameFunctions {

	public static float MapRange(float value, float inRangeA, float inRangeB, float outRangeA, float outRangeB)
    {
        return (value - inRangeA) / (outRangeA - inRangeA) * (outRangeB - inRangeB) + inRangeB;
    }

    public static float MapRangeClamped(float value, float inRangeA, float inRangeB, float outRangeMin, float outRangeMax)
    {
        return Mathf.Clamp(MapRange(value, inRangeA, inRangeB, outRangeMin, outRangeMax), outRangeMin, outRangeMax);
    }
}

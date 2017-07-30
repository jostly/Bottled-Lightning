using UnityEngine;

[System.Serializable]
public struct MinMax
{
    public float min;
    public float max;

    public float Rnd()
    {
        return Random.Range(min, max);
    }
}

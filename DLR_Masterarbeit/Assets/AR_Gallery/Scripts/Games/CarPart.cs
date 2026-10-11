using UnityEngine;

public class CarPart : MonoBehaviour
{
    [SerializeField] private int kmhValue;

    public int GetKmhValue()
    {
        return kmhValue;
    }
}
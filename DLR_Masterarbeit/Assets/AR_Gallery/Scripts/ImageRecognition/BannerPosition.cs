using UnityEngine;

public class BannerPosition : MonoBehaviour
{
    public string pictureName;
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] float z;
    [SerializeField] float size;

    public float X => x;
    public float Y => y;
    public float Z => z;
    public float Size => size;
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderLogic : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Data data;

    public void UpdateText()
    {
        int age = Mathf.RoundToInt(slider.value);
        text.text = age.ToString();
        data.SetAge(age);
    }
}
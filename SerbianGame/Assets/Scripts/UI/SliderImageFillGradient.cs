using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SliderImageFillGradient : MonoBehaviour
{
    [SerializeField] private Gradient gradient = null;
    [SerializeField] private Image image = null;

    public Slider slider = null;
    public float value;

    private void Update()
    {
        if (!image || !slider)
            return;

        slider.value = value;
        image.color = gradient.Evaluate(value);
    }
}

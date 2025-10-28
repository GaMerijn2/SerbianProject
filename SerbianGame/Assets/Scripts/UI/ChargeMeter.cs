using UnityEngine;

public class ChargeMeter : MonoBehaviour
{
    [SerializeField] private SliderImageFillGradient sliderImageFillGradient;
    [SerializeField] private FrogMovement frogMovement;


    private void Awake()
    {
        sliderImageFillGradient.slider.maxValue = frogMovement.maxChargeTime;
    }

    private void Update()
    {
        if (!sliderImageFillGradient || !frogMovement)
            return;

        float mappedChargeAmount = Map(0, frogMovement.maxChargeTime, 0, 1, frogMovement.chargeTimer);
        sliderImageFillGradient.value = mappedChargeAmount;
    }

    private float Map(
       float inMin,
       float inMax,
       float outMin,
       float outMax,
       float value)
    {
        return (value - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
    }
}

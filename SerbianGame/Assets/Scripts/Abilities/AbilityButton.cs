using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public Image radialImage;
    public Image abilityIcon;
    public int index;
    InputControl assignedInput;

    public event Action<int> OnButtonPressed = delegate { };

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => OnButtonPressed(index));
    }

    void Update()
    {
        if (assignedInput is ButtonControl button && button.wasPressedThisFrame)
            OnButtonPressed(index);
    }

    public void RegisterListener(Action<int> listener)
    {
        OnButtonPressed += listener;
    }

    public void Initialize(int index, InputControl input)
    {
        this.index = index;
        this.assignedInput = input;
    }

    public void UpdateButtonSprite(Sprite newIcon)
    {
        abilityIcon.sprite = newIcon;
    }

    public void UpdateRadialFill(float progress)
    {
        if (radialImage)
            radialImage.fillAmount = progress;
    }
}

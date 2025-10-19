using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityView : MonoBehaviour
{
    [SerializeField] public AbilityButton[] buttons;

    [SerializeField] private InputControl[] inputs;

    void Awake()
    {
        inputs = new InputControl[]
        {
            Mouse.current.leftButton,
            //Mouse.current.rightButton,
            //Keyboard.current.eKey,
        };

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i >= inputs.Length)
            {
                Debug.LogError("Not enough inputs for the number of buttons.");
                break;
            }

            buttons[i].Initialize(i, inputs[i]);
        }
        UpdateRadial(0);
    }

    public void UpdateRadial(float progress)
    {
        if (float.IsNaN(progress))
        {
            progress = 0;
        }
        Array.ForEach(buttons, button => button.UpdateRadialFill(progress));
    }

    public void UpdateButtonSprites(IList<Ability> abilities)
    {
        if (abilities.Count < 0)
            return;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < abilities.Count)
                buttons[i].UpdateButtonSprite(abilities[i].data.icon);
            else
                buttons[i].gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowPassword : MonoBehaviour
{
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button toggleVisibility;
    [SerializeField] private Sprite visibleIcon;
    [SerializeField] private Sprite hiddenIcon;

    private bool isPasswordVisible = false;

    public void TogglePasswordVisibility()
    {
        // Toggle the visibility of the password input field
        passwordInput.inputType = isPasswordVisible ? TMP_InputField.InputType.Password : TMP_InputField.InputType.Standard;

        // Force the password field to lose focus and then regain it to update its visual state
        passwordInput.DeactivateInputField();
        passwordInput.ActivateInputField();

        // Update the button icon to reflect the visibility state
        Image buttonImage = toggleVisibility.GetComponent<Image>();
        buttonImage.sprite = isPasswordVisible ? hiddenIcon : visibleIcon;

        // Toggle the flag
        isPasswordVisible = !isPasswordVisible;
    }
}

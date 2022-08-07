// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All rights reserved.

using System;
using UnityEngine.UI;

namespace Naninovel
{
    public class ScriptableDropdown : ScriptableUIControl<Dropdown>
    {
        public event Action<int> OnDropdownValueChanged;

        protected override void BindUIEvents ()
        {
            UIComponent.onValueChanged.AddListener(OnValueChanged);
            UIComponent.onValueChanged.AddListener(InvokeOnDropdownValueChanged);
        }

        protected override void UnbindUIEvents ()
        {
            UIComponent.onValueChanged.RemoveListener(OnValueChanged);
            UIComponent.onValueChanged.RemoveListener(InvokeOnDropdownValueChanged);
        }

        protected virtual void OnValueChanged (int value) { }

        private void InvokeOnDropdownValueChanged (int value)
        {
            OnDropdownValueChanged?.Invoke(value);
        }
    }
}

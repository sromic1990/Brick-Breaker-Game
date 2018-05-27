using UnityEngine;
using System.Collections;
using System;

[AttributeUsage(AttributeTargets.Method)]
public class ButtonInspector : Attribute
{
    public string buttonName;
    public ButtonWorkMode mode;
    public ButtonInspector(string buttonName, ButtonWorkMode mode = ButtonWorkMode.Both)
    {
        this.buttonName = buttonName;
        this.mode = mode;
    }

    public ButtonInspector(ButtonWorkMode mode = ButtonWorkMode.Both)
    {
        this.buttonName = "";
        this.mode = mode;
    }
}

[Flags]
public enum ButtonWorkMode
{
    EditorOnly = 1,
    RuntimeOnly = 2,
    Both = EditorOnly | RuntimeOnly,
}

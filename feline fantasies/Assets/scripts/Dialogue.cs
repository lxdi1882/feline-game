using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;  // For using the icon

[System.Serializable]
public class Dialogue
{
    public string name;
    [TextArea(3, 10)]
    public string[] sentences;
    public Sprite icon;  // The icon for this dialogue
}

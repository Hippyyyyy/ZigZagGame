using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color", menuName = "Scriptable Objects/" + "Color")]
public class ColorSO : ScriptableObject
{
    public int Id;
    public Sprite Icon;
    public Transform Car;
    public int Price;
}

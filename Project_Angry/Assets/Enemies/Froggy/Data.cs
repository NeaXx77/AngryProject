using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data", fileName = "new Data")]
public class Data : ScriptableObject
{
    public float bouncingForce;
    public float knockbackForce;
}

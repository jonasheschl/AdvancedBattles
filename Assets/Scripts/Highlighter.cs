using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{

    public SpriteRenderer marking;

    public bool Highlighted
    {
        get => marking.enabled;
        set => marking.enabled = value;
    }
    
}

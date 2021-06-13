using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public bool playerStartsTalking;

    [TextArea(3, 10)]
    public string[] sentences;
}

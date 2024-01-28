using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hotspot : MonoBehaviour
{
    public abstract void Trigger();

    public abstract void LeaveHotspot();
}

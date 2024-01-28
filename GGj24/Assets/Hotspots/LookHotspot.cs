using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookHotspot : Hotspot
{
   //public lookTime

    public override void Trigger()
    {
        // guarda per N secondi
    }

    public override void LeaveHotspot()
    {
        HotspotsManager.instance.GoToNextHotspot();
    }
}

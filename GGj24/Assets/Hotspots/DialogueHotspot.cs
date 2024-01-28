using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHotspot : Hotspot
{

    // public dialogo da mostrare
    
    public override void Trigger()
    {
        // mostra dialogo
    }

    public override void LeaveHotspot()
    {
        //interrompi dialogo e passa all'hotspot dopo
        HotspotsManager.instance.GoToNextHotspot();
    }
}

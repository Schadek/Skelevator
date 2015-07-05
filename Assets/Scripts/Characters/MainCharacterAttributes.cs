using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MainCharacterAttributes : MonoBehaviour {

    public static MainCharacterAttributes Instance;
    public Skulls heldSkull;

    private void Awake()
    {
        Instance = this;
    }

}

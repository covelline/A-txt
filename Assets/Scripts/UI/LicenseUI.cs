using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LicenseUI : MonoBehaviour
{
    public GameObject LicenseUIObj;

    public void onClickClose()
    {
        Destroy(LicenseUIObj);
    }
}

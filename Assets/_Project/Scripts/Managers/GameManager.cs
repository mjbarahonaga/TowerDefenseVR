using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{
    public static Action<Vector3> OnPositionCharacter;
    public XROrigin XROrigin;
    public RespawnData[] RespawnLocations = new RespawnData[4];

    private void OnValidate()
    {
        Utils.ValidationUtility.SafeOnValidate(() =>
        {
            if (this == null) return;
            if(XROrigin == null) XROrigin = FindObjectOfType<XROrigin>();
            if (RespawnLocations.Length == 0) RespawnLocations = FindObjectsOfType<RespawnData>();
        });
    }

    private void Start()
    {
        TeleportationProviderCustom.OnTeleport += SendPositionXR;
    }

    private void OnDestroy()
    {
        TeleportationProviderCustom.OnTeleport -= SendPositionXR;
    }

    private void SendPositionXR()
    {
        OnPositionCharacter?.Invoke(XROrigin.transform.position);
    }
}

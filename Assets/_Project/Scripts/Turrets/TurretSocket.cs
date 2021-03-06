using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(XRSimpleInteractable))]
[RequireComponent(typeof(MeshRenderer))]
public class TurretSocket : MonoBehaviour
{
    
    public float DistanceToShow = 50f;
    private Transform _myTransform;
    private MeshRenderer _meshRenderer;
    public GameObject PanelStore;

    private void OnValidate()
    {
        Utils.ValidationUtility.SafeOnValidate(() =>
        {
            if (this == null) return;

            if(_myTransform == null) _myTransform = GetComponent<Transform>();
            if(_meshRenderer == null) _meshRenderer = GetComponent<MeshRenderer>();
        });
    }

    private void Awake()
    {
        OnValidate();   //< To check cached variables

        GameManager.OnPositionCharacter += CheckIfThisCanShow;
    }

    private void OnDestroy()
    {
        GameManager.OnPositionCharacter -= CheckIfThisCanShow;
    }

    private void CheckIfThisCanShow(Vector3 characterPos)
    {
        var distance = (_myTransform.position - characterPos).sqrMagnitude;

        if (distance >= DistanceToShow) {
            _meshRenderer.enabled = false;
            PanelStore.SetActive(false);
            return; 
        }

        _meshRenderer.enabled = true;
        // Some cool things?
    }
}

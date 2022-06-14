using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReferencesManager : MonoBehaviour
{
#if UNITY_EDITOR
    public XRInteractionManager InteractionManager;

    private void OnValidate()
    {
        Utils.ValidationUtility.SafeOnValidate(() =>
        {
            if (this == null) return;
            if(InteractionManager == null) InteractionManager = FindObjectOfType<XRInteractionManager>();
        });
    }

    [Button("Refresh Interaction Manager")]
    public void RefreshReferencesToInteractionManager() 
    { 
        if(InteractionManager == null) return;

        var listTeleportArea = FindObjectsOfType<TeleportationArea>();
        int length = listTeleportArea.Length;
        for (int i = 0; i < length; i++) listTeleportArea[i].interactionManager = InteractionManager;

        var listSockets = FindObjectsOfType<XRSocketInteractor>();
        length = listSockets.Length;
        for (int i = 0; i < length; i++) listSockets[i].interactionManager = InteractionManager;

        var listInteractables = FindObjectsOfType<XRGrabInteractable>();
        length = listInteractables.Length;
        for (int i = 0; i < length; i++) listInteractables[i].interactionManager = InteractionManager;

        var listSimpleInteraction = FindObjectsOfType<XRSimpleInteractable>();
        length = listSimpleInteraction.Length;
        for (int i = 0; i < length; i++) listSimpleInteraction[i].interactionManager = InteractionManager;

        Debug.Log("Complete");
    }
#endif
}

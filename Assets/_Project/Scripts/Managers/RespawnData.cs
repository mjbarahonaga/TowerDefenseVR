using UnityEngine;

// This provides where the respawn position is and its target.
public class RespawnData : MonoBehaviour
{
    public Transform Target;
    public Vector3 MyPositionInWorld;

    private void OnValidate()
    {
        Utils.ValidationUtility.SafeOnValidate(() =>
        {
            MyPositionInWorld = transform.position;
        });
    }

}

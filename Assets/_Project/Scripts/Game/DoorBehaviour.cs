using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DoorBehaviour : MonoBehaviour
{

    //private void OnValidate()
    //{
    //    Utils.ValidationUtility.SafeOnValidate(() =>
    //    {
    //        //gameObject.layer = LayerMask.NameToLayer("Doors");  //< Just Collisions with Enemies
    //        //GetComponent<Collider>().isTrigger = true;
    //    });
    //}

    private void OnTriggerEnter(Collider other)
    {
        EnemyBehaviour enemy = other.gameObject.GetComponent<EnemyBehaviour>();
        if (enemy == null) return;
        if (enemy.Initialising == true) return; //< If they are activated close to doors, avoid this frame to be deactivated
        enemy.Initialising = true;  //< it will work like a flag, sometimes same enemy enter twice here
        GameManager.Instance.TakeDamage();

        // Send back the enemy to the pool
        GameManager.Instance.ReturnEnemyToPool(enemy);
    }

}

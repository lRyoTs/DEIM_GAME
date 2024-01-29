using UnityEngine;

/// <summary>
/// Script the serves as a base class for future enemies implementation
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    #region Variables
    protected string enemyName;
    protected int level;
    protected Life enemyLifeInfo;
    protected int expValue; //Experience points given to the player once the enemy is destroyed

    protected float actionTimer;
    #endregion

    protected void ManageDeath() {
        gameObject.SetActive(false);
    }

    protected void OnDisable() {
        //Store expvalue in a variable to call once the battle is done
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)//collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Collided");
            //Get projectile damage
            int damage = -5; //Testing value
            enemyLifeInfo.UpdateLife(damage);
        }
    }
}

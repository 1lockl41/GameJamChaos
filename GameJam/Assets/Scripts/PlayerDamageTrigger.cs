using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageTrigger : MonoBehaviour
{
    public float damage;
    public bool punchUp;

    Vector3 startPos;

    public bool isProjectile = false;

	void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == "Player" && !col.isTrigger)
        {
            Vector2 pushPoint;

            if(!isProjectile)
            {
                pushPoint = new Vector2(this.transform.parent.position.x, this.transform.parent.position.y);
            }
            else
            {
                pushPoint = new Vector2(this.transform.position.x, this.transform.position.y);
            }

            col.GetComponent<UnityStandardAssets._2D.Platformer2DUserControl>().TakeDamage(damage, pushPoint, punchUp);

            if(isProjectile)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}

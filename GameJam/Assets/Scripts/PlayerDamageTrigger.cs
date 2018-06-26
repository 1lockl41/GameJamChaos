using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageTrigger : MonoBehaviour
{
    public float damage;
    public bool punchUp;

	void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == "Player" && !col.isTrigger)
        {
            col.GetComponent<UnityStandardAssets._2D.Platformer2DUserControl>().TakeDamage(damage, new Vector2(this.transform.parent.position.x, this.transform.parent.position.y), punchUp);
        }
    }
}

using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    protected float duration;

    public virtual void Perform(GameObject gameObject)
    {
        this.gameObject.GetComponent<Renderer>().enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;
    }
}
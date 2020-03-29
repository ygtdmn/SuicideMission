using System.Collections;
using UnityEngine;

public class FastShoot : Powerup
{
    private Player player;
    private new const float duration = 5f;

    public override void Perform(GameObject gameObject)
    {
        base.Perform(gameObject);
        player = gameObject.GetComponent<Player>();
        StartCoroutine(Set2xSpeed());
    }

    private IEnumerator Set2xSpeed()
    {
        var initialDelay = player.GetFiringDelay();
        player.SetFiringDelay(initialDelay / 2);
        yield return new WaitForSeconds(duration);
        player.SetFiringDelay(initialDelay);
        Destroy(gameObject);
    }
}
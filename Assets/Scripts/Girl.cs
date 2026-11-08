using UnityEngine;

public class Girl : Entity
{
    [Header("Extra Details")]
    [SerializeField] private Transform player;

    protected override void Update()
    {
        HandleFlip();
    }

    protected override void HandleFlip()
    {
        if (player.transform.position.x > transform.position.x && facingRight == false)
            flip();
        else if (player.transform.position.x < transform.position.x && facingRight == true)
            flip();
    }
}
     
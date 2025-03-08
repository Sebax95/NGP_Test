using UnityEngine;

public class PlayerController: IController
{
    private Player _player;
    public PlayerController(Player playerModel)
    {
        _player = playerModel;
    }
    

    public Vector3 Move()
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal") /2, 0, Input.GetAxisRaw("Vertical"));
        return direction.normalized;
    }
}
using UnityEngine;

public class PlayerController: IController
{
    private Player _player;
    public PlayerController(Player playerModel)
    {
        _player = playerModel;
    }


    public void CheckInputs()
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _player.MovePlayer(direction.normalized);

        if (Input.GetKeyDown(KeyCode.E))
            _player.PickUpItem();
    }
}
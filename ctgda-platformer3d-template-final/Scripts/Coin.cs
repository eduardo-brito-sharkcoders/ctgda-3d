using System;
using Godot;

public partial class Coin : Area3D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is PlayerController player)
        {
            GD.Print("Score: +1");
            player.AddScore(1);
            QueueFree();
        }
    }
}

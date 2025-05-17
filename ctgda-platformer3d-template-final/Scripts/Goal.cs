using System;
using Godot;

public partial class Goal : Area3D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is PlayerController)
        {
            GD.Print("Win!");
            //GetTree().ChangeSceneToFile("res://Scenes/UI/VictoryScreen.tscn");
        }
    }
}

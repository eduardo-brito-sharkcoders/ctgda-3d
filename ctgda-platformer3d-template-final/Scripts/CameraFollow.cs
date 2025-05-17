using System;
using Godot;

public partial class CameraFollow : Node3D
{
    [Export] public Node3D Target;
    [Export] public Vector3 Offset = new Vector3(0, 5, -10);
    [Export] public float LerpSpeed = 5f;

    public override void _Process(double delta)
    {
        if (Target == null) return;

        Vector3 targetPosition = Target.GlobalTransform.Origin + Offset;
        GlobalTransform = new Transform3D(GlobalTransform.Basis,
            GlobalTransform.Origin.Lerp(targetPosition, (float)delta * LerpSpeed));
    }
}

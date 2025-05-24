/*using Godot;

public partial class EnemySentinel : CharacterBody3D
{
    [Export] public float Speed = 2f;
    [Export] public float Gravity = -30f;

    [Export] public float LeftLimit = -1.15f;
    [Export] public float RightLimit = 1.15f;


    private Vector3 _velocity;
    private int _direction = 1; // 1 = right, -1 = left
    private float _startX;

    public override void _Ready()
    {
        _startX = GlobalTransform.Origin.X;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Gravity
        if (!IsOnFloor())
            _velocity.Y += Gravity * (float)delta;
        else
            _velocity.Y = 0;

        // X-axis movement
        _velocity.X = _direction * Speed;

        // Apply movement
        Velocity = _velocity;
        MoveAndSlide();

        // Get current X relative to start
        float relativeX = GlobalTransform.Origin.X - _startX;

        // Check patrol bounds
        if (relativeX > RightLimit)
        {
            _direction = -1;
        }
        else if (relativeX < LeftLimit)
        {
            _direction = 1;
        }
    }

    private void _on_area_3d_area_entered(Node body)
    {
        if (body is PlayerController player)
        {
            GD.Print("Damage!");
            player.TakeDamage(1);
        }
    }
}
*/
using Godot;

public partial class EnemySentinel : CharacterBody3D
{
    [Export] public float Speed = 2f;
    [Export] public float Gravity = -30f;
    [Export] public float LeftLimit = -1.15f;
    [Export] public float RightLimit = 1.15f;

    private Vector3 _velocity;
    private int _direction = 1;
    private float _startX;

    public override void _Ready()
    {
        _startX = GlobalTransform.Origin.X;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Apply gravity
        if (!IsOnFloor())
            _velocity.Y += Gravity * (float)delta;
        else
            _velocity.Y = 0;

        // X-axis movement
        _velocity.X = _direction * Speed;

        // Move and check collisions
        Velocity = _velocity;
        MoveAndSlide();

        // Check for collision with player
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            KinematicCollision3D collision = GetSlideCollision(i);
            if (collision.GetCollider() is PlayerController player)
            {
                player.TakeDamage(1);
            }
        }

        // Patrol limit logic
        float relativeX = GlobalTransform.Origin.X - _startX;
        if (relativeX > RightLimit)
        {
            _direction = -1;
        }
        else if (relativeX < LeftLimit)
        {
            _direction = 1;
        }
    }
}

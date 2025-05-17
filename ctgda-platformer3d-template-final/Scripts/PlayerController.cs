using System;
using Godot;

public partial class PlayerController : CharacterBody3D
{
    [Export] public float Speed = 6f;
    [Export] public float JumpForce = 12f;
    [Export] public float Gravity = -30f;

    [Export] public int MaxHealth = 5;
    private int _currentHealth = 0;
    private int _score = 0;

    private Vector3 _velocity;

    public override void _Ready()
    {
        _currentHealth = MaxHealth;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 input = Input.GetVector("move_left", "move_right", "move_forward", "move_back");

        Vector3 direction = new Vector3(input.X, 0, input.Y).Normalized();
        Vector3 movement = GlobalTransform.Basis * direction;
        movement.Y = 0;
        movement = movement.Normalized();

        _velocity.X = movement.X * Speed;
        _velocity.Z = movement.Z * Speed;

        if (!IsOnFloor())
        {
            _velocity.Y += Gravity * (float)delta;
        }
        else if (Input.IsActionJustPressed("jump"))
        {
            _velocity.Y = JumpForce;
        }

        Velocity = _velocity;
        MoveAndSlide();
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        GD.Print("Player took damage. Health: " + _currentHealth);

        if (_currentHealth <= 0)
        {
            GD.Print("Player died. Restarting level...");
            GetTree().ReloadCurrentScene();
        }
    }

    public void AddScore(int amount)
    {
        _score += amount;
        GD.Print("Score: " + _score);
    }

    // Optional accessors
    public int GetScore() => _score;
    public int GetHealth() => _currentHealth;
}

/*
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
*/

using Godot;

public partial class PlayerController : CharacterBody3D
{
    [Export] public float Speed = 6f;
    [Export] public float JumpForce = 12f;
    [Export] public float Gravity = -30f;

    [Export] public int MaxHealth = 5;
    [Export] public float InvincibilityDuration = 1.5f;
    [Export] public float FlashInterval = 0.1f;

    private int _currentHealth;
    private int _score;
    private bool _isInvincible = false;
    private float _invincibilityTimer = 0f;
    private float _flashTimer = 0f;

    private MeshInstance3D _mesh;
    private Vector3 _velocity;

    public override void _Ready()
    {
        _currentHealth = MaxHealth;
        _mesh = GetNode<MeshInstance3D>("MeshInstance3D"); // adjust path if needed
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
            _velocity.Y += Gravity * (float)delta;
        else if (Input.IsActionJustPressed("jump"))
            _velocity.Y = JumpForce;

        Velocity = _velocity;
        MoveAndSlide();

        HandleInvincibility(delta);
    }

    public void TakeDamage(int amount)
    {
        if (_isInvincible)
            return;

        _currentHealth -= amount;
        GD.Print($"Player took damage. Health: {_currentHealth}");

        if (_currentHealth <= 0)
        {
            GD.Print("Player died.");
            GetTree().ReloadCurrentScene();
            return;
        }

        StartInvincibility();
    }

    public void AddScore(int amount)
    {
        _score += amount;
        GD.Print("Score: " + _score);
    }

    private void StartInvincibility()
    {
        _isInvincible = true;
        _invincibilityTimer = InvincibilityDuration;
        _flashTimer = FlashInterval;
    }

    private void HandleInvincibility(double delta)
    {
        if (!_isInvincible)
            return;

        _invincibilityTimer -= (float)delta;
        _flashTimer -= (float)delta;

        // Toggle visibility
        if (_flashTimer <= 0f)
        {
            _mesh.Visible = !_mesh.Visible;
            _flashTimer = FlashInterval;
        }

        if (_invincibilityTimer <= 0f)
        {
            _isInvincible = false;
            _mesh.Visible = true; // Make sure it's visible at the end
        }
    }

    // Optional accessors
    public int GetScore() => _score;
    public int GetHealth() => _currentHealth;
}


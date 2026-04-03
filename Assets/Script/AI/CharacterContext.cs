using System;

[Serializable]
public class CharacterContext
{
    public float Speed;
    public float JumpForwardSpeed = 1.2f;
    public float JumpForce = 5f;
    public float AttackRange;
    public float SearchRange;
    public float BaseTargetLockRange;
    public string TargetTag;
}
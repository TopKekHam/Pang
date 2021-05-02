
using System;

public enum Direction : int
{
    Left = -1, Right = 1
}

[Flags]
public enum PlayerMoveDirection : int
{
    None = 0, Left = 1, Right = 2
}

public enum LevelState : int
{
    Paused = 0, Playing = 1
}

public enum PlayerNumber : int
{
    One = 1, Two = 2
}
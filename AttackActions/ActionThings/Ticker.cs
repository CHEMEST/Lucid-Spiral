using Godot;
using System;

public class Ticker
{
    public float Duration { get; private set; }
    public float TickSpeed { get; private set; }
    private float elapsedTime;

    public Ticker(float duration, float tickSpeed)
    {
        Duration = duration;
        TickSpeed = tickSpeed;
        elapsedTime = 0;
    }

    public bool Update(float delta)
    {
        elapsedTime += TickSpeed * delta;
        return elapsedTime >= Duration;
    }
}
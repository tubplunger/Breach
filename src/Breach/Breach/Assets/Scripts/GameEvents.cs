using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public static event Action<Health> OnHealthDied;
    public static event Action<Health, int> OnHealthDamaged;
    public static event Action<TimeState> OnTimeStateRequested;

    public static void HealthDied(Health health)
    {
        OnHealthDied?.Invoke(health);
    }

    public static void HealthDamaged(Health health, int damage)
    {
        OnHealthDamaged?.Invoke(health, damage);
    }

    public static void TimeStateRequested(TimeState state)
    {
        OnTimeStateRequested?.Invoke(state);
    }
}

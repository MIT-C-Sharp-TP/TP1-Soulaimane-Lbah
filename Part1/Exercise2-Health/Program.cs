using System;

int health = 45;

if (health > 70)
{
    Console.WriteLine($"Aya has {health} HP: healthy");
}
else if (health > 30)
{
    Console.WriteLine($"Aya has {health} HP: injured");
}
else if (health > 0)
{
    Console.WriteLine($"Aya has {health} HP: critical");
}
else
{
    Console.WriteLine("Aya is down");
}

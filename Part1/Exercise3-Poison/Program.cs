using System;

int health = 40;
int poison = 8;

for (int turn = 1; turn <= 6; turn++)
{
    health = Math.Max(health - poison, 0);
    Console.WriteLine($"Turn {turn}: poison, {health} HP left");

    if (health == 0)
    {
        Console.WriteLine("Aya is down.");
        break;
    }
}

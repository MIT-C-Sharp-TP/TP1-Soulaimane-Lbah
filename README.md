# TP1: Arena, a Turn-Based Duel in the Console

![C#](https://img.shields.io/badge/C%23-.NET%2010-512BD4?logo=dotnet&logoColor=white)
![Type](https://img.shields.io/badge/type-console%20app-lightgrey)
![Status](https://img.shields.io/badge/status-complete-brightgreen)

Lab work for **C# for Unity**, Master's in Immersive Technologies, Digital Engineering for Health Professions, ENSAM Casablanca (2026-2027).

Everything runs in the console: no Unity, no 3D scene. The aim is to practise the C# fundamentals that carry over directly to Unity (classes, properties, inheritance, events, collections).

---

## Table of contents

- [Repository structure](#repository-structure)
- [Requirements](#requirements)
- [Getting started](#getting-started)
- [Part 1: Warm-up exercises](#part-1-warm-up-exercises)
- [Part 2: The Arena](#part-2-the-arena)
- [Class design](#class-design)
- [Results file](#results-file)
- [Example output](#example-output)
- [Concepts covered](#concepts-covered)
- [Author](#author)

---

## Repository structure

```
.
├── README.md
├── .gitignore
├── Part1/
│   ├── Exercise1-Sheet/     Variables and interpolated strings
│   ├── Exercise2-Health/    if / else if / else
│   ├── Exercise3-Poison/    for loop, break
│   └── Exercise4-Heal/      Methods and return values
└── Part2/
    └── Arena/               The complete duel game
```

Each folder is an independent project with its own `.csproj`. Run a project from inside its own folder.

---

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- A code editor: [Visual Studio Code](https://code.visualstudio.com/) with the C# Dev Kit extension, or Visual Studio
- A terminal (PowerShell, Windows Terminal or the VS Code terminal)

Check your installation:

```bash
dotnet --version
```

The version should start with `10`.

---

## Getting started

```bash
# 1. Clone the repository
git clone https://github.com/MIT-C-Sharp-TP/TP1-Soulaimane-Lbah.git
cd TP1-Soulaimane-Lbah

# 2. Go into the project you want to run
cd Part2/Arena

# 3. Run it
dotnet run
```

> **Note:** `dotnet run` only looks in the current folder. If you get `Couldn't find a project to run`, you are in the wrong folder.
>
> Run the game with `dotnet run` in a terminal, not with F5 in VS Code: the Debug Console does not accept keyboard input.

---

## Part 1: Warm-up exercises

Four small programs, each in a single `Program.cs`.

| Exercise | What it does | Main ideas |
|---|---|---|
| [1. Character sheet](Part1/Exercise1-Sheet) | Prints Aya's name, level, health, speed and key, plus the XP for the next level | `string`, `int`, `float`, `bool`, `$"..."` |
| [2. Health status](Part1/Exercise2-Health) | Prints healthy, injured, critical or down depending on health | `if` / `else if` / `else` |
| [3. Poison](Part1/Exercise3-Poison) | Aya loses 8 HP per turn, never below 0, and the loop stops when she is down | `for`, `Math.Max`, `break` |
| [4. Heal](Part1/Exercise4-Heal) | A `Heal` method that never goes above the maximum health | Methods, `return`, `Math.Min` |

---

## Part 2: The Arena

The player, **Aya**, fights four enemies one at a time, turn by turn. Each turn she attacks or drinks a potion, then the enemy strikes back.

### Features

- A wave of four enemies: **Goblin, Orc, Goblin, Boss**
- **Inheritance**: an abstract `Enemy` class and three child classes, each with its own attack
- A **Boss with armour** that reduces every hit it takes (`virtual` / `override`)
- A **`Died` event**: `Player` announces its own death and the game listens
- **Safe input**: invalid text, numbers and empty lines never crash the game
- **Several games in a row**, with a summary of the three fastest victories (LINQ)
- A **results file** (`results.txt`) written after each game, with `try` / `catch`

### How to play

| Key | Action |
|---|---|
| `1` | Attack: the enemy takes 12 to 24 damage |
| `2` | Drink a potion: Aya recovers 30 HP (uses her turn) |

Any other input prints `Type 1 or 2.` and gives the same turn again. After each game, answer `y` to play again.

### Game rules

| Character | Health | Attack damage | Special |
|---|---|---|---|
| **Aya** (player) | 100 HP | 12 to 24 | 2 potions, +30 HP each, never above 100 HP |
| **Goblin** | 30 HP | 4 to 8 | |
| **Orc** | 60 HP | 8 to 14 | |
| **Boss** | 60 HP | 9 to 16 | Armour 6: removes 6 from every hit (at least 1 gets through) |

- The enemies come in this order: Goblin, Orc, Goblin, Boss.
- A potion uses Aya's turn, so the enemy still strikes back.
- A defeated enemy does not strike back.
- Health never goes below 0 and never above the maximum.
- You win by defeating all four enemies, and lose when Aya reaches 0 HP.

---

## Class design

```
Player                           Enemy (abstract)
- health : int                   + Name : string
+ Name, MaxHealth, Potions       + Health : int
+ Health (0 to MaxHealth)        + IsAlive : bool
+ IsAlive : bool                 + TakeDamage(amount)   {virtual}
+ event Died                     + Attack(target, rng)  {abstract}
+ TakeDamage(amount)                     ^
+ DrinkPotion()                          |
                                +--------+--------+
                              Goblin    Orc     Boss
                                                (overrides TakeDamage)
```

**Why a private field?** `health` can only be changed through the `Health` property, whose `set` uses `Math.Clamp(value, 0, MaxHealth)`. Whatever the damage or the healing, the health stays in the valid range.

**Why an event?** `Player` raises `Died` at the exact moment its health reaches 0, without knowing who listens. The game subscribes with `player.Died += ...`, the same pattern Unity scripts use to talk to each other.

---

## Results file

After each game, one line is appended to `results.txt` in the project folder:

```text
Game 1: defeat
Game 2: victory in 15 turns
Game 3: victory in 13 turns
Game 4: defeat
```

If writing fails (read-only file, locked file, protected folder), the `catch` blocks print `Could not save the result: ...` and the game goes on.

---

## Example output

```text
Turn 14: Aya 16 HP, 1 potions | Boss 15 HP
1 attack, 2 drink a potion > 1
The armour absorbs 6 damage.
Boss takes 6 damage (9 HP)
Boss slams the ground!
Aya takes 16 damage (0 HP)
Game over for Aya.

Defeat...
Saved: Game 1: defeat
Play again? (y/n) > n
Victories: 0 of 1
```

Random numbers differ at every game. To replay exactly the same game while debugging, create the generator with a fixed seed: `new Random(7)`.

---

## Concepts covered

| Area | Concepts |
|---|---|
| Basics | Variables, `if`, `for`, `while`, `switch`, methods that return a value |
| Object-oriented | Classes, private fields, properties, inheritance, abstract classes, `virtual` / `override`, `base` |
| Collections | `List<T>`, `foreach`, `RemoveAt` |
| Robustness | `int.TryParse`, `Math.Clamp`, `try` / `catch` with two exception types |
| Advanced | Events and lambdas, LINQ (`OrderBy`, `Take`), file input/output |

In Unity, `Console.WriteLine` becomes `Debug.Log`; the rest of the structure stays the same.

---

## Author

**Soulaimane Lbah**
Master's in Immersive Technologies, Université Hassan II de Casablanca

Course by Abdelhakim EL Ghayoubi.

SoulStoneSurvivorLike 🎮✨
A Unity project demonstrating scalable game systems using design patterns, inspired by Vampire Survivors and Soulstone Survivors.

Unity Version
Demo Video

Demo GIF

<!-- Embed a short gameplay clip showing patterns in action (e.g., waves, ability systems, UI) -->
🔑 Key Design Patterns Demonstrated
Map your PDF topics to in-game systems for clarity:

Pattern	Implementation Example	Game Context
Singleton	AudioManager, AssetManager	Global access to sound/resources.
State Pattern	Player states (Idle, Attack, Dash)	Smooth transitions between abilities.
Event Bus	EnemyDeathEvent, LevelUpEvent	Decoupled UI/achievement systems.
Command Pattern	Input handling, undoable actions (dev cheats)	Rebindable controls or replay systems.
Object Pool	Bullet/projectile spawning	Optimized performance during combat.
Strategy Pattern	Enemy AI behavior variations	Adjustable difficulty/scaling.
Decorator	Upgradable abilities (e.g., +burn dmg, +AoE)	Modular power-up system.
Visitor Pattern	Damage calculation (e.g., modifiers)	Buffs/debuffs affecting multiple entities.
WFC (Wave Collapse)	Procedural level/map generation	Randomized dungeon layouts.
🛠️ Technical Breakdown
Architecture
Modular Systems: Each pattern is isolated for easy reuse (e.g., EventBus can be dropped into new projects).

ScriptableObject-Driven: Data-driven design for patterns like Strategy (enemy stats) and Decorator (ability upgrades).

Performance: Uses Object Pooling for bullets, ECS-lite for enemy groups.

Code Snippets
csharp
Copy
// Example: Event Bus for handling enemy deaths  
public class EnemyDeathEvent : IEvent {  
    public int EnemyID;  
    public Vector3 DeathPosition;  
}  

// UI system subscribing to the event  
EventBus.Subscribe<EnemyDeathEvent>(OnEnemyDeath);  
🎮 How to Play
Clone the repo.

Open Assets/Scenes/Main.unity in Unity 2022.3+.

Controls:

WASD: Movement

Space: Dash (State Pattern)

Mouse: Aim/Shoot (Command Pattern)

📸 Media
System	Screenshot/Diagram
State Pattern	Player State Machine
Event Bus Flow	Event Sequence
WFC Level Gen	Procedural Map
📚 Design Pattern Deep Dives
Why Use the Command Pattern for Input?
Decouples input logic from gameplay systems.

Enables save/replay systems (stores command history).

Inspired by games like Celeste (rebindable controls).

Object Pool vs. Instantiation
Benchmarked 10k bullets: 200ms → 2ms per wave.

Critical for bullet-hell gameplay styles.

🚀 How to Extend
Add New Patterns:

Try Observer Pattern for achievements.

Implement Flyweight for shared enemy stats.

Modify Systems:

Create new Decorators for crazier abilities (e.g., homing missiles).

Add Visitor-based modifiers (e.g., "double damage vs. flying").

📝 Lessons Learned
Singletons: Great for managers but risky for tight coupling. Used Zenject for DI in later iterations.

Event Bus Overhead: Too many events can clutter debuggers. Added event tagging for filtering.

🔗 Related Resources
Game Programming Patterns (Nystrom)

Unity Architecture Tips

This structure showcases:

Pattern mastery: You understand when and why to use each pattern.

Practical application: Patterns are tied to real game systems.

Performance focus: You care about optimization (critical in game dev).

# SoulStoneSurvivorLike 🎮✨  
*A Unity project demonstrating scalable game systems using design patterns, inspired by Vampire Survivors and Soulstone Survivors.*  

[![Unity Version](https://img.shields.io/badge/Unity-2022.3+-black?logo=unity)](https://unity.com)  

---

## 🔑 **Key Design Patterns Demonstrated**  
| Pattern              | Implementation Example                          | Game Context                              |  
|----------------------|------------------------------------------------|-------------------------------------------|  
| **Singleton**        | `AudioManager`, `AssetManager`                 | Global access to sound/resources.         |  
| **State Pattern**    | Player states (`Idle`, `Attack`, `Dash`)       | Smooth transitions between abilities.     |  
| **Event Bus**        | `EnemyDeathEvent`, `LevelUpEvent`              | Decoupled UI/achievement systems.         |  
| **Command Pattern**  | Input handling, undoable actions (dev cheats)  | Rebindable controls or replay systems.    |  
| **Object Pool**      | Bullet/projectile spawning                     | Optimized performance during combat.      |  
| **Strategy Pattern** | Enemy AI behavior variations                   | Adjustable difficulty/scaling.            |  
| **Decorator**        | Upgradable abilities (e.g., +burn dmg, +AoE)   | Modular power-up system.                  |  
| **Visitor Pattern**  | Damage calculation (e.g., modifiers)           | Buffs/debuffs affecting multiple entities.|  
| **WFC (Wave Collapse)** | Procedural level/map generation             | Randomized dungeon layouts.               |  

---

## 🛠️ **Technical Breakdown**  
### **Architecture**  
- **Modular Systems**: Each pattern is isolated for easy reuse (e.g., `EventBus` can be dropped into new projects).  
- **ScriptableObject-Driven**: Data-driven design for patterns like `Strategy` (enemy stats) and `Decorator` (ability upgrades).  
- **Performance**: Uses `Object Pooling` for bullets, `ECS-lite` for enemy groups.  

### **Code Snippets**  
```csharp
// Example: Event Bus  
public class EventBus : Singleton<EventBus>
{
    private readonly Dictionary<Type, Action<object>> eventListeners = new Dictionary<Type, Action<object>>();
    private readonly object lockObject = new object(); // For thread-safety

    /// <summary>
    /// Subscribes to an event with a specific listener.
    /// Thread-safe.
    /// </summary>
    /// <typeparam name="T">The type of event to subscribe to.</typeparam>
    /// <param name="listener">The listener to invoke when the event is published.</param>
    public void Subscribe<T>(Action<object> listener)
    {
        var eventType = typeof(T);
        lock (lockObject)
        {
            if (!eventListeners.ContainsKey(eventType))
            {
                eventListeners[eventType] = null;
            }
            eventListeners[eventType] += listener;
        }
    }

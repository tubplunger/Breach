# Breach

1. System Overview:
    1. Player Movement
    2. Combat
    3. Health/Win-Lose
    4. Time System
    5. Enemy AI

Dependencies:
    Player Movement depends on the Input, Character Controller, Camera, and Time System.
    Combat depends on input for player firing, enemy firing, Projectile prefab, Health System and the time system.
    Health and Win-Lose depends on Health values, projectile damage, the Game Manager, and Tags
    The Time System depends on the Player's state and movement, and Unity Time.timeScale
    Enemy AI depends on Player transformation, patrol points, the projectile prefab, fire point, and time system

2. Time System:
    For the Time System, the single source of it is Unity's Time.timeScale system which is controlled by the PlayerTimeFPSController on the Player character.
    Most of the systems in the game use Time.deltaTime within the game to respect when time is slowed or not. This is put in for enemy movement, fire cooldowns, projectile movement, and visual hit and muzzle flashes. Player movement partially uses it and the camera look uses unscaled time instead, so that the Player remains responsive. I don't think anyone would have fun if both of these were fully under the time system.

3. Cube Test:
    If all of the visuals were removed the game would still run and function as normal. Movement still works for both the Player and Enemies, and combat still works as well. Everything still works as long as the logic side of things isn't changed drastically.
    The only thing that breaks is some of the visuals no longer being there, like for the muzzle flash and enemy hit flash

4. System Isolation Test:
    The movement system by itself works as long as the Player has a Character controller on it and the camera.
    The combat system along partially works. Projectiles can move and damage objects with the Health script on their own, but to fire they need either a Player or Enemy source.
    The AI system alone partially works as well. It can patrol without issue, though it does require a reference to the Player, projectile prefab, and a fire point to work 100%
    The time system works partially alone, though it needs the inputs and states from the Player moving.
    Most of the systems within Breach do reference each other to work, so there are some breaks when the scripts are isolated.

5. Input vs Simulation:
    Input in the game does a mix of things for it to work. Movement input directs the Player to move through the Player Controller. The shooting input makes the Player fire and create a Projectile prefab which is handled by the combat simulation. Time is effected by player movement.

6. Combat System:
    Combat can funtion without any visuals, though it only partially works without the Player. The Projectile itself and Health system can function just fine without the Player and any visuals. However, Player shooting needs the Player controller to exist for it to work.

7. AI System:
    The AI itself depends on a mix of system. The AI does use Unity objects in the scene for things like tracking Player Transform, its designated patrol points, where it fires from, and the Projectile prefab itself. However, it also uses simulation to figure out things like how far it is from the Player, fire cooldown, detection range, and the current patrol target.

8. Data vs Code:
    There are a lot of things that are hardcoded in the game that can be data driven.
    Enemy stats:
        Health, patrol speed, detection range, attack range, fire cooldown

    Weapon stats:
        Projectile speed, projectile damage, fire cooldown

    Time values:
        Moving time scale, idle time scale, time change speed

    AI behavior rules:
        Patrol, detect player by distance, attack when in range
    However, since many of these values are public they can be changed in the Inspector as needed, though their behavior is still hardcoded within the scripts.

9. Change Cost Test:
    For this, lets say that we want to change the amount of damage the Projectile deals. For the files that needs to be change it is technically 0 since you can change the value in the inspector, though it becomes 1 file if you want to change the default value itself.
    The only systems that are really effected by this change are Combat and Health, which takes the damage the Projectile does and applies it to both. Still, these changes are mostly local and nothing broke when making this change.
    Changing the damage of a projectile doesn't cause any issues. Though if more weapons or types of Enemies were added in, the stats of things like weapons would change to be data-driven with different weapon and projectile data.

10. Scaling Test:
    If the Enemies were suddenly scaled by 10x it would become taxing on the game, mostly because of all of the Enemies needing to check distance and firing in their code as they go.
    For 10x the projectiles, collision and movement updates would become a bit of a bottleneck though one that wouldn't be too hard to fix.
    For multiple weapons, the hardcoded shooting logic would become harder to manage and would need to become data-driven like I mentioned before.
    For multiple abilities, time control and player state would need to be organized better in the grand scheme of things, though at this moment it is in a good spot.
    The first bottlenecks would come from AI and Projectiles, entirely because they update each frame independently which would tax the system quite a bit if they were both suddenly scaled up.

11. Assumptions:
    Only one player exists.
    Only one main enemy is needed at the moment.
    The player controls the time system.
    All projectiles use the same behavior.
    All enemies use the same AI behavior.
    Player and enemies use tags.
    Visuals are completely separate from gameplay logic.
    Only one scene.
    The player has a player controller.
    Slowdown should affect enemies, projectiles, and cooldowns.
    Player camera remains responsive during slowdowns.

12. Debuggability:
    If something were to break it can be tracked down quickly. Mostly became things like the time system has a visual Debug UI and print logs to the console, which makes the problem usually clear if there is one. I give it about a 7/10.
    Some systems do depend on manually assigned references though, so missing these references can cause issues that aren't immediately apparent without warning logs. Though right now everything is working as it should.

13. Refactor Test:
    If I had 2 hours to improve the architecture, it would be to make the scripts not as hardcode dependent. The values can be changed in the inspector without issue, but if I were to start expanding the scope of this I would run into issues because of the hardcoding.
    What I would change first would be Projectiles. I envision multiple weapons in the game so they would need to have different values and projectiles, or even get rid of the projectile system altogether and go to hitscans instead.

Summary:
    In the end, I feel like systems like the time control and player inputs are rather strong and are in a good spot, though they still need a few tweaks. The most fragile things right now would definitely be enemies and projectiles, which would both need improvement before more features are added in I feel like.
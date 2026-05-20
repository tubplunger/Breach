# Breach

A 3D game made in Unity created to be an FPS. It takes heavy inspiration from Superhot with the main mechanic being that when you stop time slows down and when you move time speeds up. There is also inspiration from SCP with the player mainly facing supernatural entities.

The game also takes inspiration from the original DOOM with its movement and shooting mechanics. Which, speaking of the shooting mechanics, has been improved from its original state. It is now data driven in that multiple weapons can exist with different values for each that are no longer hard coded onto one script, making each weapon feel a bit more unique than the last.

This weapon script is also reusable. In that it can be taken from Breach and put into a different game if shooting mechanics are needed for it, doing so with little or no changes needed in the process though you will likely need to build your own projectile and health script for it.

Continuing with reusability and upgrading the systems in place, every other system in place has been changed in a similar fashion. The next to be changed was the AI system, which now has a chase state added in so the enemies will go after the Player rather than standing there. Enemy variants have also been added though only one new addition is currently present.

The Time System was changed around so that there is a central time manager rather than it being controlled by the Player, though Player movement still dictates the time state. This system has been made reusable along with the AI systems.

A simple event system has also been added which published and subscribes the information that it is given. Right now, it uses only Health and Death and dictates both though it can be easily expanded and reused for other purposes in different projects.

Object Pooling has been added as well, mostly for Projectiles so they don't become a problem later. This Object Pooling can work for different objects and each one can be adjusted easily to different projects and objects as needed.

These are all the improvements made so far and components that have been made to be reusable.
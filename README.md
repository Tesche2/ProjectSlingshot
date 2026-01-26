# Project Slingshot

> A high-precision, physics-based speedrunning prototype built in Unity.

![License](https://img.shields.io/badge/license-MIT-green)
![Unity](https://img.shields.io/badge/Unity-6-black)
![Status](https://img.shields.io/badge/status-Prototype-orange)

**Project Slingshot** is a mobile-first runner where players manipulate gravitational forces to navigate obstacle-courses. The core mechanic involves toggling the rocket's internal gravity receiver to execute precise **gravitational slingshots** around planetary bodies.

![Gameplay Demo](Docs/Gameplay.gif)

## Core Mechanics
* **Vector-Based Propulsion:** Thruster logic respects conservation of momentum and applies forces relative to current velocity (boosting orthogonal vectors for tighter control).
* **Gravity Toggling:** Players can dynamically toggle gravitational influence mid-flight to gain momentum or escape orbits.
* **Time-Trial Focus:** Designed for milliseconds-perfect runs.

## Key Features & Tech

* **Newtonian Gravity Mechanics:**
  * Custom gravity implementation allowing dynamic toggling of mass influence.
  * Optimized detection logic using spatial triggers instead of global searches, ensuring high performance on mobile.

* **Deterministic Physics:**
  * Input handling is synchronized with the physics engine rather than the render loop.
  * Ensures identical behavior across different frame rates and enables future replay/speedrun verification.

* **Mobile-First Architecture:**
  * Decoupled visual feedback from physics logic.
  * Touch input buffer prevents "dropped inputs" during frame spikes.

* **Scalable Codebase:**
  * Domain Driven folder structure for easy maintenance.

* **Automatic Controller Device Detection**
  *  Input controller type is automatically detected, changing the game GUI based on it.

* **Subframe Time Calculation**
  * Time to reach the finish line is calculated down to a millisecond precision, even when the physics calculations are performed at a fixed 50fps.

## Installation & Setup
1.  **Clone** the repository.
2.  Open in **Unity 6** (Recommended: 6000.3.1f1 or later).
3.  Open the scene: `_Game/Scenes/Core.unity`.
4.  Press Play.
    * **Controls:**
    * *Keyboard:* **WASD** (Thrust), **Space** (Hold for Gravity).
    * *Gamepad:* **Left Stick** (Thrust), **South Button** (Hold for Gravity).
    * *Mobile:* **On-Screen Overlay**, Simulating a Gamepad. 

## Roadmap
- [x] Newtonian Gravity Implementation
- [x] Deterministic Player Controller
- [x] Dynamic Velocity-Based Camera Zoom
- [x] Refactor data into **ScriptableObjects** for designer iteration
- [x] Mobile Touch UI Overlay (Virtual Joysticks/Buttons)
- [ ] Game Save System
- [ ] Ghost Replay System (recording inputs per physics tick)
- [ ] General Visual Improvements (UI and Gameplay)
- [ ] Sound Effects and Soundtrack

---
*Developed by [Nikolas Tesche](https://www.linkedin.com/in/nikolas-tesche-3112b9167)*
<br>
*Placeholder art by [DanProps](https://assetstore.unity.com/packages/2d/gui/space-game-gui-kit-psd-315973)*
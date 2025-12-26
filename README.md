# Project Slingshot

> A high-precision, physics-based speedrunning prototype built in Unity.

![License](https://img.shields.io/badge/license-MIT-green)
![Unity](https://img.shields.io/badge/Unity-6-black)
![Status](https://img.shields.io/badge/status-Prototype-orange)

**Project Slingshot** is a mobile-first "runner" where players manipulate gravitational forces to navigate obstacle-courses. The core mechanic involves toggling the rocket's internal gravity receiver to execute precise **gravitational slingshots** around planetary bodies.

![Gameplay Demo](docs/Gameplay.gif)

## 🎮 Core Mechanics
* **Vector-Based Propulsion:** Thruster logic respects conservation of momentum and applies forces relative to current velocity (boosting orthogonal vectors for tighter control).
* **Gravity Toggling:** Players can dynamically toggle gravitational influence mid-flight to gain momentum or escape orbits.
* **Time-Trial Focus:** Designed for milliseconds-perfect runs.

## 🚀 Key Features & Tech

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

* **Allocation-Free Physics Loop:**
  * Gravity calculations rely on cached lists and Trigger events rather than runtime queries (`FindObjectsOfType`), preventing Garbage Collection spikes during gameplay.

## 🛠️ Installation & Setup
1.  **Clone** the repository.
2.  Open in **Unity 6** (Recommended: 6000.3.1f1 or later).
3.  Open the scene: `_Game/Scenes/SampleScene.unity`.
4.  Press Play.
    * *Controls:* **WASD** (Thrust), **Space / LMB** (Hold for Gravity).

## 🔜 Roadmap
- [x] Newtonian Gravity Implementation
- [x] Deterministic Player Controller
- [x] Dynamic Velocity-Based Camera Zoom
- [ ] Refactor Planet Data into **ScriptableObjects** for designer iteration
- [ ] Mobile Touch UI Overlay (Virtual Joysticks/Buttons)
- [ ] Ghost Replay System (recording inputs per physics tick)
- [ ] General Visual Improvements

---
*Developed by [Nikolas Tesche](https://www.linkedin.com/in/nikolas-tesche-3112b9167)*
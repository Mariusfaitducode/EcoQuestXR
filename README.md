# EcoQuest XR

A mixed-reality city-management game for Meta Quest: you steer a procedurally generated city toward a sustainable future by playing action cards, and watch the effects play out on a miniature city in your room.

[Watch the presentation video](https://youtu.be/2JAxSYWuL90)

## Gameplay

- **A new city every game.** The terrain is generated from Perlin noise with a falloff map, split into chunks, then filled with four zones (city, industry, agriculture, energy), a river, a road network and nature.
- **Card-based decisions.** At regular intervals you draft new cards into a deck attached to your left arm. Each card builds, destroys, replaces, upgrades or downgrades buildings in one zone (eco-friendly housing, parks, wind and solar farms, nuclear, coal and biogas plants, recycling centres, industrial or eco-friendly farms, and more). Cards are defined in CSV files.
- **A simulated city.** Every action costs money and energy and changes CO2 emissions, waste and green spaces. A citizen model tracks happiness, health, awareness and acceptance, and the population chooses between transport modes.
- **A living map.** Cars and buses drive on the generated roads, pedestrians walk in the zones, and each zone has its own ambient sound.
- **Finn the cloud.** A guide character floats above the city, reacts to your choices with voiced lines, warns you when money, energy or public support run low, and gets cleaner as the ecology score improves.
- **Two control modes.** Meta Quest controllers in mixed reality, or keyboard and mouse for testing in the editor.

## Tech stack

Unity 2022.3, C#, Meta XR SDK (Oculus Integration, `com.unity.xr.oculus`), Unity AI Navigation, TextMesh Pro.

## Run it locally

1. Install Unity **2022.3.7f1** (see `ProjectSettings/ProjectVersion.txt`).
2. Clone the repo and open the folder with Unity Hub.
3. Open `Assets/Scenes/MenuPrincipal.unity` and press Play. The build order is `MenuPrincipal`, `Tutorial`, `Main`.
4. To test without a headset, set **Control Mode** to `keyboard` on the `GameManager` object in the main scene. To play on a Quest, keep `ovr` and build for Android.

## Context

Team project for the MV50 course at UTBM, spring 2024, by Victor Goudal, Marius Diguat--Mateus, Souleymane Zoungrana and Gilles Maurer. Marius worked mainly on the procedural map (terrain, zones, roads, chunks), the vehicle and pedestrian agents, the building and model system, and Finn's event lines.

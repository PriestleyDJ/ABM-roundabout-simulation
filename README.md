# Agent-Based Traffic Simulation of University Roundabout in Sheffield

This project is an agent-based simulation of the University of Sheffield roundabout, built using Unity. It provides a flexible and extensible framework for simulating traffic flow and vehicle behavior in a real-world location.

## Features

*   **Agent-Based Simulation:** Each vehicle is an autonomous agent with its own behavior and decision-making process.
*   **Realistic Traffic Network:** The simulation uses a detailed traffic network with lanes, waypoints, and junctions to guide vehicle movement.
*   **Collision Avoidance:** Vehicles use raycasting to detect and avoid collisions with other vehicles and obstacles.
*   **Configurable Vehicle Spawning:** The rate of vehicle spawning can be configured for different entry points to the roundabout.
*   **Mapbox Integration:** The simulation uses the Mapbox Unity SDK to display a map of the area.
*   **Extensible:** The project is designed to be easily extensible with new vehicle behaviors, traffic rules, and scenarios.

## Getting Started

### Prerequisites

*   Unity 2021.3.19f1 or later

### Installation

1.  Clone the repository to your local machine:
    ```
    git clone https://github.com/your-username/ABM-roundabout-simulation.git
    ```
2.  Open the project in the Unity Hub.
3.  Unity will automatically import the project and its dependencies.

## Usage

1.  Open the project in the Unity Editor.
2.  Open the main scene located at `Assets/Scenes/university_roundabout.unity`.
3.  Press the "Play" button in the Unity Editor to start the simulation.

You can adjust the vehicle spawn rates in the `SpawnVehicle` component of the `Spawner` GameObject in the scene.

## Project Structure

*   **Assets/Scenes:** Contains the main Unity scene for the simulation.
*   **Assets/Scripts:** Contains the core C# scripts for the simulation.
    *   `VehicleAgent.cs`: Defines the behavior of individual vehicle agents.
    *   `TrafficNetwork.cs`: Manages the traffic network, including lanes and junctions.
    *   `SpawnVehicle.cs`: Handles the spawning of vehicles into the simulation.
*   **Assets/Prefabs:** Contains the vehicle prefabs used in the simulation.
*   **Assets/Mapbox:** Contains the Mapbox Unity SDK.
*   **Assets/BrokenVector/LowPolyCarPack:** Contains the low-poly car models used in the simulation.

## Dependencies

*   **Mapbox Unity SDK:** Used for map rendering and location data.
*   **Low Poly Car Pack by Broken Vector:** Used for the vehicle models.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

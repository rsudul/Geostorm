# Geostorm

## Unity version

- Unity **6000.3.15f1** (LTS)

## Packages

- HDRP `17.3.0`
- New Input System `1.19.0`
- Cinemachine `3.1.6`
- AI Navigation `2.0.12`
- VContainer from Git dependency

## Getting started

1. Clone the repository:

   ```bash
   git clone https://github.com/rsudul/Geostorm.git
   ```
2. Install and initialize Git LFS:

   ```bash
   git lfs install
   git lfs pull
   ```
3. Open the project in Unity Hub using Unity 6000.3.15f1.
4. When the project opens for the first time, Unity should restore packages and regenerate project files.

## Project structure

   ```bash
   Assets/
     Data/             Project data and configuration assets
     Prefabs/          Reusable Unity prefabs
     Resources/        Runtime-loaded resources
     Scenes/           Unity scenes
     Scripts/
       Core/           Core gameplay/domain logic
       Infrastructure/ Unity-specific and external integration layer
     Settings/         Unity/HDRP/project-related settings
   
   Packages/           Unity package manifest and lock file
   ProjectSettings/    Unity project settings
   ```

## Workflow (IMPORTANT)

- Do not commit directly to the `main` branch.
- Create feature branches, for example:
  ```bash
  git checkout -b feature/feat_name
- Open a pull request into `main`.
- **Always** make sure to commit `.meta` files.

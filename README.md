# Cognitive Map Research Project

This repository contains the Unity application and associated files used for research on cognitive mapping in humans. The goal of the project was to investigate how individuals construct cognitive maps during the exploration of a maze.

## Overview

The Unity application developed for this research reads Excel files to construct mazes, which subjects then navigate. The application logs the exploration process, creating output Excel files that record how subjects interacted with the maze.

## Application Workflow

1. **Reading Excel Files**:
    - The application begins by reading input Excel files. These files contain the necessary parameters and layout information to construct the maze.

2. **Maze Construction**:
    - Utilizing the data from the Excel files, the application procedurally generates a 3D maze within the Unity environment. This maze serves as the experimental setting for subjects to explore.

3. **Experimentation and Data Collection**:
    - Subjects explore the maze in Unity, and their movements and decisions are logged in real-time.
    - Upon completion of the experiment, the application generates an output Excel file. This log file contains detailed records of how each subject explored the maze, which is crucial for further analysis.

## File Structure

- `/MazeGeneration` - Contains Unity scripts and assets for maze generation.
- `/ExcelFiles/Input` - Directory for input Excel files that define the maze parameters.
- `/ExcelFiles/Output` - Directory where the application saves the exploration log files.

## Running the Application

1. Ensure you have Unity Editor installed (version used for this project: [specify version]).
2. Clone the repository to your local machine.
3. Open the project in Unity by navigating to the cloned repository folder.
4. To run an experiment, go to the main scene and hit the 'Play' button in the Unity Editor.

## Post-Experiment Analysis

- The output Excel files are located in the `/ExcelFiles/Output` directory.
- Use the provided analysis scripts (if any) or your preferred data analysis tools to evaluate the cognitive mapping process.

## Contributing

If you wish to contribute to this research project, please follow the standard procedure:
- Fork the repository.
- Make your changes and test them.
- Submit a pull request with a comprehensive description of the changes.

## License

The code and data in this repository are the intellectual property of the original researchers. Use of the materials for academic or research purposes is welcome, provided that proper credit is given.

---

For any queries or further information, please contact the repository maintainer.

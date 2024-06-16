# ETABS to Revit Model Converter
This project provides a solution for converting structural models from ETABS to Revit. The primary goal is to facilitate the integration and interoperability between these two popular structural engineering software tools.
## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Requirements](#requirements)
- [Installation](#installation)
- [Usage](#usage)
- [Configuration](#NoCopyright)

## Introduction

The ETABS to Revit Model Converter allows users to convert structural models created in ETABS to Revit format. This tool streamlines the workflow for structural engineers and BIM professionals, enabling seamless data exchange and reducing the need for manual rework.

## Features

- Convert ETABS models to Revit format.
- Support for structural elements including beams, columns, slabs, and walls.
- Preserve material properties and section dimensions.
- Automatic unit conversion.
- Configurable mapping of ETABS elements to Revit families and types.

## Requirements

- .NET Framework 4.8 or later
- Revit 2021 or later
- ETABS 2018 or later
- Visual Studio 2019 or later (for development)
## Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/huyngo1412/VibrantBIM.git
    ```

2. Open the solution in Visual Studio:
    ```sh
    start VibrantBIM.sln
    ```

3. Restore the NuGet packages:
    ```sh
    nuget restore
    ```

4. Build the solution:
    ```sh
    msbuild
    ```
## Usage

1. Launch Revit and load the ETABS to Revit Converter add-in.

2. Open the ETABS model you want to convert in ETABS.

3. Export the ETABS model to an CXV file using the provided ETABS script:

4. In Revit, open the ETABS to Revit Converter add-in and select the exported CXV file.

5. Configure the mapping settings if necessary, and start the conversion process.

6. The converted model will be imported into Revit.

## No Copyright
This project is provided "as is" without any copyright. You are free to use, modify, and distribute this project as you see fit.

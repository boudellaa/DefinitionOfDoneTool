# Definition of Done Tool

## Overview

The **Definition of Done Tool** automates and simplifies the process of submitting and checking the steps you need to complete in your task. It helps streamline the workflow by ensuring that all required steps are clearly outlined and easily tracked through the application.

The repository contains two main projects:
1. **Done Tool**: This project is the core of the application, designed to handle the user interface and interaction with the task checklist.
2. **DataMiner NuGet Package Solution**: This package is responsible for managing JSON data, where the `TaskID` is stored.

## How to Use (Development)

1. Clone the repository.
2. Open the cloned project in Visual Studio.
3. Navigate to the **Done Tool** project and add a file named `appsettings.Secrets.json` with the following structure:

   ```json
   {
     "SkylineApi": {
       "Username": "SKYLINE2\\your_username",
       "Password": "your_password"
     }
   }

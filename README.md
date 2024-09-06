# SoftUni Exam Projects - Integration Tests - Databases
[![C#](https://img.shields.io/badge/Made%20with-C%23-239120.svg)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-5C2D91.svg)](https://dotnet.microsoft.com/)
[![MS SQL Server](https://img.shields.io/badge/Database-MS%20SQL%20Server-CC2927.svg)](https://www.microsoft.com/en-us/sql-server)
[![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework-Core-512BD4.svg)](https://github.com/dotnet/efcore)
[![NUnit](https://img.shields.io/badge/tested%20with-NUnit-22B2B0.svg)](https://nunit.org/)

### This is a test project for **Back-End Test Technologies** January 2024 Course @ SoftUni.
---

## Project Overview

The Integration-Tests-Databases project demonstrates the implementation of integration testing for console-based applications built using the .NET Core Framework and Entity Framework. It highlights how to test the interaction between the application and its database layer to ensure correct functionality across different components. 

## Key Concepts

- **Integration Testing:** Verifies that various components of an application function together as expected, including the interaction with external systems like databases.
- **Database Testing:** Ensures that the application properly interacts with the database, handles data retrieval, updates, and other operations correctly.

## Technologies Used

- **.NET Core**: The framework used to develop the console applications.
- **SQL Server**: The database management system used for data storage.
- **Entity Framework Core**: An Object-Relational Mapping (ORM) tool used to manage database operations.
- **nUnit**: A unit-testing framework for .NET applications.

## Projects Included
The repository contains multiple projects, each representing a different console application with a focus on specific functionalities:

- **ContactsConsoleApi**: Simulates a contact book application.
- **ProductConsoleAPI**: Simulates product management operations.
- **TownsConsoleApi**: Simulates an address management system for towns.
- **ZooConsoleApi**: Simulates the management of zoo data, including animal records.

## Project Structure

- **ConsoleAPI**: This is the main project directory containing the console application and integration tests.
    - **ConsoleAPI.csproj**: The project file for the console application.
    - **Program.cs**: The entry point of the application.
    - **DataAccess.cs**: Contains the Entity Framework context for handling database interactions.
    - **Data.cs**: Represents the data models used by the application.
    - **Business.cs**: Contains the business logic for managing the application's data.

- **ConsoleAPI.IntegrationTests.NUnit**: The test project folder containing integration tests.
    - **IntegrationTests.csproj**: The project file for the integration tests.
    - **TestDbContext.csproj**: Project file for the test database context used in the integration tests.

## Contributing
Contributions are welcome! If you have any improvements or bug fixes, feel free to open a pull request.

## License
This project is licensed under the [MIT License](LICENSE). See the [LICENSE](LICENSE) file for details.

## Contact
For any questions or suggestions, please open an issue in the repository.

---
### Happy Testing! 🚀

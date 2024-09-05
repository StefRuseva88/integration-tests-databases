# Integration-Tests-Databases
[![C#](https://img.shields.io/badge/Made%20with-C%23-239120.svg)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-5C2D91.svg)](https://dotnet.microsoft.com/)
[![MS SQL Server](https://img.shields.io/badge/Database-MS%20SQL%20Server-CC2927.svg)](https://www.microsoft.com/en-us/sql-server)
![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)
[![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework-Core-512BD4.svg)](https://github.com/dotnet/efcore)
[![NUnit](https://img.shields.io/badge/tested%20with-NUnit-22B2B0.svg)](https://nunit.org/)

### This is a test project for **Back-End Test Technologies** January 2024 Course @ SoftUni.
---

## Project Description

**Integration-Tests-Databases** is a test project designed to demonstrate integration testing with databases for console-based applications built using the .NET Core Framework and Entity Framework. 

## Key Concepts

- **Integration Testing**: Ensuring different parts of the application work together as expected.
- **Database Testing**: Verifying that the application correctly interacts with the database.

## Technologies Used

- **.NET Core**: The primary framework used for building the console applications.
- **SQL Server**: The database management systems used for storing application data.
- **Entity Framework Core**: ORM and database drivers for interacting with databases.
- **nUnit**: A unit-testing framework for .NET applications.

## Projects Included

- **ContactsConsoleApi**: Console program for simulating a contact book application.
- **ProductConsoleAPI**: Console program for simulating a product managing application. 
- **TownsConsoleApi**: Console program for simulating a town adress application.
- **ZooConsoleApi**: Console program for simulating a zoo managing application.

## Project Structure

- **ConsoleAPI**: The main project folder containing the console application and integration tests.
    - **ConsoleAPI.csproj**: Project file for the console application.
    - **Program.cs**: The main entry point of the application.
    - **DataAccess.cs**: The Entity Framework context for managing database operations.
    - **Data.cs**: The model representing the data structure.
    - **Business.cs**: Contains business logic for managing data.

- **ConsoleAPI.IntegrationTests.NUnit**: The test project folder containing integration tests.
    - **IntegrationTests.csproj**: Project file for the test project.
    - **TestContactDbContext.csproj**

## Contributing
Contributions are welcome! If you have any improvements or bug fixes, feel free to open a pull request.

## License
This project is licensed under the [MIT License](LICENSE). See the [LICENSE](LICENSE) file for details.

## Contact
For any questions or suggestions, please open an issue in the repository.

---
### Happy Testing! 🚀

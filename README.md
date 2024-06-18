# Integration-Tests-Databases
![image](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![image](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![image](https://img.shields.io/badge/Visual_Studio-5C2D91?style=for-the-badge&logo=visual%20studio&logoColor=white)
## This is a test project for Back-End Test Technologies January 2024 Course @ SoftUni.
---

### Project Description

**Integration-Tests-Databases** is a test project designed to demonstrate integration testing with databases for console-based applications built using the .NET Core Framework and Entity Framework. 

## Key Concepts

- **Integration Testing**: Ensuring different parts of the application work together as expected.
- **Database Testing**: Verifying that the application correctly interacts with the database.

## Technologies Used

- **.NET Core**: The primary framework used for building the console applications.
- **SQL Server**: The database management systems used for storing application data.
- **Entity Framework Core**: ORM and database drivers for interacting with databases.
- **nUnit**: A unit-testing framework for .NET applications.

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

### Contributing
Contributions are welcome! If you have any improvements or bug fixes, feel free to open a pull request.

### License
This project is licensed under the [MIT License](LICENSE). See the [LICENSE](LICENSE) file for details.

### Contact
For any questions or suggestions, please open an issue in the repository.

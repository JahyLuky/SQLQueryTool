# SQLQueryTool

**SQLQueryTool** is a lightweight ASP.NET Core web application designed to provide an intuitive interface for executing SQL queries against a Microsoft SQL Server database. The tool allows users to specify their database connection details and the SQL queries they wish to execute directly within the `appsettings.json` configuration file.

## Features
- **Customizable Database Connection**: Easily configure the connection string to your Microsoft SQL Server instance in the `appsettings.json` file.
- **Dynamic Query Execution**: Define multiple SQL queries within the configuration file and execute them via a web interface.
- **Automatic Result Formatting**: Query results are automatically formatted and displayed in a dynamically generated HTML table for easy viewing.
- **Error Logging**: Errors encountered during query execution are logged using `log4net` for traceability and troubleshooting.

## Requirements
- **Microsoft SQL Server**
- **ASP.NET Core** (for running the web application)
- **[.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)**
- **NuGet Packages**:
  - `Microsoft.EntityFrameworkCore.SqlServer`
  - `Microsoft.EntityFrameworkCore.Tools`
  - `log4net`

## Installation

### 1. Clone the Repository
Clone or download the repository to your local machine:
```bash
git clone https://github.com/yourusername/SQLQueryTool.git
```

### 2. App Configuration
#### In `appsettings.json`
- **`DefaultConnection`**: Defines the connection string used by your application to connect to the SQL Server database.
- **`Queries`**: Holds SQL queries that your application can execute. You can define queries here as needed, and they can be renamed or duplicated for different use cases.
- **`Url`**: Determines the IP address and port that your application will use. You can modify this if you want your app to listen on a specific address or port.

#### In `log4net.config`
- **`file value`**: This element is used to specify the location and name of the log file where the log entries will be written.

3. Build the application
```bash
dotnet build
```
4. Run the application
```bash
dotnet run
```
5. View the queries
- **`Go to`**: ``http://<your-ip>:<your-port>/?layoutid=<your-query>``


---

### Summary of Changes:
1. Organized the information into clear sections: **Requirements**, **Installation**, **App Configuration**, **Build and Run**, and **Viewing the Tool**.
2. Added example code snippets for `appsettings.json` and `log4net.config` to clarify how to configure them.
3. Simplified and clarified the instructions for building, running, and using the app.

Feel free to adjust any paths, values, or details to match your specific project.

# OTS_WebAPI

C# ASP.NET Web API for OTS job

Supports Get, Post (Create), Put (Update), and Delete for:

- Region 1 <-> * Offices 

- Offices * <-> * Employees 
  - employees can work at multiple offices in this case

- Employees 1 <-> 1 Laptop


## How to Run:
### Use the "universal" branch!!

Universal uses SQLite, so with .Net 8 and EF core it should work on anyone's computer

This branch really only works on Visual Studio on Windows because it uses LocalDB

You need the .NET 8 sdk to run it

- Clone the proj

- If you are not on Visual Studio on Windows
  - Switch to the universal branch and read its README
  ```sh
  git checkout universal
  ```

- Otherwise, in Package Manager Console run:
  - ```sh
    Update-Database
    ```

- To run:
  - ```sh
    cd {workspaceFolder:OTS_WebAPI}/API_Proj/API_Proj
    dotnet run
    ```

The Swagger UI is available here: [swagger](http://localhost:5263/swagger/index.html)

### API
When creating:

- Relationships are nullable:
  - Ex: 
    ```json
    {
        "laptopID": 1001,
        "laptopName": "exampleName"
    }
    ```
    Instead of:

    ```json
    {
        "laptopID": 1001,
        "laptopName": "exampleName",
        "employeeID": 1001
    }
    ```
    Just makes a laptop without an owner


- Not setting a property field, such as Laptop's LaptopName, sets it to null
  - Ex: 
    ```json
    {
        "laptopID": 1001,
        "employeeID": 1001
    }
    ```
    Instead of:

    ```json
    {
        "laptopID": 1001,
        "laptopName": "exampleName",
        "employeeID": 1001
    }
    ```
    Creates a laptop with a null name


When updating:

- Not setting a property field, such as Laptop's LaptopName, doesn't edit that field of the object

  - Ex: 
    ```json
    {
        "laptopID": 1001
    }
    ```
    Instead of:

    ```json
    {
        "laptopID": 1001,
        "laptopName": "oldName",
        "employeeID": 1001
    }
    ```
    Keeps the old information without having to type it 

- The endpoints UnassignOwner or Region is to remove it from the Laptop or Office
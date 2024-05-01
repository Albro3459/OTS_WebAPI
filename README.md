# OTS_WebAPI

C# ASP.NET Web API for OTS job

Supports Get, Post (Create), Put (Update), and Delete for:

- Region 1 <-> * Offices 

- Offices * <-> * Employees 
  - employees can work at multiple offices in this case

- Employees 1 <-> 1 Laptop


### How to Run:
I have only tested on VScode

I use the SQLtools extension with SQLtools SQLite extension, but I don't think that is needed

You need the .NET 8 sdk and EF Core installed in your cli to run it 

- Clone the proj

- To run:
  - ```sh
    dotnet ef database update
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
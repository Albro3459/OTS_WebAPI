# OTS_WebAPI
web api for ots job

Supports Get, Post (Create), Put (Update), and Delete
for:
Region 1 <-> * Offices
Offices * <-> * Employees (employees can work at different offices in this case)
Employees 1 <-> 1 Laptop


### How to Run:
You need the .net 8 sdk to run it

clone the proj

In the project root directory/API_Proj/API_Proj
run:
dotnet run

The swagger ui is available at:
http://localhost:5263/api/swagger/index.html
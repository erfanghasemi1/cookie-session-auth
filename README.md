My purpos of developing this minimal project was just using cookie and session . 
--------------------------------------------------------------------------------------------------------------------------------------------

How to Run :

1 : Clone the project 

2 : Change directory to the project 

3 : Create a MySQL database and run the SQL schema in /Database/schema.sql

4 : Add a valid appsettings.json: {"Encryption": {
  "Key": "your-base64-encoded-key-here",
  "IV": "your-base64-encoded-iv-here"
},
 "ConnectionStrings": { "mysqlconnection": "Server=localhost;Database=shopdb;Uid=root;Pwd=yourpassword;" } }

5 : Add CORS piece of code if you need it ( e.g., Developing front-end )

6 : Run the project => dotnet run

-------------------------------------------------------------------------------------------------------------------------------------------

APIs :
Just 3 APIs that you can check them in Controllers folder ( for /signup and /login you should check Middleware folder too ).

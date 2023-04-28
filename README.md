# SQL-WindowsFormApp
Step 1:

For a database, a Windows application was created using the .NET framework. The application contains a window through which a user can manipulate the data of 2 tables in a 1-n relationship (which we will call the parent table and the child table). The following functionalities are implemented:
- to display all records of the parent table;
- when selecting a record from the parent, all the records of the child table will be displayed
- when selecting a record from the son, it must be allowed to delete or update its data
- having selected a record from the parent, it is allowed to add a new child record.
Data sets and data adapters were used for communication with the database

Step 2:

Generalize the project on the Step 1, such that it will work for at least 2 scenarios (2 relations 1- n from database).
It was considered a Panel in the form to create TextBoxes for the fields of the tables involved.
Everything that is related to tables and the CRUD operations (table name, fields, parameters, ...) is set in an XML file: App.config.

## C# Basics - Beginner

- Create a new console app with `dotnet new console`
- Add name with `-n` or `--name`
- Add path for project with `-o` or `--output` to create a new project in a different directory
- `-lang` or `--language` to use a different language e.g. F#, Visual Basic
- Run `dotnet nuget list source` to see what sources for C# libraries have been added. `nuget.org` seems to be like `pypi`.
- There have been some changes to the default templates with .NET 6. https://learn.microsoft.com/en-us/dotnet/core/tutorials/top-level-templates
- Name of the project should match the namespace name (still runs if it doesn't)
- Storing decimals in variables, 3 types, ordered by size (bytes)
    - `float` 4 byte (32 bit)
    - `double` 8 byte (64 bit)
    - `decimal` 16 byte (128 bit)
- `float` values need to have an `f` after to be considered a valid `float` type e.g. `0.567f`. Compiler assumes a decimal number is a `double` by default. `decimals` are notated with an `m`.
- Strings are stored in double quotes. Single characters `char` are stored in single quotes. 
- Variable decleration structure:
    - `data type` `name` = `value`;
- C# is statically typed. Variable types are static, so the type can't change. 

### Data Structures

#### Arrays

- Declare as string with [] after `string[] myStringArray = new string[2];`
- Assign values to an index in the array `myStringArray[0] = "banana";`
- Assigning a value to an index greater than the number of items in the array results in `System.IndexOutOfRangeException: Index was outside the bounds of the array.`
- You can declare values in the array along with the variable definition `string[] mySecondStringArray = {"banana", "eggs"};`


#### Lists
- More dynamic in length - additional memory will be assigned as it's needed.
- Generic type. Declare like `List<string> myNewList = new List<string>();`. Parenthesis because we are calling the constructor of the class to create a new instance of a List class. Can pass any type in to the list.
- Dont have to give a size or values, and it will be dynamic. Empty list has len 0, and it will be able to dynamically increase in size as more things are `Add()`ed.
- IEnumerable is like a list, but faster to loop through with a `ForEach`. If looking at every element, use `IEnumberable`, if accessing a single element, or dynamically adding/removing, use a `List<>`.
- A list or array must be assigned to the IEnumerable - cannot delcare in the same way we have for arrays and lists. It looks to be like Python generators

#### Dictionaries
- Works different to arrays - interrogate with keys rather than indices.
    ```
    Dictionary<string, string[]> myDictionary = new Dictionary<string, string[]>(){
                    {"Fruit", new string[]{"banana", "apple", "orange"}},
                    {"Vegetables", new string[]{"carrot", "potato", "onion"}}
                };
                
    Console.WriteLine(myDictionary["Fruit"]);  
    ```

### Operators and conditional statements
- `++` adds one to your int e.g. `myInt++;`
- `+=` adds a value to your int e.g. `myInt += 7;` adds 7. Works on strings too. 
- `-=` subtracts a value from the int `myInt -= 8;` subtracts 8
- `Math.Pow` for exponents
- `Math.Sqrt` for square root
- `.Split`, `.Remove`, `.Replace`, `.Reverse` functions all here. `string[] myStringArray = myString.Split('.');`
- `.Equals` is the same as `==`, 
- `&&` == and. `||` == or.

### Conditional Statements
- working with `if`,`else if`, and `else` just the same as anywhere.
- `switch` is new - this can be used to build conditional logic when you know the expected values of a variable. Has a `default` case, and any other conditional `cases`.
    ```
        string myFirstValue = "some words";
        string mySecondValue = "Some Words";

        if (myFirstValue == mySecondValue)
            {
                Console.WriteLine("equal");
            }
        else if (myFirstValue == mySecondValue.ToLower())
            {
                Console.WriteLine("equal without case sensitivity");
            }
        else
            {
                Console.WriteLine("not equal");
            }
    ```

### Loops

- `for` has set number of times
    ```
    // For Loop approach
    Console.WriteLine("-- For Loop approach --");
    totalOfArray = 0;
    startTime = DateTime.Now;

    for (int i=0; i < intsToCompress.Length; i++)
    {
        totalOfArray += intsToCompress[i];
    }

    Console.WriteLine($"Time taken: {(DateTime.Now - startTime).TotalSeconds}");
    Console.WriteLine($"Total of array: {totalOfArray}");
    ```
- `foreach` knows to deconstruct the array. Means you don't need to reference the array by the current index as in the for loop - you can reference each element in the array with the variable name you give in the initialization of the foreach.
    ```
    // foreach approach
    Console.WriteLine("-- foreach approach --");
    startTime = DateTime.Now;

    totalOfArray = 0;
    foreach (int i in intsToCompress){
        totalOfArray += i;
    }

    Console.WriteLine($"Time taken: {(DateTime.Now - startTime)}");
    Console.WriteLine($"Total of array: {totalOfArray}");
    ```
- while - until a conditional is false. Must set the conditional before the loop is started.
    ```
    // while loop
    Console.WriteLine("-- while loop --");
    int index = 0;
    totalOfArray = 0;
    startTime = DateTime.Now;

    while(index < intsToCompress.Length){
        totalOfArray += intsToCompress[index];
        index++;
    }

    Console.WriteLine($"Time taken: {(DateTime.Now - startTime)}");
    Console.WriteLine($"Total of array: {totalOfArray}");
    ```
- do while - run as long as a contitional is true. conditional is checked after each execution, not before. 
    ```
    // do while loop
    Console.WriteLine("-- do while loop --");
    index = 0;
    totalOfArray = 0;
    startTime = DateTime.Now;

    do {
        totalOfArray += intsToCompress[index];
        index++;
    }
    while(index < intsToCompress.Length);

    Console.WriteLine($"Time taken: {(DateTime.Now - startTime)}");
    Console.WriteLine($"Total of array: {totalOfArray}");
    ```
- conditionals in loops
    ```
    // total of all the even numbers
    Console.WriteLine("-- sum of even numbers --");
    totalOfArray = 0;
    foreach (int i in intsToCompress){
            if (i % 2 == 0){
                totalOfArray += i;
        }
    }
    Console.WriteLine($"Total of even numbers in array: {totalOfArray}");
    ```

### Methods/Arguments/Return

- A method is a named member of a class that executes a code block.
- `private void` method where not expecting a return value.
- `private int MethodName()` for a method that returns an int.
- Main method on console app needs to be static, can only so also need to declare the method as static `static private int GetSum()..`. 
- the method makes sense but I'm not sure how the public/private thing works, or why I can't call the method without instantiating a new instance of the program. 
- Example of using void to do something on a condition:
    ```
    public void PrintIfOdd(int number){
        if (number % 2 == 1){
            Console.WriteLine(number);
        }
    }
    ```
    looks like void means we can return if we want, but no problem if we don't. Without void, if there is nothing returned then it will not work.

### Scope

- Means we can have variables with the same name in different places, so need to be able to tell the difference between them.
- Depth of indent / braces is the scope. cant access objects from a lower scope higher up.
- Static methods can only access other static variables. Non static methods can access static or non static attributes. C# will ignore variables defined at a higher scope if there is a variable with the same name declared at the local scope.
    - better to have local names for local variables. `PascalCase` should be used for variables at the class level, and `camelCase` for local variables, so this should prevent confusion and conflicts too. 

## C# Basics - Intermediate

### Models
- mapping data from one place to another - e.g. from db to app. Can also be used to manage data within the app, and map between multiple applications too. 
    ```
    public class Computer
    {
        public string Motherboard
        public int CPUCores
        public bool HasWiFi
        public bool HasLTE
        public DateTime ReleaseDate
        public decimal Price
        public string VideoCard
    }
    ```
- public attributes so they can be interacted with.
- Note: no semicolons at the end of the lines. If you did, it would create a `field`, which is against C# best practice. If you need a field, make it `private`, follow differet naming convention - `public string Motherboard` becomes `private string _motherboard;`, and create another attribute `private string Motherboard {get; set;}` 
    - `get` and `set` used to retrieve and set the values of the _motherboard field on the Computer class. Motherboard becomes a property, which stays private. So the above class becomes:
        ```
        public class Computer
        {
            private string _motherboard;
            private string Motherboard { get { return _motherboard; } set { _motherboard = value; } }
            public int CPUCores
            public bool HasWiFi
            public bool HasLTE
            public DateTime ReleaseDate
            public decimal Price
            public string VideoCard
        }
        ```
    - This is the best practice to manage fields on the model - get the value from a property, which pulls from a field via a getter. Don't interact directly with the field. 
    - C# has evolved and made this easier for us too. The above becomes:
        ```
            public class Computer
        {
            private string Motherboard { get; set; }
            ...
        }
        ```
        which has the exact same functionality.
    - Generally we have attributes as private properties of the object (i.e. `private` with get and set) rather than public fields. It's best practice, and other libraries will often not look for public fields on objects. So, our `Computer` becomes:
        ```
        public class Computer
        {
            private string? Motherboard { get ; set ; }
            public int CPUCores { get ; set ; }
            ...
        }
        ```
        - Note the `?` on the strings - more recently, strings have become non-nullable by default. The ? makes them nullable, and some folks are doing this as a workaround to maintain existing behavior. Instead, what we should do is define a constructor that enforces and gracefully handles this new behavior.
            ```
            public class Computer
            {
                public string Motherboard { get; set; } = "";
                public int CPUCores { get; set; }
                public bool HasWifi { get; set; }
                public bool HasLTE { get; set; }
                public DateTime ReleaseDate { get; set; }
                public decimal Price { get; set; }
                public string VideoCard { get; set; } = "";
            } 
            ```
        
- can also map out to json
- class with properties that match the field names in the table, to pull it out into the model with the properties of the model.

### Namespaces
- Way to organise the code and reference it from other places. Tells C# where the code lives, so it can be found and accessed at execution time.
- `mkdir` the Models directory. Add the namespace - currently matching the one we are currently working in. Then drop the model in. [Example](2-console-app\Models\Computer.cs).
- This will break the code in Main until the model has been imported - either `ctrl+.` on the `Computer myComputer = new Computer()` issue and generte the import statement, or add it in at the top manually `using MyApp.Models;`

### Database connections
- Running database in container
    - `docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password123" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest`
- `dotnet add package` to add a new package to the project 
    - `dapper` for easy sql connection/pushdown sql
    - `microsoft.data.sqlclient` to enable connection to mssql
    - `microsoft.entityframeworkcore` generates SQL for you with methods on the c# objects - a more comprehesive way to build and interact with sql objects via c#
    - `microsoft.entityframeworkcore.sqlserver`
- Verify package installation by checking `.csproj` file

#### Dapper

- Aaaand we can connect to the db:
    ```
    string connectionString = "Server=localhost;Database=DotNetCourseDatabase;TrustServerCertificate=true;Trusted_Connection=false;User Id=xxxxx;Password=xxxxx;";

    IDbConnection dbConnection = new SqlConnection(connectionString);

    string sqlCommand = "SELECT GETDATE();";

    var resp = dbConnection.QuerySingle<string>(sqlCommand);

    Console.WriteLine("Current Date: " + resp);
    ```
- Now writing queries to manipulate data `string insertSql = @"";`. The `@` allows you to write multi-line string.
    ```
    // Inserting data
    string insertSql = @"INSERT INTO TutorialAppSchema.Computer 
    (
        Motherboard, 
        CPUCores,
        HasWifi,
        HasLTE,
        ReleaseDate,
        Price,
        VideoCard
    )
    VALUES
    ('" +
    myComputer.Motherboard + "', '" +
    myComputer.CPUCores + "', '" +
    myComputer.HasWiFi + "', '" +
    myComputer.HasLTE + "', '" +
    myComputer.ReleaseDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" +
    myComputer.Price + "', '" +
    myComputer.VideoCard 
    + "')";

    Console.WriteLine(insertSql);

    int result = dbConnection.Execute(insertSql);

    Console.WriteLine("Response from SQL : " + result);

    // Reading data
    string sqlSelect = @"SELECT 
            Computer.Motherboard, 
            Computer.CPUCores,
            Computer.HasWifi,
            Computer.HasLTE,
            Computer.ReleaseDate,
            Computer.Price,
            Computer.VideoCard
        FROM TutorialAppSchema.Computer
        ";

    IEnumerable<Computer> computers = dbConnection.Query<Computer>(sqlSelect);

    foreach(Computer singlecomputer in computers)
    {
        Console.WriteLine(singlecomputer.Motherboard + "', '" +
            singlecomputer.CPUCores + "', '" +
            singlecomputer.HasWiFi + "', '" +
            singlecomputer.HasLTE + "', '" +
            singlecomputer.ReleaseDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" +
            singlecomputer.Price + "', '" +
            singlecomputer.VideoCard);
    }
    ```
- Can convert IEnumberable to a List with ToList(). 
- Now to refactor the Dapper code into a module. Dapper code going in a new folder called `Data`. All data logic in here `DataContextDapper.cs`
- Creating a new class `DataContextDapper` in the `MyApp.Data` namespace. Connection string becomes a field on the class, and connection will be used inside the method.
    - `private string _connectionString = "Server=localhost;Database=DotNetCourseDatabase;TrustServerCertificate=true;Trusted_Connection=false;User Id=xxxxx;Password=xxxxx;";`
        - set string as `private` and update the name to have an underscore before as this is a private field that is only accessible inside the class. The `_` is there because we could technically have something that has the same name. Prevents clashing variable names with variables in methods on the class.
    - Two methods, one to send data and one to get data. Public because we want these to be accessible on any instance of the class and call the method. Then what we want it to return. `IEnumerable<T>`, which is using generics, so we can dynamically use types and models to return different types of the same thing. `T` is generic type.
    - `(string sql)` because we want to pass in the sql string
    - Getting error on `<T>` - haven't defined the type for when the method is called. Shows `The type or namespace name 'T' could not be found (are you missing a using directive or an assembly reference?) CS0246`. Need to add dynamic type as input on the method, so the type is also passed in when calling, to resolve.
        - `public IEnumerable<T> LoadData(string sql)` becomes `public IEnumerable<T> LoadData<T>(string sql)`
    - We add some more methods for inputting data:
        ```
        public T LoadDataSingle<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.QuerySingle<T>(sql);
        }

        public bool ExecuteSql(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return (dbConnection.Execute(sql) > 0);
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Execute(sql);
        }
        ```
    - and then remove references to `dbConnection` and replace them with the methods in our class.

#### Entity Framework
- New file in data folder for ef base logic
- Going to use inheritance. Take one class, and make it have everything that another class has. We do this with a colon after the class name definition:
    `public class DataContextEntityFramework : DbContext`
- `F12` takes you to the class definition of imported objects - useful to see what functions are available and how it works
- Going to overwrite some existing methods to do what we want to do. `protected override void` because we don't want to return anything. `OnConfiguring`, which takes a `DbContextOptionsBuilder` as an argument. It's called when DbContext is created. So we're going to pass in the connection string here.
- `=>` is callback function. We're using this to retry on failure. `options => options.EnableRetryOnFailure()` is passed in as another argumet to `UseSqlServer`.
- Added a property to the class, to tell it what models to connect to. It will look for tables that match the models it can affect, so data in the SQL table can be manipulated with EF.
    - `public DbSet<Computer>? Computer { get; set; }`
    - want this to be nullable (in case there are no Computers in the table?), hence the `?`
    - also need the `using` statement
    - we now have methods that access that connect into the DbSet when we access the DataContext class
- We also need something to find the table - `OnModelCreating`. We call the `Entity` method on the `modelBuilder`: `modelBuilder.Entity<Computer>();`. It will look on `dbo` schema by default, but we can override this:
    ```
    modelBuilder.Entity<Computer>()
    .ToTable("Computer", "TutorialAppSchema");
    ```
- We can also reset the default schema:
    `modelBuilder.HasDefaultSchema("TutorialAppSchema");`
- The `ToTable` method will look for a table with the same name as the model by default.
- So then to use this
    ```
    DataContextEntityFramework entityFramework = new DataContextEntityFramework();
    
    Computer myComputer = new Computer()
    {
    // Populate variables in Computer class
    }

    entityFramework.Add(myComputer);
    entityFramework.SaveChanges();
    ```
- So this is quite straight forward to write data to the table. But we have a lot less control - if we want to do anything outside of writing to the table from the object, EF isn't going to help us.
- Quite straight forward to get data from sql too `IEnumerable<Computer>? computersEf = entityFramework.Computer?.ToList<Computer>();`. Need to ensure that potential null values are handled in the Model - if db returns null and the properties of the class are not nullable, then it will error.

### Config

- Removing secrets e.g. `connectionString` from the code we'll be working on.
- Create `appsettings.json` in the project root, with the following format:
    ```
    {
        "ConnectionStrings": { 
            "DefaultConnection":"Server=xxx;"
        }
    }
    ```
    > NB: ConnectionStrings key is what the ConfigurationBuilder looks for
- Nice, good to go. To pull through to the Data classes, we use `ConfigurationBuilder`. New package `Microsoft.Extensions.Configuration`. Also need `Microsoft.Extensions.Configuration.Json`. Install with `dotnet add package [package_name]`
- Run `dotnet restore` after install to refresh csproj and pull through context of new packages that have been installed.
- Here is how we pull through the appsettings:
    ```
    IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
    ```
    - Without calling the `.Build()` method, this will not compile - this is because the `ConfigurationBuilder` class is not a `IConfiguration` type. But once built, it is, as it returns a Configuration.  
    - We can now pass this configuration object when we create a new instance of the classes, to access the config. This can be done in two ways:
        1. Pull through the whole object to the class and call the `GetConnectionString` method on that object in the code
            ```
            private IConfiguration _config;
            public DataContextEntityFramework(IConfiguration config)
            {
                _config = config;
            }
            ...
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection")

            ```
        1. Store the connection string in a private property on the class after pulling it out when creating a new instance of the class
            ```
            private string _connectionString;
            public DataContextDapper(IConfiguration config)
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                if (connectionString is null)
                {
                    throw new ArgumentNullException(nameof(connectionString), "DefaultConnection is not found in the configuration.");
                }
                _connectionString = connectionString;
            }
            ```

### Reading and writing files
- Easy way to write file, `File.WriteAllText("log.txt", insertSql);`
- Running the code again as is above will overwrite the file.
- Using `StreamWriter`, we can append data to the bottom of a file.
    ```
    using StreamWriter openFile = new("log.txt", append: true);

    openFile.WriteLine(insertSql);
    ```
- To read, very similar: `File.ReadAllText("log.txt")`

### Parsing json
- Working now with a jsonl file.
- Going to read the json file into an `IEnumberable` as we want to be able to process them all, and don't need to add records or access specific instances from the array.
    ```
    // Read the text into a string
    string computersJson = File.ReadAllText("Computers.json");

    // Set the JsonSerializerOptions as inbound data conforms to camel case
    JsonSerializerOptions options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    // And ingest into an IEnumerable of Computers (nullable!)
    IEnumerable<Computer>? computers = JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJson, options);
    ```
- Could have used `Newtonsoft.Json` instead
    `IEnumerable<Computer>? computers = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computersJson);`
- When serializing, both of these packages default to PascalCase keys to json files. So need to follow the same pattern of passing config/settings objects to the serialization method to get camelCase keys out:
    ```
    // Settings for Newtonsoft
    JsonSerializerSettings settings = new JsonSerializerSettings()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    string computerscopyNewtonsoft = JsonConvert.SerializeObject(computersNewtonsoft, settings);

    File.WriteAllText("computersCopyNewtonsoft.txt", computerscopyNewtonsoft);

    // Using the same options class from our reader
    string computerscopySystem = System.Text.Json.JsonSerializer.Serialize(computersSystem, options);

    File.WriteAllText("computersCopySystem.txt", computerscopySystem);
    ```
- We can then iterate over our computers to input them to the table
    ```
    if (computersSystem != null)
    {
        foreach (Computer computer in computersSystem)
        {
            string insertSql = @"INSERT INTO TutorialAppSchema.Computer 
            (
                Motherboard, 
                CPUCores,
                HasWifi,
                HasLTE,
                ReleaseDate,
                Price,
                VideoCard
            )
            VALUES
            ('" +
            EscapeSingleQuote(computer.Motherboard )+ "', '" +
            computer.CPUCores + "', '" +
            computer.HasWiFi + "', '" +
            computer.HasLTE + "', '" +
            computer.ReleaseDate?.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" +
            computer.Price + "', '" +
            EscapeSingleQuote(computer.VideoCard)
            + "')";
            dapper.ExecuteSql(insertSql);
        }
    }

    static string EscapeSingleQuote(string input)
    {
        string output = input.Replace("'", "''");

        return output;
    };
    ```
- Note the `EscapeSingleQuote` method - encountered an error where string values in `VideoCard` property has a quote - replace this with double apostrophe to resolve. There's got to be a better way to sanitize these string inputs!



### Simple model mapping
- Automapper
    - Field Renaming

- JSONAttributeName


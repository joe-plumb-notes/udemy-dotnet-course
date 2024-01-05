# API Development with C# .NET Core 8 and MS SQL Server

My notes from the udemy course https://www.udemy.com/course/net-core-with-ms-sql-beginner-to-expert/

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

### Model mapping

- Different use cases, but we're going to apply to the same. `Automapper` generally not for what we're going to use it for, but just to prove the point. Working with `ComputersSnake.json` file, which has properties in `snake_case`.
- Add Automapper with `dotnet add package AutoMapper`
    ```
    Mapper mapper = new Mapper(new MapperConfiguration((cfg) => {
                
            }));
    ```
- Need a destination and source to map between. Want to line up with `Models/Computer.cs`, so, created a new Model, `ComputerSnake.cs` with the properties in the format in the inbound file. Then:
    ```
    Mapper mapper = new Mapper(new MapperConfiguration((cfg) => {
                cfg.CreateMap<ComputerSnake, Computer>();
            }));
    ```
- Need to add different mapping options - this on it's own will be looking to map names that are the same from one to the other - and the names are not the same!
- We can do this based on `fields`, known as `members`.
    ```
        Mapper mapper = new Mapper(new MapperConfiguration((cfg) => {
        cfg.CreateMap<ComputerSnake, Computer>()
            .ForMember(destination => destination.ComputerId, options => 
                options.MapFrom(source => source.computer_id))
            .ForMember(destination => destination.Motherboard, options => 
                options.MapFrom(source => source.motherboard))
            .ForMember(destination => destination.CPUCores, options => 
                options.MapFrom(source => source.cpu_cores))
            .ForMember(destination => destination.HasWiFi, options => 
                options.MapFrom(source => source.has_wifi))
            .ForMember(destination => destination.HasLTE, options => 
                options.MapFrom(source => source.has_lte))
            .ForMember(destination => destination.ReleaseDate, options => 
                options.MapFrom(source => source.release_date))
            .ForMember(destination => destination.Price, options => 
                options.MapFrom(source => source.price))
            .ForMember(destination => destination.VideoCard, options => 
                options.MapFrom(source => source.video_card));      
    }));
    ```
- Can also convert value while mapping, which is good with this approach as you can embed logic in the conversion. It's also good when one of the models has fields the other one doesn't have.
- We perform the mapping with `IEnumerable<Computer> computersResult = mapper.Map<IEnumerable<Computer>>(computersSystem);`
- There's an easier way to do all of this - we can use the `JsonAttributeName` attribute on our original model to perform this mapping. Add the attribute directly above each of the properties on the model to enable the mapping directly from the consumed json into the `Computer` class.
    ```
    public class Computer
        {
            [JsonPropertyName("computer_id")]
            public int ComputerId { get; set; }
            [JsonPropertyName("motherboard")]
            public string Motherboard { get; set; } = "";
            ...
    ```

## API basics

- `startup.cs` was 'old' way of doing it in .net 6, so may come across this in legacy projects.
- `dotnet new webapi -n DotnetAPI` to create a new webapi project
- Focus on `Program.cs` for now.
    - `builder` creates the web server that runs, which listens for and responds to requests. 
    - `builder.Services.AddEndpointsApiExplorer();` maps the endpoints to make the routes available.
    - `builder.Services.AddSwaggerGen();` to add Swagger, which is made available when running in development environment:
        ```
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        ```
    - _They've updated the course to dotnet 8 here, and their boilerplate code is different._ The new sample you get when running `dotnet new webapi -n DotnetAPI` implements *Minimal APIs*, which was introduced in dotnet 6(!). It offers a simplified way to build APIs, especially for smaller applications or microservices.
        - The new example has 1 file, rather that `/Controllers`, which is probably good for small projects and examples but may not scale well. It's got some new functionality too, like: 
            ```
            app.MapGet("/endpoint" () => 
            {
                ...
            })
            ```
        - Still going to want to use controller infrastrucure in most situations
        - Seems to have also introduced `records` which are similar to classes. Implements properties as arguments. [More Here](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record).
    - They suggest putting `app.UseHttpsRedirection();` in a conditional statement after the `if (app.Environment.IsDevelopment())` logic - not great. Instead, we can run `dotnet dev-certs https --trust` to trust the development https cert, and launch the app with `dotnet run --launch-profile https` [more here](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio-code#test-the-project). We can then access the swagger OpenAPI spec at `swagger/index.html`, and call the api at `/WeatherForecast`.
    - Interestingly, no logs or command line output when we hit the API.
- When you get to developing front end with these APIs, enforcing HTTPS can make it more complex to develop. CORS = Cross Origin Resource Sharing - origin for us being the URL that our API lives at. Front end would live at a different URL, so CORS error would be returned if you did not have a policy. Enter, `builder.Services.AddCors()`
    ```
    builder.Services.AddCors((options) => 
    {
        options.AddPolicy("DevCors", (corsBuilder) =>
            {
                corsBuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
    ```

### Controllers
- Way to handle requests, and do things (go to database, transform data, etc).
- The default route will match the name of the controller (i.e. `WeatherForecastController` listens on `weatherforecast`). Not case sensitive.
- `[ApiController]` attribute marks the class as a controller that respondes to web API requests, and enables some default functionality and behaviours like model validation and automatic HTTP 400 responses.
- `[Route("[controller]")]` attribute is used to define the route - the `[controller]` placeholder gets replaced by the name of the controller - this is where the controller name gets mapped to the route.
- `ControllerBase` has properties and methods for handling HTTP requests. Base class for all controllers in an ASP.NET core app. Enables different action results e.g. `Ok()`, `NotFound()`, `BadRequest()`, support for model binding, which is a process that maps data from HTTP requests to action method parameters, and properties for accessing services that are registered in the dependency injection container, such as `HttpContext`, `User`, `Url`.

### Database connection

- Working with `appsettings.json` as always
    - Adding `ConnectionStrings` to the file is then retrievable (dotnet 6 and above) simply by instanciating an `IConfiguration` object and calling `config.GetConnectionString("DefaultConnection")`.
    - If in `startup.cs`, will need to code the parsing of the appsettings file to get the connection string. 
- Added `dapper`, `Microsoft.Data.SqlClient` and `automapper` packages to the solution.
- Created a new (the same as previous) [`Data/DataContextDapper.cs`](DotnetAPI\Data\DataContextDapper.cs) file, with methods to execute multi-line and single SQL commands. 
- Then, created a new instance of the `DataContextDapper` object and use it in a new endpoint on the `User` controller, to execute a SQL command to `GETDATE()`.
    ```
    DataContextDapper _dapper;
    public UserController(ILogger<UserController> logger, IConfiguration config)
    {
        _logger = logger;
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }
    ```
- It works!

### Getting data into the application from SQL

- Got some SQL queries against the sample database, going to build some models to map the data from the query response into the app. Two steps .. 

#### Build the objects to recieve the results

- `public partial class` enables you to add to the class from inside another file. Good to be partial, in case you need to add to them on the fly.
- To deal with the `this property is non-nullable` error, we could add a constructor to the class. Basically conditional logic that inserts default values if we see `null` values for a given `User`. Even better, we can provide default values along with the class properties.
- Building the models was straight forward - just mapped the datatypes from the database to datatypes in the model. `BIT` == `bool`, `NVARCHAR` == `string`, `DATETIME2` == `DateTime`.

#### Build the controllers

- We made a mistake in the last session - POCOs should have singular names, as you can't have one `Users`.
- After we were given the API endpoints that we were going to create I had a crack at implementing this myself. Tried to get both bits of functionality in the same endpoint (he had `GetUser` and `GetAllUsers` which is bad design)
    ```
    [HttpGet("Users/{userId?}")]
    public ActionResult<IEnumerable<User>> GetUsers(int? userId = null)
    {
        if (userId == null)
        {
            _logger.LogInformation("Users endpoint processed a request at " + DateTime.Now + ".");

            string query = @"SELECT  [UserId]
                            , [FirstName]
                            , [LastName]
                            , [Email]
                            , [Gender]
                            , [Active]
                            FROM TutorialAppSchema.Users";

            return Ok(_dapper.LoadData<User>(query));
        }

        else
        {
            _logger.LogInformation("C# User endpoint processed a request at " + DateTime.Now + ".");

            string query = @"SELECT  [UserId]
                            , [FirstName]
                            , [LastName]
                            , [Email]
                            , [Gender]
                            , [Active]
                            FROM TutorialAppSchema.Users 
                            WHERE [UserId] = " + userId + ";";

            var user = _dapper.LoadDataSingle<User>(query);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new List<User> { user });
        }
    }
    ```
- This is ok, and works fine with postman, but Swagger UI shows `userId` as a required parameter. Could be because this is a route parameter, and perhaps this would be eliminated if it were a query param?
- One thing he did was cast the inbound int to a string with `userId.ToString()`.

### Adding and Editing records
- Add with `POST`, edit with `PUT`.
- Writing a new method with `[HttpPut]` attribute, returning `IActionResult`, which returns a response that tells you what happened without necessarily returning data. Just returning HTTP response code, with potentially an error message.
    ```
    [HttpPut]
    public IActionResult EditUser()
    {
        return Ok();
    }
    ```
- The `Ok()` is a built-in method which we **inherit** from the ControllerBase class.
- Written the SQL first, to map the change we want to make into a valid query with appropriate gaps for data to be passed through from the application.
- We can take a model as the parameter e.g. `public IActionResult EditUser(User user)`, or we can parse from the body of the request e.g. `public IActionResult EditUser([FromBody])`, We're going to set the parameter to be an instance of our `User` class, and map the results into the SQL query from there.
- Good idea to log the composed SQL line to check what this looks like while developing. Issue was `False` being passed through needed to be passed as a string. 

### DTOs (Data Transfer Objects)
- Going to do this with the `POST` endpoint. Resolving the situation where, when passing in a model as a parameter, it assumes you are going to pass in every parameter of the model (e.g. we don't know what the `UserId` is when creating a new `User`, as this is handled by the DB).
- New folder `Dtos`, make a copy of the `User.cs`, but rename the object and file to `UserDto`, and remove the `UserId` property. Then, update the `POST` request to instead take the `UserDto` model as the input parameter.
- **NB: hot reload can mess up and give funky error messages when making changes like this. Stop and restart if getting odd errors!**
- Add a more specific name when creating the DTO - e.g. `UserToAddDto`

### Namespaces
- So far, we've been tying everything together in the same namespace, except from Controllers - it's at a sub namespace `DotnetAPI.Controllers`. Everything else has been at the `DotNetAPI` namespace, so as soon as the app starts, all of the models are loaded into memory. 
- To remedy, we can create sub namespaces for each folder e.g. `DotnetAPI.Models`, `DotnetAPI.Dtos`, `DotnetAPI.Data` etc.
- Can go more granular than this too when needed e.g. `DotnetAPI.UserModels`. Really depends on the size of the application. 

### DELETE endpoint
- Pretty straight forward, creating a new method with `[HttpDelete]` attribute:
    ```
    [HttpDelete("{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string query = @"
            DELETE FROM TutorialAppSchema.Users 
                WHERE [UserId] = " + userId + ";";
         _logger.LogInformation("SQL to be executed on DB: "+ query);
        if (_dapper.ExecuteSql(query))
        {
            return Ok();
        }
        else
        {
            return NotFound();
            throw new Exception("Failed to delete user with Id: "+ userId);
        }
    }
    ```

### Entity Framework and Repository Pattern
- Add package `dotnet add package Microsoft.EntityFrameworkCore`
- Set up `DataContextEF`, inheriting from DbContext, and having a private field to store secrets like connection string, which is passed into the constructor when instanciating the class:
    ```
    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _config;

        public DataContextEF(IConfiguration config)
        {
            _config = config; 
        }
    }
    ```
- `DbSet` is used to map the table back to the model. `public virtual DbSet<User> Users { get; set; }`
- EF will default to look in `dbo` schema. Our objects are in a different schema. Configure EF to use a different schema with `modelBuilder.HasDefaultSchema("TutorialAppSchema");`
- We also need to add in some config to tell EF where to find our tables related to our objects, and provide key information too:
    ```
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("TutorialAppSchema");

        modelBuilder.Entity<User>() // Call Entity
            .ToTable("Users", "TutorialAppSchema") // Set the table and schema details
            .HasKey(u => u.UserId); // Provide the key 
        
        modelBuilder.Entity<UserJobInfo>() 
            .HasKey(u => u.UserId); // Provide the key

        modelBuilder.Entity<UserSalary>() 
            .HasKey(u => u.UserId); // Provide the key
    }
    ```
- Finally, we need to override the `OnConfiguring` method inherited from the DbContext class, to pass in the connection string from our `config` field. This requires `dotnet add package Microsoft.EntityFrameworkCore.SqlServer` to be added.
    ```
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                    optionsBuilder => optionsBuilder.EnableRetryOnFailure());
        }
    }
    ```
- Duplicated the `UserController.cs` as `UserEFController.cs`, going to use this to implement the same functionality as we did in Dapper, with EF.
- It's a lot less code to work with EF entites to retrieve them from the db... mainly because you don't have to write the SQL. His argument against EF is that EF is slower, and if we want to change the functionality, it's more work to get something working with EF compared to writing the SQL query. Apparently easier to join data and change/update multiple entites at the same time with Dapper too. If CRUD, EF is a good choice in his opinion... needs more research.
    ```
    [HttpGet]
    public ActionResult<IEnumerable<User>> GetUsers(int? userId = null)
    {
        if (userId == null)
        {
            _logger.LogInformation("Users endpoint processed a request at " + DateTime.Now + ". Getting all users from the db..");

            IEnumerable<User> users = _entityFramework.Users.ToList<User>();
            return Ok(users);
        }

        else
        {
            _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + userId + " was passed. Getting User Details from db..");

            var user = _entityFramework.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new List<User> { user });
        }
    }
    ```
- To get a user, you can also use `.Where()` - `User? userDb = _entityFramework.Users.Where(u => u.UserID == userId).FirstOrDefault<User>();`. `Find` only works on primary key columns, and checks the cache before going to the DB.
- To add an entity, use `_entityFramework.Users.Add()`. To commit the changes, you need to call `SaveChanges()`:
    ```
    if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
    ```
- When updating or deleting, need to get the user object from the db first, then ether update with the new values and call `Add()` then `SaveChanges()`, or call `_entityFramework.Users.Remove(user)`.

### Automapper
- Simplifying the `POST` using automapper.
- To add the mapper:
    - Add a new mapper field `IMapper _mapper;`
    - Add the mapper to the constructor
        ```
        _mapper = new Mapper(new MapperConfiguration(cfg =>{
            cfg.CreateMap<UserDto, User>();
        }));
        ```
- Then, in the method:
    ```
    User userDb = new User
        {
            Active = user.Active,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Gender = user.Gender
        };
    ```
    becomes `User userDb = _mapper.Map<User>(user);`

### Task - implement some more APIs for UserSalary and/or UserJobInfo

- I implemented GET, POST, PUT, and DELETE APIs for UserSalary, using both EF and Dapper. 
- Both approaches are good really. EF is fast to implement and saves you having to this across both C# and SQL domains so much, as EF is handling the query generation. However I can imagine in complex scenarios, being able to embed your own SQL and map this manually to your C# objects could be more efficient. I want to learn more about the complexities of setting up EF on projects e.g. using the `DbContextOptionsBuilder` used in `OnConfiguring` and `ModelBuilder` in `OnModelCreating` - feels like we skimmed over the complexity here, and because the data model was straight forward, consuming this in EF was easy.
- When using EF, use mappers when updating objects to compare inbound data to existing data and merge. 

## API Intermediate

- Repository flow - more common with EF projects. 
- Abstract the database access layer away from the user by another level. Anything we do with EF will be done from within the repository. Any time we create a new controller that uses EF, we would create a new repository for that controller too.
- Start by creating a `UserRepository.cs` in the `Data` folder, add it to the namespace, then create a `UserRepository` class where we will access EF and migrate all our EF logic to.
- Going to start by migrating all the `Add` patterns, i.e. all the `POST` requests. 
- Need our constructor from the controller, so we can inject EF into our repository.
    ```
    namespace DotnetAPI.Data
    {
        public class UserRepository
        {
            private readonly ILogger<UserRepository> _logger;

            DataContextEF _entityFramework;
            public UserRepository(ILogger<UserRepository> logger, IConfiguration config)
            {
                _logger = logger;
                _entityFramework = new DataContextEF(config);
            }
        }
    }
    ```
- We remove the mapper too - we can leave that in the controller.
- Adding a generic `SaveChanges()` method that uses the EF Data Context to commit changes to the db:
    ```
    public bool SaveChanges()
    {
        return _entityFramework.SaveChanges() > 0;
    }
    ```
- And adding a generic `AddEntity` method, with a generic Type T so we can add any model type using the method:
    ```
    public void AddEntity<T>(T entityToAdd)
    {
        if (entityToAdd != null)
        {
            _entityFramework.Add(entityToAdd);
        }
    }
    ```
- So far, with the deletes, we've been deleting a specific instance of an entity by fetching it from the db to validate it exists, then calling `Remove` against the entity e.g. `_entityFramework.UserSalary.Remove(userSalaryDb);`, passing in the object. But we don't have to do this - we can just call `Remove`:
    ```
    public void RemoveEntity<T>(T entityToRemove)
    {
        if (entityToRemove != null)
        {
            _entityFramework.Remove(entityToRemove);
        }
    }
    ```
- Seems that we cannot implement generics for `GET` and `POST` without some more infrastrucutre.. we're going to implement an interface.

### Interfaces
- We're going to use the interface to connect the Repository to the controller. New file in the `Data` folder, `IUserRepository.cs`, add the interface to the namespace, and add method signatures for the methods that any consuming class must implement. [Good video that explains interfaces as contracts + more detail here](https://www.youtube.com/watch?v=A7qwuFnyIpM). We'll then be able to access the methods by creating an instance of the interface, rather than of the class.
- One more step - update `Program.cs` and add a new `builder.Services` call to add a `Scoped` connection between our UserRepositry interface and class. `builder.Services.AddScoped<IUserRepository, UserRepository>();`
- With the above, we can now implement the method signatures declared in the `IUserRepository` interface in the `UserRepository` controller. In the controller, add a new field for the repository `IUserRepository _userRepository;`, and then add this to the constructor:
    ```
    public UserEFController(ILogger<UserEFController> logger,        IConfiguration config, **IUserRepository userRepository**)
    {
        _logger = logger;
        _entityFramework = new DataContextEF(config);
        **_userRepository = userRepository;**
    ...
    ```
- We can now access the methods implemented in the `UserRepository` class via the `_userRepository` interface, e.g. `_entityFramework.Users.Remove(userDb);` becomes `_userRepository.RemoveEntity(userDb);`
- Moving the `GET` and `PUT` requests
    - My code is different to his - I have some `AspNetCore.Mvc.ControllerBase` calls for HTTP status codes and such. The solution is to remove these from the `UserRepository` class, and implement this logic in the controller. 
    - I had some conditional logic that I was using to override behavior based on whether or not parameters were passed - I can still have this, but this logic lives in the controller, and I have separate methods for each of those operations in the repository, e.g.
        ```
        public IEnumerable<User> GetAllUsers()
        {
            _logger.LogInformation("Users endpoint processed a request at " + DateTime.Now + ". Getting all users from the db..");
            return _entityFramework.Users.ToList();
        }
        
        public User GetUserById(int userId)
        {
            _logger.LogInformation("User endpoint processed a request at " + DateTime.Now + ". User Id " + userId + " was passed. Getting User Details from db..");
            return _entityFramework.Users.Find(userId);
        }
        ```
- The flow is, get the EF calls out of the controller and into the Repository (i.e. `UserRepository.cs`), add the method signature to the interface (`IUserRepository.cs`), then refactor the original code in the controller to use the repository instead. 

### Authentication
- He's creating a user table to store email addresses, passwords hashes, and salts. 
    ```
    CREATE TABLE TutorialAppSchema.Auth (
        Email NVARCHAR(200),
        PasswordHash VARBINARY(MAX),
        PasswordSalt VARBINARY(MAX)
    )
    ```
- Going to create a controller to work with with this table.
- First, adding a section in `appsettings.json` called `AppSettings`, and adding a `PasswordKey` with a random string that we'll use along with the salt to hash the password.
- Adding some different DTO models:
    - [Login](.\DotnetAPI\Dtos\UserForLoginDto.cs)
    - [Login Confirmation](.\DotnetAPI\Dtos\UserForLoginConfirmationDto.cs)
        - Got `byte[]` datatype (byte arrays) for the `PasswordHash` and `PasswordSalt` values - equivalent to `VARBINARY` in the db.
    - [Registration](.\DotnetAPI\Dtos\UserForRegistrationDto.cs)
- Now adding an `AuthController.cs` 
    ```
    namespace DotnetAPI.Controllers
    {
        public class AuthController : ControllerBase
        {
            private readonly DataContextDapper _dapper;
            private readonly IConfiguration _config;
            public AuthController(IConfiguration config)
            {
                _dapper = new DataContextDapper(config);
                _config = config;
            }
    ...
    ```
    - Fields for `DataContextDapper` and `IConfiguration` that we pass into the constructor, as we did with the `UserController`
- Then creating two endpoints
    ```
    // Register user
    [HttpPost("register")]
    public IActionResult Register(UserForRegistrationDto userForRegistration)
    {
        return Ok();
    }

    // Login
    [HttpPost("login")]
    public IActionResult Login(UserForLoginDto userForLogin)
    {
        return Ok();
    }
    ```
    - Got an error trying to access these Dto models as they were not `public`. Same thing with the fields too. Need to make those public to be able to access them.
- Now implementing the Registration logic. 
    ```
    public IActionResult Register(UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                // Check if user exists
                string query = $@"SELECT [Email]
                        FROM TutorialAppSchema.Auth 
                        WHERE [Email] = '{ userForRegistration.Email }'";
                IEnumerable<string> existingUsers = _dapper.LoadData<string>(query);
                if (existingUsers.Count() == 0)
                {
                    byte[] passwordSalt = new byte[128/8]; // 128 bits (i.e. 16 bytes) in the byte array
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt); // Generating a random byte array with System.Security.Cryptography to salt the password
                    }
                    string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value 
                            + Convert.ToBase64String(passwordSalt); // Adding our known secret from the App Config settings

                    byte[] passwordHash = KeyDerivation.Pbkdf2(
                        password: userForRegistration.Password, 
                        salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                        prf: KeyDerivationPrf.HMACSHA256, // pseudo-random function
                        iterationCount: 100000,
                        numBytesRequested: 256/8)
                        ;
                    
                    // SQL query with parameters for the hash and salt
                    string sqlAddAuth = $@"INSERT INTO TutorialAppSchema.Auth
                    ([Email],
                    [PasswordHash],
                    [PasswordSalt]) VALUES
                    ('{ userForRegistration.Password }', @PasswordHash, @PasswordSalt);";

                    // Create list of SQL parameters to pass in with query
                    List<SqlParameter> sqlParameters = new List<SqlParameter>();
                    SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary)
                    {
                        Value = passwordHash // User the constructor to pass in the value
                    };
                    SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary)
                    {
                        Value = passwordSaltPlusString
                    };
                    // sqlParameters.Add(passwordHashParameter);
                    // sqlParameters.Add(passwordSaltParameter);
                    sqlParameters.AddRange(new[] { passwordHashParameter, passwordSaltParameter });

                    if (_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
                    {
                        return Ok();
                    }
                    throw new Exception ("Failed to register user");
                } 
                throw new Exception ("User with this Email already exists");
            }
            throw new Exception("Passwords do not match");
        }
    ```
- Now implementing the `login` functionality to test the password hash and password salt to make sure the password can be verified. 
- Take the e-mail and get the password hash and salt from the database, add the App Setting to the salt, and then compare the values
    ```
            // Login
        [HttpPost("login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
           // Get passwordhash and passwordsalt from db
           string sqlForHashAndSalt = $@"SELECT [PasswordHash], [PasswordSalt]
                FROM TutorialAppSchema.Auth
                WHERE [Email] == '{ userForLogin.Email }'";
            
            UserForLoginConfirmationDto userForLoginConfirmation = _dapper
                .LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt);
           
            byte[] passwordHash = GetPasswordHash(userForLogin.Password, userForLoginConfirmation.PasswordSalt);

            for (int index = 0; index < passwordHash.Length; index++)
            {
                if (passwordHash[index] != userForLoginConfirmation.PasswordHash[index])
                {
                    return StatusCode(401, "Incorrect password");
                }
            }
            return Ok();
        }
    ```
- // if (passwordHash == userForLoginConfirmation.PasswordHash) 
    - Won't work - can't compare byte arrays like this, because arrays are reference types, and the `==` operator compares the references, not the contents of the arrays. If you use `==`, it will return `true` only if both arrays are the exact same instance (i.e., they both point to the same location in memory).
- We also moved the code to hash a password into a `private` method `GetPasswordHash` so we can re-use it across login and registation
    ```
    private byte[] GetPasswordHash(string password, byte[] passwordSalt)
        {
            // Generate passwordsalt with AppSetting and database stored salt
            string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value 
                            + Convert.ToBase64String(passwordSalt);

            // hash the password
            return KeyDerivation.Pbkdf2(
                        password: password, 
                        salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 100000,
                        numBytesRequested: 256/8);
        }
    ```
- Enforcing HTTPS means credentials are encrypted, and we can use HTTP Basic Auth (encode u:p in base64, and POST in the Authentication header of the request)

### Adding users to our users table on registration
- We edited the [Registration Dto](.\DotnetAPI\Dtos\UserForRegistrationDto.cs) to add in name and gender fields, then added a SQL query to write these details to the user table on user registration:
    ```
    if (_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
    {
        string sqlAddUserFromRegistration = @"
            INSERT INTO TutorialAppSchema.Users (
                    [FirstName]
                    , [LastName]
                    , [Email]
                    , [Gender]
                    , [Active])
                VALUES
                    (
                        '" + userForRegistration.FirstName + @"',
                        '" + userForRegistration.LastName + @"',
                        '" + userForRegistration.Email + @"',
                        '" + userForRegistration.Gender + @"',
                        1
                    )  
                    ;";
        if (_dapper.ExecuteSql(sqlAddUserFromRegistration))
        {
            return Ok();
        }
        throw new Exception ("Failed to add user");
    ```
- We ideally would need to wrap these writes in a transaction to rollback the registration query if the user write fails for some reason.

### JWT

#### Creating JWT

- Going to create a JWT JavaScript Web Token inside our auth controller to Authenticate requests and identify our user after a successful login to our API. 
- Instead of just returning `Ok()`, we'll also return the JWT that can be sent along with the request so that the user doesn't need to keep authenticating themselves. Further, we will be able to identify them in other controllers based on that token to check what functionality a user has access to. Can of course be used in the front end to manage UX.
- First, create another `private` helper method, to return a string for the token we are going to pass back. 
- Going to add a `TokenKey` that will be used to sign the tokens we generate (similar to the `PasswordKey` that we appended to the salt when hashing passwords)
    - in `appsettings.json`, add a `TokenKey` to the `AppSettings`.
- Need to define some claims that will be attached to the token so we can identify the user and their permissions
    ```
    Claim[] claims = new Claim[]{
        Claim("userId", userId.ToString())
    }
    ```
    Building an array of claims, a claim needs to have a key and a value.
- Then need to generate the Symmetric key that will be added to the JWT (so our `TokenKey` is our private key).
- We are using `Microsoft.IdentityModel.Tokens` (y), and passing the Symmetric key a byte array of our token key.
    ```
    string? tokenKeyString = _config.GetSection("AppSettings:Token").Value;
    
    SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                tokenKeyString != null ? tokenKeyString : "" // handling the potential null returned from _config.GetSection()
            )
        );
    ```
    > The warning comes up because IConfiguration (which "_config" is an instance of) may return a null value in .NET 7 when calling GetSection(). We address that here by assigning it to a nullable string, and then checking if that string is null before we decide whether or not to use it. If the conditional statement "tokenKeyString != null" is true, and tokenKeyString is not null, we take the value between the "?" and the ":", tokenKeyString, if the condition is false, we take the value after the ":", an empty string, as an else value.
- We then create a `SigningCredential` with the `tokenKey` and `SecurityAlgorithms.HmacSha512Signature` to sign the security token when we create it
    ```
    SigningCredentials credentials = new SigningCredentials(tokenKey, 
                SecurityAlgorithms.HmacSha512Signature);
    ```
- We describe the properties of token we want to create by creating a `SecurityTokenDescriptor`, instanciate a `JwtSecurityTokenHandler`, and then create the JWT by passing the handler our descriptor `SecurityToken token = jwtSecurityTokenHandler.CreateToken(descriptor);`
- Finally, to return this to the client, we need to turn the `token` into a string
    ```
    return jwtSecurityTokenHandler.WriteToken(token);
    ```
- Hit some oddities passing the Email back to SQL via Dapper - ended up implementing a new method in the DataContext `LoadDataSingleWithParams`:
    ```
    public T LoadDataSingleWithParams<T>(string sql, object parameters)
        {
            using (IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                return dbConnection.QuerySingle<T>(sql, parameters);
            }
        }
    ```
    - This allows me to pass in a parameter for the e-mail instead of accepting user input into the database, protecting us from SQL injection attacks.

#### Token Validation

- Refresh token logic, and a new endpoint to take an old token and issue a new one.
- Also going to see how we can access claims from within the token e.g. `userId`.
- First, need to modify our application to accept the token as a form of authentication. Need to add details into `Program.cs`
    - Add `app.UseAuthentication();` **NB: make sure this goes BEFORE app.UserAuthorization();**, then add to the buider - will need to add a new package `dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer -v 7.0.14` - this project is on dotnet 7 at the moment so need to specify a compatible version number as it defaults to version 8. Find the most recent compatible version by searching for the package in [`nuget`](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer)
    ```
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    builder.Configuration.GetSection("AppSettings:TokenKey").Value
                    )),
                ValidateIssuer = false,
                ValidateAudience = false
            };
    });
    ```
    - We add the JwtBearer Authentication scheme to the builder, adding authentication services to the application's dependency injection container, and pass in a the `TokenValidationParameters` options. We are passing the same signing key to validate the token's signature that we used to create the signature. We are not validaiting the issuer or audience yet.
- Now we can change our `AuthController` to accept the token from our user for authentication and and to identify that user.
- Putting `[Authorize]` decorator on the controller - we can get around this for login and registeration by adding `[AllowAnonymous]`
- Writing a new function `RefreshToken()`, which checks if userId  tied to the token is valid, and if so, uses that id to create a new token and return to user.
    ```
    [HttpGet("refreshtoken")]
    public string RefreshToken()
    {
        // see if user id tied to the token is valid, and if so, use that id to create a new token and return to user
        string sqlGetUserId = $@"SELECT UserId
            FROM TutorialAppSchema.Users
            WHERE [UserId] = @UserId";

        int userId = _dapper.LoadDataSingleWithParams<int>(sqlGetUserId, new Dictionary<string, string>{{ "UserId",  User.FindFirstValue("userId") }});
        
        return CreateToken(userId);
    }
    ```
    - I don't like this at all - we're not validating the issuer of the token so if someone were to send in a token with a valid user id, we would then return them a valid token for our application!
    - To make this secure, we'd need to validate the token being sent for refresh has been signed by our app. We could also implement some kind of grace period whereby we accept tokens for refresh within a certain time window, else we force a new login to validate the authentication.
- Need to also remember to register our controller with the decorators `[ApiController]` and `[Route("[controller]")]` - without this, Swagger won't pull through example documents from the inbound objects e.g. Dtos for the endpoints. Doing that as put those endpoints behind the `Auth` route.

#### Helper classes
- At the moment we have some noise in our `AuthController` e.g. `GetPasswordHash` and `CreateToken` - we can create helper classes to remove the methods that don't need to be in the controller into a more suitable place (because they are not controller endpoints).
- New folder in the project `Helpers`, created a new namespace `DotnetAPI.Helpers`, and added our two methods to it as public methods.
- Created a constructor for the class, passing in our config (as before), and storing this in a `private readonly` field with an `_` before for the naming convention.
- Then we update the code in the controller:
    - add the `_authHelper` to the constructor with another `private readonly` field, and replace the old calls to the local method with `_authHelper.GetPasswordHash` and `_authHelper.CreateToken`

### Adding Posts
- Going to create a peripheral model that we can use to create some data attached to our user.
- New table, with CCI on (UserId, PostId) to colocate User Posts on disk (as we're mostly going to be querying posts by user)
    ```
    CREATE TABLE TutorialAppSchema.Posts (
    PostId int IDENTITY(1,1),
    UserId int,
    PostTitle NVARCHAR(255),
    PostContent NVARCHAR(MAX),
    PostCreated DATETIME2,
    PostUpdated DATETIME2
    )
    GO

    CREATE CLUSTERED INDEX cix_Posts_UserId_PostId ON TutorialAppSchema.Posts(UserId, PostId)
    GO
    ```
- New model `Posts.cs`
    ```
    namespace DotnetAPI.Models
    {
        public partial class Posts
        {
            public int PostId { get; set; }
            public int UserId { get; set; }
            public string PostTitle { get; set; } = "";
            public string PostContent { get; set; } = "";
            public DateTime PostCreated { get; set; }
            public DateTime ?PostUpdated { get; set; }
        }
    }
    ```
- And [`PostsController.cs`](.\DotnetAPI\Controllers\PostController.cs)





- Refactoring code
- Relating data to the user (e.g. user and posts)

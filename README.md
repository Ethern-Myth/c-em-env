# C-EM-ENV

The `c-em-env` package simplifies dynamic variable access in C# applications, particularly for retrieving values from environment variables and configuration settings. It offers convenient functionality to handle `.env` files commonly used for configuration management.

![NuGet Version](https://img.shields.io/nuget/v/c-em-env)

![GitHub Tag](https://img.shields.io/github/v/tag/ethern-myth/c-em-env)

![NuGet Downloads](https://img.shields.io/nuget/dt/c-em-env)


### Explanation

`Variable`, represents a dynamic variable that retrieves values from environment variables and configuration settings. It offers dynamic access to these values through the `AsDynamic()` method and dynamic property access.

#### Installation

1. **Add Package Reference**:

   Ensure you have the required packages installed. Add the following package references to your project:

   ```bash
   dotnet add package c-em-env
   ```

2. **Usage**:

   Register the `Variable` class in your application's service collection during startup:

   ```csharp
   // Use the EnvFileReader to load the .env file at the beginning of the Program.cs or Startup.cs
   // NOTE: This step is helpful if .env variables are not loaded from the constructor automatically on program startup
   EnvFileReader.Read();
   EnvFileReader.Load();

   // Add the Variable class as a singleton service
   builder.Services.AddSingleton<IVariable, Variable>();

   // Initialize the Variable class with the application's configuration
   new Variable(builder.Configuration);
   ```

#### Constructor

```csharp
/// Initializes a new instance of the <see cref="Variable"/> class.
/// <param name="config">The configuration.</param>
public Variable(IConfiguration config)
```

- `config`: An instance of `IConfiguration` representing the application's configuration settings.

#### Methods

1. `AsDynamic()`

```csharp
/// Converts the variable to a dynamic object allowing dynamic access to environment variables and configuration settings.
/// <returns>A dynamic object representing the variable.</returns>
public dynamic AsDynamic()
```

- Returns a dynamic object representing the environment variables and configuration settings.

2. `GetDynamicValue(string key)`

```csharp
/// Gets the value of a dynamic property identified by the specified key.
/// <param name="key">The key of the dynamic property.</param>
/// <returns>The value of the dynamic property.</returns>
public string? GetDynamicValue(string key)
```

- `key`: The key of the dynamic property.
- Returns the value of the dynamic property identified by the specified key.

#### Properties

- `this[string key]`

```csharp
/// Gets or sets the value of a dynamic property identified by the specified key.
/// <param name="key">The key of the dynamic property.</param>
/// <returns>The value of the dynamic property.</returns>
public dynamic this[string key]
```

- `key`: The key of the dynamic property.
- Provides access to the value of a dynamic property identified by the specified key.

### Usage

```csharp
// Example usage of Variable class
var variable = new Variable(configuration);
dynamic dynamicVariable = variable.AsDynamic();

// Accessing dynamic properties
string value = dynamicVariable.SomeKey;

// Getting dynamic property value
string value = variable.GetDynamicValue("SomeKey");
```

### Notes

- This class retrieves values from environment variables and configuration settings.
- It provides dynamic access to these values using C#'s dynamic feature.
- Error handling is included for cases where keys are null, whitespace, or not found in the configuration settings.

#### Features

- Retrieve values from environment variables and configuration settings dynamically.
- Supports dynamic property access for easy retrieval of values.

#### Important Note

- **`.env` File Required**: Ensure you have an `.env` file present in your project directory with the necessary environment variables. The `Variable` class retrieves values from environment variables and requires an `.env` file to function correctly.

This guide assumes you're using .NET Core or .NET 5+ for your project, although this was created on .NET 8. Make sure to replace `builder` with your appropriate service provider registration mechanism if you're using a different framework or dependency injection container.


### Author and Creator

[Ethern Myth](http://www.github.com/ethern-myth)

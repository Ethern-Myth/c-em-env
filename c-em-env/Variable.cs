using System.Dynamic;
using Microsoft.Extensions.Configuration;

namespace c_em_env
{
    /// <summary>
    /// Represents a dynamic variable that retrieves values from environment variables and configuration settings.
    /// </summary>
    public class Variable : IVariable
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="Variable"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public Variable(IConfiguration config)
        {
            EnvFileReader.Read();
            EnvFileReader.Load();
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Converts the variable to a dynamic object allowing dynamic access to environment variables and configuration settings.
        /// </summary>
        /// <returns>A dynamic object representing the variable.</returns>
        public dynamic AsDynamic()
        {
            dynamic dynamicObject = new ExpandoObject();
            // Cast dynamicObject to IDictionary<string, object> to access its properties
            var dictionary = (IDictionary<string, object>)dynamicObject;

            // Populate the dynamic object with environment variables
            foreach (var key in Environment.GetEnvironmentVariables().Keys)
            {
                var keyString = key.ToString();
                var value = Environment.GetEnvironmentVariable(keyString!);
                if (value != null)
                {
                    // Add key-value pair to dynamicObject
                    dictionary[keyString!] = value;
                }
            }
            return dynamicObject;
        }

        /// <summary>
        /// Gets the value of a dynamic property identified by the specified key.
        /// </summary>
        /// <param name="key">The key of the dynamic property.</param>
        /// <returns>The value of the dynamic property.</returns>
        public string? GetDynamicValue(string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    Console.WriteLine("Error: Key cannot be null or whitespace.");
                    return null;
                }

                string value = _config[key]!;

                if (string.IsNullOrEmpty(value))
                {
                    Console.WriteLine($"Warning: Value for key '{key}' not found in the configuration.");
                    return null;
                }

                return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: An error occurred while retrieving value for key '{key}': {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the value of a dynamic property identified by the specified key.
        /// </summary>
        /// <param name="key">The key of the dynamic property.</param>
        /// <returns>The value of the dynamic property.</returns>
        public dynamic this[string key]
        {
            get
            {
                dynamic dynamicObject = AsDynamic();
                var dictionary = dynamicObject as IDictionary<string, object>;

                if (dictionary != null && dictionary.TryGetValue(key, out var value))
                {
                    return value;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}

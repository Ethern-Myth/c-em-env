using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace c_em_env
{
    /// <summary>
    /// Provides functionality to read environment variables from .env files.
    /// </summary>
    public static class EnvFileReader
    {
        private static readonly IDictionary<string, string> _envVariables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// Loads environment variables from .env files in the current directory.
        /// </summary>
        /// <returns>A dictionary containing the loaded environment variables.</returns>
        public static IDictionary<string, string> Read()
        {
            var projectDirectory = GetProjectDirectory();
            if (projectDirectory == null)
            {
                Console.WriteLine("Project directory not found.");
                return new Dictionary<string, string>();
            }

            var filePaths = Directory.GetFiles(projectDirectory!, "*.env*", SearchOption.AllDirectories)
                                      .Where(file => Path.GetFileName(file).StartsWith('.'));

            // Iterate through each .env file
            foreach (var filePath in filePaths)
            {
                // Read each line in the file
                foreach (var line in File.ReadLines(filePath))
                {
                    // Skip empty lines and comments (lines starting with '#')
                    if (!string.IsNullOrWhiteSpace(line) && !line.TrimStart().StartsWith('#'))
                    {
                        // Split the line into key and value pairs based on '=' delimiter
                        var parts = line.Split(['='], 2);

                        // Ensure that the line is properly formatted
                        if (parts.Length == 2)
                        {
                            var key = parts[0].Trim();
                            var value = parts[1].Trim();
                            _envVariables[key] = value;
                        }
                        else
                        { 
                            // Handle invalid lines (e.g., missing '=' delimiter)
                            Console.WriteLine($"Invalid line in .env file: {line}");
                        }
                    }
                }
            }
            // Return the dictionary containing the loaded environment variables
            return _envVariables;
        }

        /// <summary>
        /// Adds the loaded environment variables to the current process's environment variables.
        /// </summary>
        public static void Load()
        {
            foreach (var kvp in Read())
            {
                Environment.SetEnvironmentVariable(kvp.Key, kvp.Value);
            }
        }

        private static string? GetProjectDirectory()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Navigate to the project root directory
            string projectRootDirectory = baseDirectory;
            while (!Directory.GetFiles(projectRootDirectory, "*.csproj").Any() && Directory.GetParent(projectRootDirectory) != null)
            {
                projectRootDirectory = Directory.GetParent(projectRootDirectory)!.FullName;
            }

            if (projectRootDirectory == null)
                return null;

            return Path.GetDirectoryName(projectRootDirectory)!;
        }
    }
}

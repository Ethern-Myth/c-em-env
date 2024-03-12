namespace c_em_env_ntest
{
    internal class TestDotNetEnv
    {
        [Test]
        public void DotNetEnv_Load_Should_Load_Environment_Variables_From_Env_File()
        {
            // Arrange
            const string envFilePath = ".env";
            const string testKey = "TEST_KEY";
            const string testValue = "TestValue";

            // Create or overwrite the .env file with the test environment variable
            using (var writer = new StreamWriter(envFilePath))
            {
                writer.WriteLine($"{testKey}={testValue}");
            }

            // Act
            DotNetEnv.Env.Load();

            // Assert
            var loadedValue = Environment.GetEnvironmentVariable(testKey);
            Assert.AreEqual(testValue, loadedValue);

            // Clean up: remove the .env file
            File.Delete(envFilePath);
        }
    }
}

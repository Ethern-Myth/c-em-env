using c_em_env;

namespace c_em_env_ntest
{
    [TestFixture]
    internal class EnvFileReaderTest
    {
        [Test]
        public void Read_WithValidEnvFile_ShouldReadVariables()
        {
            // Arrange
            string filePath = "test.env";
            CreateEnvFile(filePath);

            // Act
            var envVariables = EnvFileReader.Read();

            // Assert
            Assert.That(envVariables.Count, Is.EqualTo(2));
            Assert.That(envVariables["TEST_KEY1"], Is.EqualTo("Value1"));
            Assert.That(envVariables["TEST_KEY2"], Is.EqualTo("Value2"));

            // Clean up
            DeleteEnvFile(filePath);
        }

        [Test]
        public void Read_WithInvalidEnvFile_ShouldNotReadVariables()
        {
            // Arrange
            string filePath = "invalid.env";
            CreateInvalidEnvFile(filePath);

            // Act
            var envVariables = EnvFileReader.Read();

            // Assert
            Assert.That(envVariables.Count, Is.EqualTo(0));

            // Clean up
            DeleteEnvFile(filePath);
        }

        [Test]
        public void Read_WithNoEnvFile_ShouldReturnEmptyDictionary()
        {
            // Arrange

            // Act
            var envVariables = EnvFileReader.Read();

            // Assert
            Assert.That(envVariables.Count, Is.EqualTo(0));
        }

        private void CreateEnvFile(string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("TEST_KEY1=Value1");
                writer.WriteLine("TEST_KEY2=Value2");
            }
        }

        private void CreateInvalidEnvFile(string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("InvalidLine");
            }
        }

        private void DeleteEnvFile(string filePath)
        {
            File.Delete(filePath);
        }

        [Test]
        public void Load_ShouldSetEnvironmentVariables()
        {
            // Arrange
            var envVariables = new Dictionary<string, string>
            {
                { "TEST_KEY1", "Value1" },
                { "TEST_KEY2", "Value2" }
            };

            // Mock the IEnvFileReader interface to provide the desired behavior for testing
            var mockEnvFileReader = new MockEnvFileReader(envVariables);

            // Act
            mockEnvFileReader.Load();

            // Assert
            foreach (var kvp in envVariables)
            {
                Assert.That(Environment.GetEnvironmentVariable(kvp.Key), Is.EqualTo(kvp.Value));
            }
        }
    }

    public interface IEnvFileReader
    {
        IEnumerable<KeyValuePair<string, string>> Read();
        void Load();
    }

    public class MockEnvFileReader : IEnvFileReader
    {
        private readonly Dictionary<string, string> _envVariables;

        public MockEnvFileReader(Dictionary<string, string> envVariables)
        {
            _envVariables = envVariables;
        }

        public IEnumerable<KeyValuePair<string, string>> Read()
        {
            // Implement Read method to return mock environment variables
            return _envVariables;
        }

        public void Load()
        {
            foreach (var kvp in Read())
            {
                Environment.SetEnvironmentVariable(kvp.Key, kvp.Value);
            }
        }
    }
}

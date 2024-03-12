using c_em_env;
using Microsoft.Extensions.Configuration;

namespace c_em_env_ntest
{
    [TestFixture]
    public class VariableTests
    {

        [SetUp]
        public void Setup()
        {
            // Load values from .env file into environment variables
            //EnvFileReader.Load();
        }

        private IConfiguration GetMockConfiguration(Dictionary<string, string> configValues)
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(configValues);
            return configBuilder.Build();
        }

        [Test]
        public void GetDynamicValue_KeyExists_ReturnsValue()
        {
            // Arrange
            var configValues = new Dictionary<string, string>
            {
                {"Key1", "Value1"},
                {"Key2", "Value2"}
            };
            var configuration = GetMockConfiguration(configValues);

            IVariable variable = new Variable(configuration);

            // Act
            var value = variable.GetDynamicValue("Key1");

            // Assert
            Assert.AreEqual("Value1", value);
        }

        [Test]
        public void GetDynamicValue_KeyDoesNotExist_ReturnsNull()
        {
            // Arrange
            var configValues = new Dictionary<string, string>
            {
                {"Key1", "Value1"},
                {"Key2", "Value2"}
            };
            var configuration = GetMockConfiguration(configValues);

            IVariable variable = new Variable(configuration);

            // Act
            var value = variable.GetDynamicValue("NonExistentKey");

            // Assert
            Assert.IsNull(value);
        }

        [Test]
        public void Indexer_KeyExists_ReturnsValue()
        {
            // Arrange
            var configValues = new Dictionary<string, string>
            {
                {"Key1", "Value1"},
                {"Key2", "Value2"}
            };
            var configuration = GetMockConfiguration(configValues);

            IVariable variable = new Variable(configuration);

            // Act
            dynamic dynamicVariable = variable.AsDynamic();
            var value = dynamicVariable.TEST_KEY_TEST;

            // Assert
            Assert.AreEqual("value1", value);
        }

        [Test]
        public void AsDynamic_ReturnsDynamicObjectWithCorrectValues()
        {
            // Arrange
            var configValues = new Dictionary<string, string>
    {
        {"Key1", "Value1"},
        {"Key2", "Value2"}
    };
            var configuration = GetMockConfiguration(configValues);

            IVariable variable = new Variable(configuration);

            // Act
            dynamic dynamicVariable = variable.AsDynamic();

            // Assert
            Assert.IsNotNull(dynamicVariable);
            Assert.IsTrue(dynamicVariable.TEST_KEY_TEST == "value1");
            Assert.IsTrue(dynamicVariable.TEST_KEY_DEV == "value2");
        }
    }
}
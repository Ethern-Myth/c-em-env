using c_em_env;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_em_env_ntest
{
    internal class AsDynamicTest
    {
      
        [Test]
        public void AsDynamic_Returns_Dynamic_Object_With_Environment_Variables()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            IVariable variable = new Variable(mockConfig.Object);

            // Act
            dynamic dynamicObject = variable.AsDynamic();

            // Assert
            Assert.AreEqual("value1", dynamicObject.TEST_KEY_TEST);
            Assert.AreEqual("value2", dynamicObject.TEST_KEY_DEV);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TentCreator;
using TentCreator.Enumerations;


namespace UnitTests
{
    [TestClass]
    public class TestTentCreator
    {
        [TestMethod]
        public void CheckParametrConstructor()
        {
            var parData = new ParameterData("name", 100);

            Assert.AreEqual(parData.Name, @"name");
            Assert.AreEqual(parData.Value, 100);
        }

        [TestMethod]
        public void CheckParametrConstructorThree()
        {
            var parData = new ParameterData("name", "adyn", 150);

            Assert.AreEqual(parData.Name, @"name");
            Assert.AreEqual(parData.Description, @"adyn");
            Assert.AreEqual(parData.Value, 150);
        }

        [TestMethod]
        public void CheckNormalValueProperty()
        {
            var sketch = new KompasSketch {NormalValue = 91};
            
            Assert.AreEqual(91, sketch.NormalValue);
        }

        [TestMethod]
        public void CheckValidParametrs()
        {
            var parametrData = new ModelParameters();
            var parametrs = new Dictionary<Parameter, ParameterData>
            {
                {Parameter.TentWidth, new ParameterData(Parameter.TentWidth.ToString(), 40, new System.Drawing.PointF(40, 100))},
            };
            List<string> error = parametrData.CheckData(parametrs);
            if (error.Count != 0)
            {
                Assert.Fail();
            }
        }


    }
}



using InterviewAssessment.Infrastructure;
using InterviewAssessment.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Dynamic;
using System.Windows.Media;

namespace InterviewAssessmentTests
{
    [TestClass()]
    public class EntityStoreServiceTests
    {

        private EntityStoreService _entityStore;
        private Mock<IRepository<ExpandoObject>> _repo;

        [TestInitialize()]
        public void Init()
        {
            //Arrange
            _repo = new Mock<IRepository<ExpandoObject>>();
            dynamic entityDynamicOne = new ExpandoObject();
            entityDynamicOne.id = 3;
            entityDynamicOne.name = "Person";
            entityDynamicOne.x = 150.0;
            entityDynamicOne.y = 120.0;

            dynamic entityDynamicTwo = new ExpandoObject();
            entityDynamicTwo.id = 4;
            entityDynamicTwo.name = "Employee";
            entityDynamicTwo.x = 20.0;
            entityDynamicTwo.y = 400.0;

            dynamic entityDynamicReturnAdd = new ExpandoObject();
            entityDynamicReturnAdd.id = 1;
            entityDynamicReturnAdd.name = "Shape";
            entityDynamicReturnAdd.x = 111.0;
            entityDynamicReturnAdd.y = 222.0;

            _repo.Setup(x => x.All()).Returns(
                new List<ExpandoObject>() { entityDynamicOne, entityDynamicTwo });
            _repo.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()))
                .Returns((ExpandoObject)entityDynamicReturnAdd);
            _entityStore = new EntityStoreService(_repo.Object);
        }

        [TestMethod()]
        public void LoadTest()
        {
            //Act
            _entityStore.Load();

            //Assert
            Assert.AreEqual(2, _entityStore.Count);
            dynamic resultZero = _entityStore[0];
            dynamic resultOne = _entityStore[1];
            Assert.AreEqual(3, resultZero.id);
            Assert.AreEqual(4, resultOne.id);
            Assert.AreEqual("Employee", resultOne.name);
            Assert.AreEqual(20, resultOne.x);
            Assert.AreEqual(400, resultOne.y);
        }

        [TestMethod()]
        public void AddTest()
        {
            //Act
            _entityStore.Load();
            //value not important
            _entityStore.AddEntity("NotImportant", 0, 0);

            //Assert
            dynamic resultZero = _entityStore[0];
            dynamic resultTwo = _entityStore[2];
            Assert.AreEqual(3, _entityStore.Count);
            Assert.AreEqual(3, resultZero.id);
            Assert.AreEqual("Shape", resultTwo.name);
            Assert.AreEqual(111.0, resultTwo.x);
            Assert.AreEqual(222.0, resultTwo.y);
        }

        [TestMethod()]
        public void UpdateCoordinateTest()
        {

            //Act
            _entityStore.Load();
            //value not important
            _entityStore.AddEntity("NotImportant", 0.0, 0.0);

            //Assert
            dynamic resultZero = _entityStore[0];
            dynamic resultTwo = _entityStore[2];


            double offsetX = -20.0;
            double offsetY = 10.0;
            //Retrieve the id
            _entityStore.UpdateCoordinate(resultTwo.id, 111.0, 222.0, new TranslateTransform(offsetX, offsetY));

            //Assert
            Assert.AreEqual(3, _entityStore.Count);
            Assert.AreEqual(3, resultZero.id);
            Assert.AreEqual("Shape", resultTwo.name);


            // Verify Method was called once only
            _repo.Verify(c => c.UpdateCoordinate(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once());

            //offsetX and offsetY has to be inverted x to y y to x
            _repo.Verify(a => a.UpdateCoordinate( //offsetX and offsetY has to be inverted x to y y to x
            It.Is<int>(p => p == 1), It.Is<double>(p => p == 111.0 + offsetY), It.Is<double>(p => p == 222.0 + offsetX)));
        }
    }
}
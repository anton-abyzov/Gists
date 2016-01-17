using System;
using AutoMapper;
using NUnit.Framework;

namespace AutoMapperTests
{
    /// <summary>
    /// Automapper will ignore lacking or excessed props when doing mapping
    /// </summary>
    public class AutoMapperTests
    {
        /// <summary>
        /// Automapper will ignore outer props of destination entity as it has more props than the source one. Props that are not matched will be
        /// populated with default values
        /// </summary>
        [Test]
        public void OuterPropsShouldNotAbortMapping()
        {
            //arrange
            const string helloWorld = "Hello world";
            Mapper.CreateMap<SmallEntity, BigEntity>();
            var smallEntity = new SmallEntity {Name = helloWorld};

            //act
            var result = smallEntity.Map<SmallEntity, BigEntity>();

            //assert
            Assert.AreEqual(result.Name, helloWorld);
            Assert.AreEqual(result.ExternalEntity, null);
            Assert.AreEqual(result.IntField, 0);
        }

        /// <summary>
        /// Automapper will populate only those props that do match with source object
        /// </summary>
        [Test]
        public void MissingPropsInDestObjectShouldNotAbortMapping()
        {
            //arrange
            const string helloWorld = "Hello world";
            Mapper.CreateMap<BigEntity, SmallEntity>();
            var bigEntity = new BigEntity { Name = helloWorld, IntField = 13, ExternalEntity = new object()};

            //act
            var result = bigEntity.Map<BigEntity, SmallEntity>();

            //assert
            Assert.AreEqual(result.Name, helloWorld);
        }

        public class SmallEntity
        {
            public string Name { get; set; }
        }

        public class BigEntity
        {
            public string Name { get; set; }
            public object ExternalEntity { get; set; }
            public int IntField  { get; set; }
        }
    }
}
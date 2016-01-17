using System;
using NUnit.Framework;
using StructureMap;

namespace StructureMapTests
{
    public class SMTests
    {
        [Test]
        public void WillUseLazyInit()
        {
            //arrange
            ObjectFactory.Initialize(x => { x.For<IDependency1>().Use<Dependency1>(); });

            //act
            var main = ObjectFactory.GetInstance<Main>();
            main.Write();

            //assert: the order of messages in Console should be
            //Write method is called
            //Dependency created
            //Dependency property Name is called
        }
    }

    public interface IDependency1
    {
        string Name { get; }
    }

    public class Dependency1 : IDependency1
    {
        public Dependency1()
        {
            Console.WriteLine("Dependency created");
        }

        public string Name
        {
            get { return "Dependency property Name is called"; }
        }
    }

    public class Main
    {
        private readonly Func<IDependency1> _dep;

        public Main(Func<IDependency1> dep)
        {
            _dep = dep;
        }

        public void Write()
        {
            Console.WriteLine("Write method is called");
            Console.WriteLine(_dep().Name);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Should;

namespace AutoMapperTests
{
    public class LinqTests
    {
        [Test]
        public void WillOuterJoinTwoSets()
        {
            //arrange
            var data1 = new List<int>() {3, 13, 7};
            var data2 = new List<int>() {8, 7, 13, 24};

            //act
            //var join = data1.Join(data2, x => x, y => y, (x, y) => x);
            var join = data1.GroupJoin(data2, x => x, y => y, (x, y) => new {Foo = x, Bars = y});
                //.SelectMany(x => x.Bars.DefaultIfEmpty());
            var j1 = data1.SelectMany(x => data2.Where(y => y == x).DefaultIfEmpty(), (x, y) => y);
            //data1.LeftOuterJoin()

            //assert
            j1.Count().ShouldEqual(5);
        }
    }
}
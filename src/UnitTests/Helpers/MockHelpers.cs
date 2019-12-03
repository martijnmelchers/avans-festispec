using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Festispec.UnitTests.Helpers
{
    public class MockHelpers
    {
        public static Mock<DbSet<T>> CreateDbSetMock<T>(List<T> elements) where T : class
        {
            var elementsAsQueryable = elements.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());

            dbSetMock.Setup(m => m.Include(It.IsAny<string>())).Returns(dbSetMock.Object);
            dbSetMock.Setup(m => m.Add(It.IsAny<T>())).Returns((T a) => { elements.Add(a); return a; });
            dbSetMock.Setup(m => m.AddRange(It.IsAny<IEnumerable<T>>())).Returns((IEnumerable<T> a) => { foreach (var item in a.ToArray()) elements.Add(item); return a; });
            dbSetMock.Setup(m => m.Remove(It.IsAny<T>())).Returns((T a) => { elements.Remove(a); return a; });
            dbSetMock.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<T>>())).Returns((IEnumerable<T> a) => { foreach (var item in a.ToArray()) elements.Remove(item); return a; });

            return dbSetMock;
        }
    }
}

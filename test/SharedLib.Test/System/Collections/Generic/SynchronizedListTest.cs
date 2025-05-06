using System.Collections;
using System.Diagnostics;

namespace SharedLib.Test.System.Collections.Generic
{
    public class SynchronizedListTest
    {
        [Fact]
        public void Ctor_ArgIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new SynchronizedList<string>(null));

            Assert.Throws<ArgumentNullException>(
                () => {
                    IEnumerable<string> list = new List<string>();
                    new SynchronizedList<string>(null, list);
                });

            Assert.Throws<ArgumentNullException>(
                () => {
                    IEnumerable<string> list = null;
                    new SynchronizedList<string>(new object(), list);
                });

            Assert.Throws<ArgumentNullException>(
                () => new SynchronizedList<string>(null, new List<string>()));

            Assert.Throws<ArgumentNullException>(
                () => new SynchronizedList<string>(new object(), null));
        }

        [Fact]
        public void Indexder_IndexIsMinus1_ThrowArgumentOutOfRange()
        {
            var sut = new SynchronizedList<string>() { "abc" };

            var actual = Assert.Throws<ArgumentOutOfRangeException>(
                () => sut[-1] = "def");
            Assert.Equal(-1, actual.ActualValue);
        }

        [Fact]
        public void Insert_IndexOverRange_ThrowArgumentOutOfRange()
        {
            var sut = new SynchronizedList<int>() { 1 };

            var actual = Assert.Throws<ArgumentOutOfRangeException>(
                () => sut.Insert(2, 2));
            Assert.Equal(2, actual.ActualValue);
        }

        [Fact]
        public void RemoveAt_IndexMinus1_ThrowArgumentOutOfRange()
        {
            var sut = new SynchronizedList<int>() { 1 };

            var actual = Assert.Throws<ArgumentOutOfRangeException>(
                () => sut.RemoveAt(-1));
            Assert.Equal(-1, actual.ActualValue);
        }

        [Fact]
        public async Task GetEnumerator_ThreadSafe()
        {
            // Arrange
            var sut = new SynchronizedList<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, };
            var task = Task.Run(() =>
            {
                Parallel.ForEach(Enumerable.Range(11, 90),
                i =>
                {
                    Thread.Sleep(1);
                    sut.Add(i);
                    Debug.WriteLine(i);
                });
            });

            // Act
            var actual = new List<int>();
            foreach (var item in sut)
            {
                await Task.Delay(2);
                actual.Add(item);
            }

            // Assert
            Assert.Equal(10, actual.Count);
            Assert.DoesNotContain(11, actual);
            Assert.DoesNotContain(99, actual);
            Assert.Equal(100, sut.Count);
        }

        [Fact]
        public void VerifyValueType_IntArgIsNull_ThrowArgumentException()
        {
            IList sut = new SynchronizedList<int>() { 1 };

            var actual = Assert.Throws<ArgumentException>(
                () => sut.Remove(null));
            Assert.Equal(
                "The type of item must not be NULL in SynchronizedList. (Parameter 'value')",
                actual.Message);
        }

        [Fact]
        public void VerifyValueType_ArgTypeIsWrong_ThrowArgumentException()
        {
            IList sut = new SynchronizedList<int>() { 1 };

            var actual = Assert.Throws<ArgumentException>(
                () => sut.Remove("1"));

            Assert.Equal(
                "The type of item and T are different in SynchronizedList. (Parameter 'value')",
                actual.Message);
        }
    }
}

using System.Collections.ObjectModel;

using SharedLib.Extensions;

namespace SharedLib.Test.Extensions
{
    public class ListExtensionsTest
    {
        #region AddToHead

        [Fact]
        public void AddToHead_DoNothing_WhenNullList()
        {
            IList<int> sut = null;
            int item = 1;

            sut.AddToHead(item);

            Assert.Null(sut);
        }

        [Fact]
        public void AddToHead_AddItemToEmptyList()
        {
            IList<int> sut = new List<int>();
            int item = 1;

            sut.AddToHead(item);

            Assert.Single(sut, item);
        }

        [Fact]
        public void AddToHead_AddItemToList()
        {
            IList<int> sut = new List<int>() { 2, 3 };
            int item = 1;

            sut.AddToHead(item);

            Assert.Equal(3, sut.Count);
            Assert.Equal(item, sut.First());
        }

        #endregion

        #region RemoveLast

        [Fact]
        public void RemoveLast_EmptyList()
        {
            var sut = new List<int>();
            var actual = sut.RemoveLast();
            Assert.False(actual);
        }

        [Fact]
        public void RemoveLast_ReadOnlyList()
        {
            var list = new List<string>() { "One", "Two", "Three" };
            var sut = new ReadOnlyCollection<string>(list);

            var actual = sut.RemoveLast();

            Assert.False(actual);
            Assert.Collection(sut,
                item => Assert.Equal("One", item),
                item => Assert.Equal("Two", item),
                item => Assert.Equal("Three", item));
        }

        [Fact]
        public void RemoveLast_Success()
        {
            var sut = new List<string>() { "One", "Two", "Three" };

            var actual = sut.RemoveLast();

            Assert.True(actual);
            Assert.DoesNotContain("Three", sut);
            Assert.Equal(2, sut.Count);
        }

        #endregion
    }
}

using FluentAssertions;

using SharedLib.Extensions;

namespace SharedLib.Test.Extensions
{
    public class EnumerableExtensionsTTests
    {
        #region SingleOrDefaultSafe

        [Fact]
        public void SingleOrDefaultSafe_SourceIsNull()
        {
            var defaultValue = new HttpClient();

            HttpClient[] sut = null;

            var actual1 = sut.SingleOrDefaultSafe();
            var actual2 = sut.SingleOrDefaultSafe(x => true, defaultValue);

            actual1.Should().BeNull();
            actual2.Should().Be(defaultValue);
        }

        [Fact]
        public void SingleOrDefaultSafe_PredicateIsNull()
        {
            var defaultValue = new HttpClient();

            var element1 = new HttpClient();
            var element2 = new HttpClient();
            var sut = new HttpClient[] { element1, element2, };

            var act = () => sut.SingleOrDefaultSafe(null, defaultValue);

            act.Should().Throw<ArgumentNullException>().WithParameterName("predicate");
        }

        [Fact]
        public void SingleOrDefaultSafe_HasNoElement()
        {
            var defaultValue = new HttpClient();

            var sut = new HttpClient[] {};

            var actual = sut.SingleOrDefaultSafe(defaultValue);

            actual.Should().Be(defaultValue);
        }

        [Fact]
		public void SingleOrDefaultSafe_HasSingleElement()
		{
			var element1 = new HttpClient();
			var sut = new HttpClient[] { element1, };

			var actual = sut.SingleOrDefaultSafe();

			actual.Should().BeEquivalentTo(sut.SingleOrDefault());
		}

        [Fact]
        public void SingleOrDefaultSafe_HasMultipleElement()
        {
            var defaultValue = new HttpClient();

            var element1 = new HttpClient();
            var element2 = new HttpClient();
            var sut = new HttpClient[] { element1, element2, };

            var actual = sut.SingleOrDefaultSafe(defaultValue);

            actual.Should().Be(defaultValue);
        }

        [Fact]
        public void SingleOrDefaultSafe_HasNoElement_SatisfyCondition()
        {
            var defaultValue = new Name("pontagc");

            var john = new Name("John");
            var mary = new Name("Mary");
            var sut = new Name[] { john, mary, };

            var actual = sut.SingleOrDefaultSafe(x => x.Value.Contains('i'), defaultValue);

            actual.Should().BeEquivalentTo(defaultValue);
        }

        [Fact]
        public void SingleOrDefaultSafe_EmptyCollection()
        {
            var defaultValue = new Name("pontagc");

            var sut = new Name[] { };

            var actual = sut.SingleOrDefaultSafe(x => x.Value.Contains('i'), defaultValue);

            actual.Should().BeEquivalentTo(defaultValue);
        }

        [Fact]
        public void SingleOrDefaultSafe_HasMultipleElement_SatisfyCondition()
        {
            var defaultValue = new Name("pontagc");

            var john = new Name("John");
            var mary = new Name("Mary");
            var sut = new Name[] { john, mary, };

            var actual = sut.SingleOrDefaultSafe(x => x.Value.Length == 4, defaultValue);

            actual.Should().BeEquivalentTo(defaultValue);
        }

        #endregion

        #region Helpers

        private class Name
        {
            public Name(string value)
            {
                this.Value = value;
            }

            public string Value { get; }
        }


        #endregion
    }
}

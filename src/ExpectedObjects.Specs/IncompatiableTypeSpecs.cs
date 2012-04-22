using Machine.Specifications;

namespace ExpectedObjects.Specs
{
	public class when_comparing_objects_with_an_incompatible_property
	{
		static ExpectedObject _expected;
		static Type2 _actual;

		Establish context = () =>
			{
				_expected = new Type1 {SomeProperty = 1}.ToExpectedObject();
				_actual = new Type2 {SomeProperty = "1"};
			};

		It should_return_type_mismatch_message = () => _expected.ShouldMatch(_actual);
	}

	class Type1
	{
		public int SomeProperty { get; set; }
	}

	class Type2
	{
		public string SomeProperty { get; set; }
	}
}
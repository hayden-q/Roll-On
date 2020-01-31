using System.Collections.Generic;
using System.Linq;
using Moq;

namespace RollOn.Tests
{
	public static class MockHelper
	{
		public static Mock<INode> CreateMockedNode(double value)
		{
			var mockedNode = new Mock<INode>();

			mockedNode
				.Setup(node => node.Evaluate(It.IsAny<IRoller>()))
				.Returns(new DiceResult(value, Enumerable.Empty<IEnumerable<DiceRoll>>()));

			mockedNode
				.Setup(node => node.ToString())
				.Returns(value.ToString("0.##"));

			return mockedNode;
		}

		public static Mock<IRandom> CreateMockedRandom(int max)
		{
			var mockedRandom = new Mock<IRandom>();

			mockedRandom
				.Setup(random => random.Next(It.IsAny<int>(), It.IsAny<int>()))
				.Returns(max);

			return mockedRandom;
		}
	}
}
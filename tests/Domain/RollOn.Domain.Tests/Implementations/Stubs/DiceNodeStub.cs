namespace RollOn.Tests
{
	public class DiceNodeStub : IDiceNode
	{
		public INode Count { get; }
		public INode Size { get; }

		public DiceNodeStub(INode count, INode size)
		{
			Count = count;
			Size = size;
		}

		public override string ToString() => $"DCE({Count},{Size})";

		public DiceResult Evaluate(IRoller roller, IVariableInjector variableInjector)
		{
			throw new System.NotImplementedException();
		}
	}
}
namespace RollOn.Tests
{
	public class UnaryNodeStub : INode
	{
		private readonly INode _node;

		public UnaryNodeStub(INode node)
		{
			_node = node;
		}

		public override string ToString() => $"UNA({_node})";

		public DiceResult Evaluate(IRoller roller, IVariableInjector variableInjector)
		{
			throw new System.NotImplementedException();
		}
	}
}
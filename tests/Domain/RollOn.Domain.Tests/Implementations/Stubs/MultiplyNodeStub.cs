namespace RollOn.Tests
{
	public class MultiplyNodeStub : INode
	{
		private readonly INode _left;
		private readonly INode _right;

		public MultiplyNodeStub(INode left, INode right)
		{
			_left = left;
			_right = right;
		}

		public override string ToString() => $"MUL({_left},{_right})";

		public DiceResult Evaluate(IRoller roller, IVariableInjector variableInjector)
		{
			throw new System.NotImplementedException();
		}
	}
}
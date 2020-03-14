namespace RollOn.Tests
{
	public class DivideNodeStub : INode
	{
		private readonly INode _left;
		private readonly INode _right;

		public DivideNodeStub(INode left, INode right)
		{
			_left = left;
			_right = right;
		}

		public override string ToString() => $"DIV({_left},{_right})";

		public DiceResult Evaluate(IRoller roller, IVariableInjector variableInjector)
		{
			throw new System.NotImplementedException();
		}
	}
}
namespace RollOn.Tests
{
	public class SubtractNodeStub : INode
	{
		private readonly INode _left;
		private readonly INode _right;

		public SubtractNodeStub(INode left, INode right)
		{
			_left = left;
			_right = right;
		}

		public override string ToString() => $"SUB({_left},{_right})";

		public DiceResult Evaluate(IRoller roller, IVariableInjector variableInjector)
		{
			throw new System.NotImplementedException();
		}
	}
}
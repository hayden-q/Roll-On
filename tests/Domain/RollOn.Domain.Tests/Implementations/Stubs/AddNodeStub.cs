namespace RollOn.Tests
{
	public class AddNodeStub : INode
	{
		private readonly INode _left; 
		private readonly INode _right;

		public AddNodeStub(INode left, INode right)
		{
			_left = left;
			_right = right;
		}

		public override string ToString() => $"ADD({_left},{_right})";

		public DiceResult Evaluate(IRoller roller, IVariableInjector variableInjector)
		{
			throw new System.NotImplementedException();
		}
	}
}
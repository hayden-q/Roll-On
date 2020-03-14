namespace RollOn.Tests
{
	public class KeepNodeStub : INode
	{
		private readonly INode _dice;
		private readonly INode _keep;

		public KeepNodeStub(IDiceNode dice, INode keep)
		{
			_dice = dice;
			_keep = keep;
		}

		public override string ToString() => $"KEP({_dice},{_keep})";

		public DiceResult Evaluate(IRoller roller, IVariableInjector variableInjector)
		{
			throw new System.NotImplementedException();
		}
	}
}
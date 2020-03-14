namespace RollOn.Tests
{
	public class NumberNodeStub : INode
	{
		private readonly double _number;

		public NumberNodeStub(double number)
		{
			_number = number;
		}

		public override string ToString() => $"{_number}";

		public DiceResult Evaluate(IRoller roller, IVariableInjector variableInjector)
		{
			throw new System.NotImplementedException();
		}
	}
}
namespace RollOn.Tests
{
	public class VariableNodeStub : INode
	{
		private readonly string _name;

		public VariableNodeStub(string name)
		{
			_name = name;
		}

		public override string ToString() => $"VAR:{_name}";

		public DiceResult Evaluate(IRoller roller, IVariableInjector variableInjector)
		{
			throw new System.NotImplementedException();
		}
	}
}
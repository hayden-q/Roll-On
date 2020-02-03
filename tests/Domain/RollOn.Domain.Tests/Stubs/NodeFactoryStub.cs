namespace RollOn.Tests
{
	public class NodeFactoryStub : INodeFactory
	{
		public INode CreateNumber(double number) => new NumberNodeStub(number);
		public INode CreateVariable(string name) => new VariableNodeStub(name);
		public INode CreateUnary(INode node) => new UnaryNodeStub(node);
		public INode CreateAdd(INode left, INode right) => new AddNodeStub(left, right);
		public INode CreateSubtract(INode left, INode right) => new SubtractNodeStub(left, right);
		public INode CreateMultiply(INode left, INode right) => new MultiplyNodeStub(left, right);
		public INode CreateDivide(INode left, INode right) => new DivideNodeStub(left, right);
		public IDiceNode CreateDice(INode count, INode size) => new DiceNodeStub(count, size);
		public INode CreateKeep(IDiceNode dice, INode keep) => new KeepNodeStub(dice, keep);
	}
}
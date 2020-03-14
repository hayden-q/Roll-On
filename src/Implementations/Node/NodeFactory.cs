namespace RollOn
{
	public class NodeFactory : INodeFactory
	{
		public INode CreateNumber(double number) => new NumberNode(number);
		public INode CreateVariable(string name) => new VariableNode(name);
		public INode CreateUnary(INode node) => new UnaryNode(node);
		public INode CreateAdd(INode left, INode right) => new AddNode(left, right);
		public INode CreateSubtract(INode left, INode right) => new SubtractNode(left, right);
		public INode CreateMultiply(INode left, INode right) => new MultiplyNode(left, right);
		public INode CreateDivide(INode left, INode right) => new DivideNode(left, right);
		public IDiceNode CreateDice(INode count, INode size) => new DiceNode(count, size);
		public INode CreateKeep(IDiceNode dice, INode keep) => new KeepNode(dice, keep);
	}
}
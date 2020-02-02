namespace RollOn
{
	public interface INodeFactory
	{
		INode CreateNumber(double number);
		INode CreateVariable(string name);
		INode CreateUnary(INode node);
		INode CreateAdd(INode left, INode right);
		INode CreateSubtract(INode left, INode right);
		INode CreateMultiply(INode left, INode right);
		INode CreateDivide(INode left, INode right);
		IDiceNode CreateDice(INode count, INode size);
		INode CreateKeep(IDiceNode dice, INode keep);
	}
}
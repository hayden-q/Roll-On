namespace RollOn
{
	public interface IDiceNode : INode
	{
		INode Count { get; }
		INode Size { get; }
	}
}
namespace RollOn
{
	public interface INode
	{
		DiceResult Evaluate(IRoller roller, RoundingMode roundingMode);
	}
}
namespace RollOn
{
	public interface IDiceExpression
	{
		DiceResult Evaluate(IRoller roller, RoundingMode roundingMode);
	}
}
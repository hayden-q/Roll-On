namespace RollOn
{
	public interface IDiceExpression
	{
		DiceResult Evaluate(IRoller roller, IVariableInjector variableInjector, RoundingMode roundingMode);
	}
}
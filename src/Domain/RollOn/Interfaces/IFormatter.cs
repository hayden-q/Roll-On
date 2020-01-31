namespace RollOn
{
	public interface IFormatter<in TInput, out TOutput>
	{
		TOutput Format(TInput input);
	}
}
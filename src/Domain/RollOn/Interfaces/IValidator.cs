namespace RollOn
{
	public interface IValidator<in T>
	{
		void Validate(T input);
	}
}
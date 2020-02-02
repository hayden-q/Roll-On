namespace RollOn
{
	public interface IVariableInjector
	{
		void RegisterVariable(string name, ReferenceValue value);
		double RetrieveValue(string name);
	}
}
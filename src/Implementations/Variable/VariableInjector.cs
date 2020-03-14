using System.Collections.Generic;

namespace RollOn
{
	public class VariableInjector : IVariableInjector
	{
		private readonly Dictionary<string, ReferenceValue> _variables;

		public VariableInjector()
		{
			_variables = new Dictionary<string, ReferenceValue>();
		}

		public void RegisterVariable(string name, ReferenceValue value)
		{
			var cleanName = CleanName(name);
			if (_variables.ContainsKey(cleanName))
			{
				throw new InvalidVariableException($"'{name}' already registered.");
			}

			_variables.Add(cleanName, value);
		}

		public double RetrieveValue(string name)
		{
			var cleanName = CleanName(name);

			if (!_variables.ContainsKey(cleanName))
			{
				throw new InvalidVariableException($"'{name}' doesn't exist.");
			}

			return _variables[cleanName].Value;
		}

		private static string CleanName(string name) => name.Trim().ToUpper();
	}
}
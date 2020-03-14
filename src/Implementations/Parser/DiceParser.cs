using System.Linq;

namespace RollOn
{
	public class DiceParser : IDiceParser
	{
		private readonly IExpressionTokenizer _tokenizer;
		private readonly IPrecedenceHandler _precedenceHandler;
		
		public DiceParser() 
		{
			var factory = new NodeFactory();
			_tokenizer = new ExpressionTokenizer();
			_precedenceHandler = new PrecedenceHandler();
			_precedenceHandler.RegisterPrecedence(new OperatorPrecedence(factory, typeof(AddToken), typeof(SubtractToken)));
			_precedenceHandler.RegisterPrecedence(new OperatorPrecedence(factory, typeof(MultiplyToken), typeof(DivideToken)));
			_precedenceHandler.RegisterPrecedence(new DicePrecedence(factory));
			_precedenceHandler.RegisterPrecedence(new UnaryPrecedence(factory));
			_precedenceHandler.RegisterPrecedence(new OperandPrecedence(factory));
		}

		public DiceParser(IExpressionTokenizer tokenizer, IPrecedenceHandler precedenceHandler)
		{
			_tokenizer = tokenizer;
			_precedenceHandler = precedenceHandler;
		}

		public INode Parse(string expression)
		{
			var tokens = _tokenizer.Tokenize(expression);

			if (!tokens.Any())
			{
				return null;
			}

			using (var enumerator = tokens.GetEnumerator().ToStateEnumerator())
			{

				if (enumerator.MoveNext())
				{
					return _precedenceHandler.RecurseToFirst(enumerator);
				}
			}

			return null;
		}
	}
}
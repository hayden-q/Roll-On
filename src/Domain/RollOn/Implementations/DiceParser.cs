using System.Collections.Generic;
using System.Linq;

namespace RollOn
{
	public class DiceParser : IDiceParser
	{
		private readonly IExpressionTokenizer _tokenizer;

		public DiceParser() : this(new ExpressionTokenizer())
		{
		}
		
		public DiceParser(IExpressionTokenizer tokenizer)
		{
			_tokenizer = tokenizer;
		}

		public INode Parse(string expression)
		{
			var tokens = _tokenizer.Tokenize(expression);

			return ToNode(tokens);
		}

		private INode ToNode(IEnumerable<Token> tokens)
		{
			using var enumerator = tokens.GetEnumerator();

			return ParseAddSubtract(enumerator);
		}

        private INode ParseAddSubtract(IEnumerator<Token> tokens)
        {
            while (tokens.MoveNext())
            {
				var lhs = ParseMultiplyDivide(tokens);
                // Work out the operator
				INode operatorNode = null;
                if (tokens.Current is AddToken)
                {
                    op = (a, b) => a + b;
                }
                else if (_tokenizer.Token == Token.Subtract)
                {
                    op = (a, b) => a - b;
                }

                // Binary operator found?
                if (op == null)
                    return lhs;             // no

                // Skip the operator
                _tokenizer.NextToken();

                // Parse the right hand side of the expression
                var rhs = ParseMultiplyDivide(tokens);

                // Create a binary node and use it as the left-hand side from now on
                lhs = new NodeBinary(lhs, rhs, op);
            }

			return null;
		}

        // Parse an sequence of add/subtract operators
		private INode ParseMultiplyDivide(IEnumerator<Token> tokens)
        {
            // Parse the left hand side
            var lhs = ParseUnary(tokens);

            while (true)
            {
                // Work out the operator
                Func<double, double, double> op = null;
                if (_tokenizer.Token == Token.Multiply)
                {
                    op = (a, b) => a * b;
                }
                else if (_tokenizer.Token == Token.Divide)
                {
                    op = (a, b) => a / b;
                }

                // Binary operator found?
                if (op == null)
                    return lhs;             // no

                // Skip the operator
                _tokenizer.NextToken();

                // Parse the right hand side of the expression
                var rhs = ParseUnary(tokens);

                // Create a binary node and use it as the left-hand side from now on
                lhs = new NodeBinary(lhs, rhs, op);
            }
        }


        // Parse a unary operator (eg: negative/positive)
		private INode ParseUnary(IEnumerator<Token> tokens)
        {
            while (true)
            {
                // Positive operator is a no-op so just skip it
                if (_tokenizer.Token == Token.Add)
                {
                    // Skip
                    _tokenizer.NextToken();
                    continue;
                }

                // Negative operator
                if (_tokenizer.Token == Token.Subtract)
                {
                    // Skip
                    _tokenizer.NextToken();

                    // Parse RHS 
                    // Note this recurses to self to support negative of a negative
                    var rhs = ParseUnary();

                    // Create unary node
                    return new NodeUnary(rhs, (a) => -a);
                }

                // No positive/negative operator so parse a leaf node
                return ParseLeaf(tokens);
            }
        }

        private INode ParseLeaf(IEnumerator<Token> tokens)
		{
			if (tokens.Current is ConstantToken token)
			{
				var node = new NumberNode(token.Constant);
				return node;
			}

			throw new InvalidDiceExpressionException($"Unexpected token: {tokens.Current.Value}");
		}
    }
}
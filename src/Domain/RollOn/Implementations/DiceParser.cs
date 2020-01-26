using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RollOn
{
	public class DiceParser : IDiceParser
	{
		public INode Parse(string expression)
		{
			var formattedExpression = ValidateExpression(FormatExpression(expression));
			var postfix = ParseExpressionToPostfix(formattedExpression);
			
			return PostfixToNode(postfix);
		}
		
		private static readonly char[] ValidOperators = { '+', '-', '*', '/' };
		private static readonly char[] ValidBrackets = { '(', ')' };
		private static readonly char[] ValidDiceTokens = { 'D', 'K' };

		private static IEnumerable<char> ValidTokens => ValidOperators.Concat(ValidBrackets).Concat(ValidDiceTokens);
		private static IEnumerable<char> ValidMathOperators => ValidOperators.Concat(ValidBrackets);
		private static IEnumerable<char> ValidOperatorTokens => ValidOperators.Concat(ValidDiceTokens);
		
		private static string ValidateExpression(string value)
		{
			ValidateIllegalCharacters(value);
			ValidateBrackets(value);
			ValidateSequentialOperators(value);
			ValidateDiceOperator(value);

			return value;
		}
		private static void ValidateIllegalCharacters(string value)
		{
			if (value is null)
			{
				throw new ArgumentNullException(nameof(value), "Expression must be set.");
			}

			if (string.IsNullOrWhiteSpace(value))
			{
				throw new InvalidExpressionException("Expression can't be empty or whitespace.");
			}

			if (value.Any(token => !char.IsDigit(token) && !ValidTokens.Contains(token)))
			{
				throw new InvalidExpressionException("Expression contains illegal character(s).");
			}
		}

		private static void ValidateBrackets(string value)
		{
			var openBracketCount = value.Count(token => token == '(');
			var closeBracketCount = value.Count(token => token == ')');

			if (openBracketCount > closeBracketCount)
			{
				throw new InvalidExpressionException("Expression has too many open brackets.");
			}

			if (closeBracketCount > openBracketCount)
			{
				throw new InvalidExpressionException("Expression has too many close brackets.");
			}

			if (value.Contains("()"))
			{
				throw new InvalidExpressionException("Expression contains empty bracket(s).");
			}

			var openBrackets = 0;

			foreach (var token in value)
			{
				if (ValidBrackets.Contains(token))
				{
					openBrackets += token == '(' ? 1 : -1;
				}

				if (openBrackets < 0)
				{
					break;
				}
			}

			if (openBrackets < 0)
			{
				throw new InvalidExpressionException("Expression contains bracket(s) which haven't been closed.");
			}
		}

		private static void ValidateDiceOperator(string value)
		{
			var diceIndexes = value.AllIndexesOf("D").ToArray();
			var keepIndexes = value.AllIndexesOf("K").ToArray();

			if (!diceIndexes.Any())
			{
				return;
			}

			if (diceIndexes.Any(index => index == value.Length - 1))
			{
				throw new InvalidExpressionException("Dice operator must be proceeded by number.");
			}

			foreach (var keepIndex in keepIndexes)
			{
				if (keepIndex == value.Length - 1)
				{
					throw new InvalidExpressionException("Keep operator must be proceeded by number.");
				}

				var diceIndex = diceIndexes
					.Where(index => index + 1 <= keepIndex)
					.Max();

				if (!int.TryParse(value.Substring(diceIndex + 1, keepIndex - diceIndex - 1), out _))
				{
					throw new InvalidExpressionException("Keep operator must be preceded by the Dice operator.");
				}
			}
		}

		private static void ValidateSequentialOperators(string value)
		{
			var previous = value.First();

			foreach (var current in value.Skip(1))
			{
				var previousIsOperator = ValidTokens.Contains(previous);
				var currentIsOperator = ValidOperators.Contains(current);

				if (previousIsOperator && currentIsOperator && !ValidSequentialBrackets(previous, current))
				{
					throw new InvalidExpressionException("Dice Expression contains tokens which illegally follow one another.");
				}

				previous = current;
			}
		}

		private static bool ValidSequentialBrackets(char previous, char current)
		{
			var open = ValidOperatorTokens.Contains(previous) && current == '(' ||  previous == '(' && current == '(';
			var close = ValidOperatorTokens.Contains(current) && previous == ')' || previous == ')' && current == ')';

			return open || close;
		}

		private static string FormatExpression(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return value;
			}
			
			value = value
				.Trim()
				.ToUpper()
				.RemoveWhitespace()
				.Replace("+-", "-")
				.Replace("-+", "-");

			var previous = value.First();
			var tokenBuilder = new StringBuilder();

			if (ValidOperators.Contains(previous))
			{
				if (previous == '+' || previous == '-')
				{
					tokenBuilder.Append($"0{previous}");
				}
				else
				{
					tokenBuilder.Append($"1{previous}");
				}
			}
			else
			{
				tokenBuilder.Append(previous.ToString());
			}

			foreach (var token in value.Skip(1))
			{
				if (ValidDiceTokens.Contains(token) && !char.IsDigit(previous) && previous != ')')
				{
					tokenBuilder.Append($"1{token}");
				}
				else
				{
					tokenBuilder.Append(token.ToString());
				}

				previous = token;
			}

			if (!ValidOperators.Contains(previous))
			{
				return tokenBuilder.ToString();
			}

			if (previous == '+' || previous == '-')
			{
				tokenBuilder.Append("0");
			}
			else
			{
				tokenBuilder.Append("1");
			}

			return tokenBuilder.ToString();
		}

		private static IEnumerable<(string Term, IEnumerable<int> Id)> ParseExpressionToPostfix(string expression)
		{
			var splitInputList = new List<(string Term, IEnumerable<int> Id)>();
			var numberToAdd = new StringBuilder();
			var operatorStack = new Stack<(string Term, IEnumerable<int> Id)>();

			var activeIds = new Stack<int>();
			var nextId = 1;

			for (var currentIndex = 0; currentIndex < expression.Length; currentIndex++)
			{
				var currentCharacter = expression[currentIndex];
				if (char.IsDigit(currentCharacter) || ValidDiceTokens.Contains(currentCharacter))
				{
					numberToAdd.Append(currentCharacter);
				}
				else if (ValidMathOperators.Contains(currentCharacter))
				{
					if (numberToAdd.ToString() != string.Empty)
					{
						if (numberToAdd.ToString().Contains('D') || numberToAdd.ToString().Contains('K'))
						{
							if (activeIds.Count == 0)
							{
								activeIds.Push(nextId);
								nextId++;
							}

							splitInputList.Add((numberToAdd.ToString(), new List<int>(activeIds)));

							if (activeIds.Count > 0 && expression[currentIndex] != '(')
							{
								activeIds.Pop();
							}
						}
						else
						{
							splitInputList.Add((numberToAdd.ToString(), new List<int>(activeIds)));
						}
					}

					numberToAdd.Clear();

					if (operatorStack.Count != 0)
					{
						var (previousCharacter, _) = operatorStack.Peek();

						if (currentCharacter == ')')
						{
							var isDiceBracket = false;
							var bracketDepth = 0;
							for (var index = currentIndex; index >= 0; index--)
							{
								if (expression[index] == ')')
								{
									bracketDepth++;
								}
								else if (expression[index] == '(')
								{
									bracketDepth--;

									if (bracketDepth != 0)
									{
										continue;
									}

									if (index - 1 >= 0 && expression[index - 1] == 'D')
									{
										isDiceBracket = true;
										break;
									}
								}
							}

							while (operatorStack.Count != 0 && operatorStack.Peek().Term != "(")
							{
								splitInputList.Add(operatorStack.Pop());
							}

							if (operatorStack.Count != 0)
							{
								operatorStack.Pop();
							}

							if (isDiceBracket && activeIds.Count > 0)
							{
								activeIds.Pop();
							}
						}
						else if (currentCharacter == '*' || currentCharacter == '/')
						{
							if (previousCharacter == "*" || previousCharacter == "/")
							{
								splitInputList.Add(operatorStack.Pop());
							}
						}
						else if (currentCharacter != '(')
						{
							while (operatorStack.Count != 0 && operatorStack.Peek().Term != "(")
							{
								splitInputList.Add(operatorStack.Pop());
							}
						}
					}

					if (currentCharacter != ')')
					{
						if (currentCharacter == '(')
						{
							var bracketDepth = 0;
							for (var index = currentIndex; index < expression.Length; index++)
							{
								var currentBracketCharacter = expression[index];
								if (currentBracketCharacter == '(')
								{
									bracketDepth++;
								}
								else if (currentBracketCharacter == ')')
								{
									bracketDepth--;

									if (bracketDepth != 0)
									{
										continue;
									}

									if (index + 1 < expression.Length && ValidDiceTokens.Contains(expression[index + 1]))
									{
										activeIds.Push(nextId);
										nextId++;
										break;
									}
								}
							}
						}
						operatorStack.Push((currentCharacter.ToString(), new List<int>(activeIds)));
					}
				}
			}

			if (numberToAdd.ToString() != string.Empty)
			{
				splitInputList.Add((numberToAdd.ToString(), new List<int>(activeIds)));
			}

			while (operatorStack.Count != 0 && operatorStack.Peek().Term != "(")
			{
				splitInputList.Add(operatorStack.Pop());
			}

			return splitInputList;
		}

		private static INode PostfixToNode(IEnumerable<(string Term, IEnumerable<int> Id)> postfix)
		{
			var nodes = new Stack<INode>();
			PostfixToNodeImpl(SortList(postfix), 0, ref nodes);

			return nodes.Pop();
		}

		private static List<(string Term, IOrderedEnumerable<int> Id)> SortList(IEnumerable<(string Term, IEnumerable<int> Id)> postfix)
		{
			return postfix.Select(x => (x.Term, x.Id.OrderBy(y => y))).ToList();
		}

		private static IEnumerable<(string Term, IOrderedEnumerable<int> Id)> SortList(IEnumerable<(string Term, IOrderedEnumerable<int> Id)> postfix, int skip = 0)
		{
			return postfix.Select(x => (x.Term, x.Id.OrderBy(y => y).Skip(skip).OrderBy(y => y))).ToList();
		}

		private static Stack<INode> PostfixToNodeImpl(List<(string Term, IOrderedEnumerable<int> Id)> postfix, int currentId, ref Stack<INode> global)
		{
			var local = new Stack<INode>();

			for (var index = 0; index < postfix.Count; index++)
			{
				var (term, tempIds) = postfix[index];
				var ids = tempIds.ToArray();
				var maxId = ids.Any() ? ids.Max() : 0;
				if (maxId != currentId && !term.Contains("D"))
				{
					var idProcessed = RecurseDiceExpression(postfix, currentId, ids, ref global, ref local);
					index = postfix.LastIndexOf(postfix.Last(x => x.Id.Contains(idProcessed)));
				}
				else if (term.Contains("D"))
				{
					var idProcessed = PushDiceNode(term, postfix, currentId, ids, ref global, ref local);

					if (idProcessed.HasValue)
					{
						index = postfix.LastIndexOf(postfix.Last(x => x.Id.Contains(idProcessed.Value)));
					}
				}
				else if (int.TryParse(term, out var value))
				{
					PushToStacks(new NumberNode(value), ref global, ref local);
				}
				else
				{
					var (firstNode, secondNode) = GetNodes(ref global, ref local);

					PushToStacks(term switch
					{
						"+" => new AddNode(firstNode, secondNode),
						"-" => new SubtractNode(firstNode, secondNode),
						"*" => new MultiplyNode(firstNode, secondNode),
						"/" => new DivideNode(firstNode, secondNode),
						_ => throw new NotImplementedException("Operator not implemented"),
					}, ref global, ref local);
				}
			}

			return local;
		}

		private static int RecurseDiceExpression(IEnumerable<(string Term, IOrderedEnumerable<int> Id)> postfix,
			int currentId, IEnumerable<int> ids, ref Stack<INode> global, ref Stack<INode> local, 
			bool pushToLocal = false, bool doubleDiceBracket = false)
		{
			var idList = new List<int>(ids);
			var skipCount = currentId != 0 && !doubleDiceBracket ? 1 : 0;
			var currentIdToSet = idList.Skip(skipCount).First();

			var currentIds =
				new List<(string Term, IOrderedEnumerable<int> Id)>(SortList(postfix.Where(x => x.Id.Contains(currentIdToSet)),
					skipCount));

			if (currentIds[0].Term.Contains("D"))
			{
				currentIds.RemoveAt(0);
			}

			if (doubleDiceBracket)
			{
				var firstDiceIndex = currentIds
					.Select((id, index) => new {id.Term, Index = index})
					.First(element => element.Term == "D")
					.Index;

				currentIds.RemoveRange(0, firstDiceIndex + 1);
			}

			var nodes = PostfixToNodeImpl(currentIds, currentIdToSet, ref global);
			if (idList.Count <= 1 && !pushToLocal)
			{
				return currentIdToSet;
			}
			
			local.PushRange(nodes);

			return currentIdToSet;
		}

		private static void PushToStacks(INode node, ref Stack<INode> global, ref Stack<INode> local)
		{
			global.Push(node);
			local.Push(node);
		}

		private static (INode first, INode second) GetNodes(ref Stack<INode> global, ref Stack<INode> local)
		{
			var second = global.Pop();
			var first = global.Pop();

			local.TryPop(out _);
			local.TryPop(out _);

			return (first, second);
		}

		private static int? PushDiceNode(string term, IEnumerable<(string Term, IOrderedEnumerable<int> Id)> postfix,
			int currentId, IEnumerable<int> ids, ref Stack<INode> global, ref Stack<INode> local)
		{
			int? idProcessed = null;
			var terms = term.Split('D', 'K');

			DieCount dieCount = null;
			DieSize dieSize = null;
			
			if (int.TryParse(terms[1], out var size))
			{
				dieSize = new DieSize(size);
			}
			
			if (terms.Length == 3)
			{
				if (int.TryParse(terms[0], out var count) && int.TryParse(terms[2], out var keep))
				{
					dieCount = new DieCount(count, keep);
				}
			}
			else
			{
				if (int.TryParse(terms[0], out var count))
				{
					dieCount = new DieCount(count);
				}
			}

			if (dieCount != null && dieSize != null)
			{
				PushToStacks(new DiceNode(dieCount, dieSize), ref global, ref local);
			}
			else if (dieSize != null)
			{
				global.TryPop(out _);
				PushToStacks(new DiceNode(local.Pop(), dieSize), ref global, ref local);
			}
			else if (dieCount != null)
			{
				idProcessed = RecurseDiceExpression(postfix, currentId, ids, ref global, ref local, true);
				global.Pop();
				PushToStacks(new DiceNode(dieCount, local.Pop()), ref global, ref local);
			}
			else
			{
				var countNode = local.Pop();
				idProcessed = RecurseDiceExpression(postfix, currentId, ids, ref global, ref local, true, true);
				global.Pop();
				PushToStacks(new DiceNode(countNode, local.Pop()), ref global, ref local);
			}

			return idProcessed;
		}
	}
}
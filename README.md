
# Roll On

Roll On is an open source library for creating and evaluating dice expressions, with full support of [PEMDAS](https://en.wikipedia.org/wiki/Order_of_operations), evaluating dice notations, and variable support .

## Usage

    
    IDiceParser parser = new DiceParser();
    IDiceExpression expression = parser.Parse("5+4D6K3 * {Character Level} / --(2-1)d4");
   
    IRoller roller = new RandomRoller();
    IVariableInjector injector = new VariableInjector();
    injector.RegisterVariable("Character Level", new ReferenceValue(() => 6));
    DiceResult result = expression.Evaluate(roller, injector, RoundingMode.Down);
    
    // result.Value [double] returns the result of the evaluation
    // result.Rolls [IEnumerable<IEnumerable<DiceRoll>>] displays all the rolls from the evaluation

## Credits
 This project is largely inspired by [DiceNotation](https://github.com/eropple/DiceNotation) project by eropple, and [Writing a Simple Math Expression Engine in C#](https://medium.com/@toptensoftware/writing-a-simple-math-expression-engine-in-c-d414de18d4ce) by Brad Robinson.

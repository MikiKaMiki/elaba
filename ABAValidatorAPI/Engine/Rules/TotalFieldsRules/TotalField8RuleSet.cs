using System.Text.RegularExpressions;

namespace ABAValidatorAPI.Engine.Rules
{
    public partial class TotalField8RuleSet : IFieldRuleSet
    {
        private IList<IFieldRule> _rules;

        private AbaValidationContext _context;

        public TotalField8RuleSet(AbaValidationContext context)
        {
            _rules = [
                new TotalField8Rule1(
                    new TotalField8Rule2(context)
                ),
            ];

            _context = context;
        }

        public FieldValidationResutl ValidateField(string line)
        {
            var result = new FieldValidationResutl();

            // todo: make it recursive
            foreach(var rule in _rules)
            {
                if (!rule.IsValid(line))
                {
                    result.Errors.Add(rule.ErrorMessage);
                }
                else
                {
                    if (rule is IFieldCompositRule)
                    {
                        foreach (var r in (rule as IFieldCompositRule)!.InteralRules)
                        {
                            result.Errors.Add(r.ErrorMessage);
                        }
                    }
                }
            }

            result.IsValid = result.Errors.Count == 0;

            return result;
        }

        internal partial class TotalField8Rule1 : RegexFieldRule, IFieldCompositRule
        {
            protected override Regex Regex => TotalChar75_80();

            public override string ErrorMessage => 
                "Char 75, Size 6, Numeric only valid. Right justified, zero filled.";

            public override bool IsValid(string line)
            {
                return Regex.IsMatch(line);
            }

            [GeneratedRegex(@"^.{74}[0-9]{5}[1-9].*$")]
            private static partial Regex TotalChar75_80();

            private IList<IFieldRule> _rules;

            public IList<IFieldRule> InteralRules { get { return _rules; } }

            public TotalField8Rule1(IFieldRule rule)
            {
                _rules = [rule];
            }

        }

        internal partial class TotalField8Rule2 : IFieldRule
        {
            public string ErrorMessage =>
                "Must equal accumulated number of Record Type 1 items on the file.";

            public bool IsValid(string line)
            {
                int counter = _context.DetailRecordCount;
                string stringValue = line.Substring(75, 6);

                if (int.TryParse(stringValue.TrimStart('0'), out int parsedValue))
                {
                    if (counter == parsedValue)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // todo:
                    //_logger.Warning("Cannot parese value of the field TotalRecod.Position_75_80. Value: {value}");
                    // theoretically we never should rize this point
                    // maybe throw an exception
                    // maybe write an error with additional info
                    // generraly the parse will succeed because it is backed by <ref="TotalField8Rule1">
                    return false;
                }
            }

            private AbaValidationContext _context;

            public TotalField8Rule2(AbaValidationContext context)
            {
                _context = context;
            }
        }
    }
}

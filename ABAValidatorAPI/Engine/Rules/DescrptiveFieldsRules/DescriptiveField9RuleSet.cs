using System.Globalization;

namespace ABAValidatorAPI.Engine.Rules
{
    public class DescriptiveField9RuleSet : FieldRuleSetBase
    {
        protected override short FieldNumber => 9;

        public DescriptiveField9RuleSet()
        {
            _rules.Add(new DescriptiveField9Rule1());
        }

        internal class DescriptiveField9Rule1 : IFieldRule
        {
            public string ErrorMessage 
                => "Must be numeric in the formal of DDMMYY. Must be a valid date. Zero filled.";

            public bool IsValid(string line)
            {
                var value = line.Substring(75, 6);

                var result = DateTime.TryParseExact(
                    value,
                    "ddMMyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime _);

                return result;
            }
        }
    }
}

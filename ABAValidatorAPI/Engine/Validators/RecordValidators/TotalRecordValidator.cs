using ABAValidatorAPI.Engine.Rules;

namespace ABAValidatorAPI.Engine
{
    public class TotalRecordValidator : IRecordValidator
    {
        private readonly List<IFieldRuleSet> _fieldRuleSet;
        private readonly AbaValidationContext _context;
        private CommonRecordRule _recordRule;

        public TotalRecordValidator(AbaValidationContext context)
        {
            _context = context;

            _recordRule = new CommonRecordRule();

            _fieldRuleSet = [
                new TotalField1RuleSet(),

                // todo: 2
                // todo: 3
                // todo: 4
                // todo: 5
                // todo: 6
                // todo: 7
                new TotalField8RuleSet(_context),
                // todo: 9
            ];
        }

        public RecordValidationResult ValidateRecord(int lineNumber, string line)
        {
            var result = new RecordValidationResult() { RecordType = RecordType.Total };

            ValidateRecord(result, lineNumber, line);

            if (result.IsValid)
            {
                ValidateFields(result, lineNumber, line);
            }

            result.LineNumber = lineNumber;

            return result;
        }

        private void ValidateRecord(RecordValidationResult result, int lineNumber, string line)
        {
            _recordRule.ValidateRecord(result, line);
        }

        private void ValidateFields(RecordValidationResult recordResult, int lineNumber, string line)
        {
            foreach (var set in _fieldRuleSet)
            {
                FieldValidationResutl result = set.ValidateField(line);

                if (!result.IsValid)
                {
                    recordResult.IsValid = false;
                    recordResult.FieldValidationResults.Add(result);
                }
            }
        }
    }
}

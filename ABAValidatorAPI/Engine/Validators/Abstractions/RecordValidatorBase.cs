using ABAValidatorAPI.Engine.Rules;

namespace ABAValidatorAPI.Engine
{
    public abstract class RecordValidatorBase : IRecordValidator
    {
        protected abstract RecordType RecordType { get; }

        protected List<IFieldRuleSet> _fieldRuleSet { get; } = [];

        protected CommonRecordRule _recordRule;

        protected RecordValidatorBase()
        {
            _recordRule = new CommonRecordRule();
        }

        public virtual RecordValidationResult ValidateRecord(int lineNumber, string line)
        {
            var rResult = NewResult();

            ValidateRecord(rResult, lineNumber, line);

            if (rResult.IsValid)
            {
                ValidateFields(rResult, lineNumber, line);
            }

            rResult.LineNumber = lineNumber;

            return rResult;
        }

        protected RecordValidationResult NewResult()
        {
            return new RecordValidationResult() {  RecordType = RecordType };
        }

        protected virtual void ValidateRecord(RecordValidationResult result, int lineNumber, string line)
        {
            _recordRule.ValidateRecord(result, line);
        }

        protected virtual void ValidateFields(RecordValidationResult recordResult, int lineNumber, string line)
        {
            foreach (var set in _fieldRuleSet)
            {
                FieldValidationResutl fResult = set.ValidateField(line);

                if (!fResult.IsValid)
                {
                    recordResult.IsValid = false;
                    recordResult.FieldValidationResults.Add(fResult);
                }
            }
        }
    }
}

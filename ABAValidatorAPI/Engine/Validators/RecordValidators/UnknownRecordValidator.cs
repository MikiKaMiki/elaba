using ABAValidatorAPI.Engine.Rules;

namespace ABAValidatorAPI.Engine
{
    public class UnknownRecordValidator : RecordValidatorBase
    {
        protected override RecordType RecordType => RecordType.Unknown;

        public UnknownRecordValidator() : base()
        {
            _fieldRuleSet.Add(new UnknownRecordField1RuleSet());
        }


        //public RecordValidationResult ValidateRecord(int lineNumber, string line)
        //{
        //    var result = new RecordValidationResult() { RecordType = RecordType.Undefined };

        //    ValidateRecord(result, lineNumber, line);

        //    if (result.IsValid)
        //    {
        //        ValidateFields(result, lineNumber, line);
        //    }

        //    result.LineNumber = lineNumber;

        //    return result;
        //}

        //private void ValidateRecord(RecordValidationResult result, int lineNumber, string line)
        //{
        //    _recordRule.ValidateRecord(result, line);
        //}

        //private void ValidateFields(RecordValidationResult recordResult, int lineNumber, string line)
        //{
        //    foreach (var fieldType in _fieldTypes)
        //    {
        //        FieldValidationResutl result = fieldType.ValidateField(line);

        //        if (!result.IsValid)
        //        {
        //            recordResult.IsValid = false;
        //            recordResult.FieldValidationResults.Add(result);
        //        }
        //    }
        //}
    }
}

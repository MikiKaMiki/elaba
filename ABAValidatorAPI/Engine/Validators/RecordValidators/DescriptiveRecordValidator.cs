using ABAValidatorAPI.Engine.Rules;

namespace ABAValidatorAPI.Engine
{
    public class DescriptiveRecordValidator : RecordValidatorBase
    {
        protected override RecordType RecordType => RecordType.Descriptive;

        public DescriptiveRecordValidator() : base ()
        {


            _fieldRuleSet.AddRange(
                [
                    new DescriptiveField1RuleSet(),
                    new DescriptiveField2RuleSet(),
                    new DescriptiveField3RuleSet(),
                    new DescriptiveField4RuleSet(),
                    new DescriptiveField5RuleSet(),
                    new DescriptiveField6RuleSet(),
                    // todo: 7
                    // todo: 8
                    new DescriptiveField9RuleSet(),
                    // todo: 10
                ]
            );
        }

        //public RecordValidationResult ValidateRecord(int lineNumber, string line)
        //{
        //    var rResult = new RecordValidationResult() { RecordType = RecordType.Descriptive };

        //    ValidateRecord(rResult, lineNumber, line);

        //    if (rResult.IsValid)
        //    {
        //        ValidateFields(rResult, lineNumber, line);
        //    }

        //    rResult.LineNumber = lineNumber;
            
        //    return rResult;
        //}

        //private void ValidateRecord(RecordValidationResult result, int lineNumber, string line)
        //{
        //    _recordRule.ValidateRecord(result, line);
        //}

        //private void ValidateFields(RecordValidationResult recordResult, int lineNumber, string line)
        //{
        //    foreach (var fieldType in _fieldTypes)
        //    {
        //        FieldValidationResutl fResult = fieldType.ValidateField(line);

        //        if (!fResult.IsValid)
        //        {
        //            recordResult.IsValid = false;
        //            recordResult.FieldValidationResults.Add(fResult);
        //        }
        //    }
        //}
    }
}

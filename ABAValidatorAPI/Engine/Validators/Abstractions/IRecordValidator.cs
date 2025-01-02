namespace ABAValidatorAPI.Engine
{
    public interface IRecordValidator
    {
        RecordValidationResult ValidateRecord(int lineNumber, string line);
    }
}

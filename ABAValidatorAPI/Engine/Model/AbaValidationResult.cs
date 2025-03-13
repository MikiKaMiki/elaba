namespace ABAValidatorAPI.Engine
{
    public class FileValidationResult
    {
        public bool IsValid { get; set; } = true;

        public IList<RecordValidationResult> RecordValidationResults { get; set; } = [];
    }

    public class RecordValidationResult
    {
        public RecordType RecordType { get; set; }

        public int LineNumber { get; set; }

        public bool IsValid { get; set; } = true;

        //public string Record { get; set; } = string.Empty;

        public IList<string> Errors { get; set; } = [];

        public IList<FieldValidationResutl> FieldValidationResults { get; set; } = [];
    }


    public class FieldValidationResutl
    {
        public short FieldNumber { get; set; }

        public bool IsValid { get; set; } = true;

        public IList<string> Errors { get; set; } = [];

    }

    public enum RecordType
    {
        Unknown = -1,
        Descriptive = 0,
        Detail = 1,
        Total = 7
    }
}

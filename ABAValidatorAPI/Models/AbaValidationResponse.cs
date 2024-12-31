using ABAValidatorAPI.Engine;

namespace ABAValidatorAPI.Models
{
    public class AbaValidationResponse
    {
        public bool IsValid { get; set; }

        public IList<RecordValidationResponse> RecordValidationResults { get; set; } = [];
    }

    public class RecordValidationResponse
    {
        public int LineNumber { get; set; }

        public bool IsValid { get; set; }

        //public string Record { get; set; } = string.Empty;

        public IList<string> Errors { get; set; } = [];
    }


    public static class ToAbaValidationResponse
    {
        public static AbaValidationResponse ToResponse(this FileValidationResult context)
        {
            var response = new AbaValidationResponse();

            // todo: map

            return response;
        }
    }
}

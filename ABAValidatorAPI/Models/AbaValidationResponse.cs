using ABAValidatorAPI.Engine;
using SharpCompress;

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


        public IList<FieldValidationResponse> FieldErrors { get; set; } = [];
    }

    public class FieldValidationResponse
    {
        public short FieldNumber { get; set; }

        public bool IsValid { get; set; }

        public IList<string> Errors { get; set; } = [];
    }


    public static class ToAbaValidationResponse
    {
        public static AbaValidationResponse ToResponse(this FileValidationResult result)
        {
            var response = new AbaValidationResponse();
            response.IsValid = result.IsValid;

            Func<RecordValidationResult, IList<FieldValidationResponse>>
                collectFieldValidationResults = r =>
                {
                    return r.FieldValidationResults.Select(f => new FieldValidationResponse
                    {
                        FieldNumber = f.FieldNumber,
                        IsValid = f.IsValid,
                        Errors = f.Errors,
                    }).ToList();
                };

            result.RecordValidationResults.ForEach(r => {
                response.RecordValidationResults.Add(new RecordValidationResponse()
                {
                    LineNumber = r.LineNumber,
                    IsValid = r.IsValid,
                    Errors = r.Errors,
                    FieldErrors = collectFieldValidationResults(r)
                });
            });

            return response;
        }
    }
}

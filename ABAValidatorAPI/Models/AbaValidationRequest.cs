using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ABAValidatorAPI.Models
{
    public class AbaValidationRequest
    {
        [Required]
        [MinLength(4)]
        [MaxLength(36 + 4)] // todo: clarify
        public string? FileName { get; set; }

        [Required]
        [MinLength(360)]
        [MaxLength((99999 * 120 + 2) * 40)] // todo: clarify
        public string? Base64Content { get; set; }
    }
}

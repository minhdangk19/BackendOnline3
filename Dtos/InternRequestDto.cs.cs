using System.ComponentModel.DataAnnotations;

namespace BackendOnline3.Dtos
{
    public class InternRequestDto : IValidatableObject
    {
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Tên thực tập sinh phải từ 3 đến 255 ký tự.")]
        public string? InternName { get; set; }
        public string? InternAddress { get; set; }
        public IFormFile? ImageData { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string? InternMail { get; set; }
        public string? InternMailReplace { get; set; }
        public string? University { get; set; }
        public string? CitizenIdentification { get; set; }
        public DateTime? CitizenIdentificationDate { get; set; }
        public string? Major { get; set; }
        public bool? Internable { get; set; }
        public bool? FullTime { get; set; }
        public IFormFile? Cvfile { get; set; }
        public int? InternSpecialized { get; set; }
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? TelephoneNum { get; set; }
        public string? InternStatus { get; set; }
        public DateTime? RegisteredDate { get; set; }
        public string? HowToKnowAlta { get; set; }
        public string? InternPassword { get; set; }
        public string? ForeignLanguage { get; set; }
        public short? YearOfExperiences { get; set; }
        public bool? PasswordStatus { get; set; }
        public bool? ReadyToWork { get; set; }
        public bool? InternEnabled { get; set; }
        public float? EntranceTest { get; set; }
        public string? Introduction { get; set; }
        public string? Note { get; set; }
        public string? LinkProduct { get; set; }
        public string? JobFields { get; set; }
        public bool? HiddenToEnterprise { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ImageData != null && ImageData.Length > 50 * 1024 * 1024)
            {
                yield return new ValidationResult("Kích thước file ảnh không được vượt quá 50MB.", new[] { nameof(ImageData) });
            }
            if (Cvfile != null && Cvfile.Length > 50 * 1024 * 1024)
            {
                yield return new ValidationResult("Kích thước file CV không được vượt quá 50MB.", new[] { nameof(Cvfile) });
            }
        }
    }
}
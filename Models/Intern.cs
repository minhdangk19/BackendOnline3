using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendOnline3.Models
{
    [Table("Interns")]
    public class Intern
    {
        [Key]
        public int Id { get; set; }
        public string? InternName { get; set; }
        public string? InternAddress { get; set; }
        public byte[]? ImageData { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? InternMail { get; set; }
        public string? InternMailReplace { get; set; }
        public string? University { get; set; }
        public string? CitizenIdentification { get; set; }
        public DateTime? CitizenIdentificationDate { get; set; }
        public string? Major { get; set; }
        public bool? Internable { get; set; }
        public bool? FullTime { get; set; }
        public string? Cvfile { get; set; }
        public int? InternSpecialized { get; set; }
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
    }
}
using BackendOnline3.Data;
using BackendOnline3.Dtos;
using BackendOnline3.Interface;
using BackendOnline3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendOnline3.Services
{
    public class InternService : IInternService
    {
        private readonly AppDbContext _context;

        public InternService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<dynamic>> GetAllAsync(string userEmail, string? searchQuery, int page, int pageSize)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .ThenInclude(r => r.AllowAccesses)
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
                throw new ArgumentException("Người dùng không tồn tại.");

            var allowedColumns = user.Role.AllowAccesses
                .Where(a => a.TableName == "Interns")
                .SelectMany(a => a.AccessProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(c => c.Trim())
                .Distinct()
                .ToList();

            if (!allowedColumns.Any())
                throw new UnauthorizedAccessException("Không có quyền truy cập cột nào của bảng Interns.");

            var query = _context.Interns.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                query = query.Where(i => i.InternName.ToLower().Contains(searchQuery) ||
                                         i.InternMail.ToLower().Contains(searchQuery));
            }

            query = query.OrderBy(i => i.Id).Skip((page - 1) * pageSize).Take(pageSize);

            var interns = await query.ToListAsync();
            return interns.Select(i => FilterInternColumns(i, allowedColumns)).ToList();
        }

        public async Task<dynamic> GetByIdAsync(int id, string userEmail)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .ThenInclude(r => r.AllowAccesses)
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
                throw new ArgumentException("Người dùng không tồn tại.");

            var allowedColumns = user.Role.AllowAccesses
                .Where(a => a.TableName == "Interns")
                .SelectMany(a => a.AccessProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(c => c.Trim())
                .Distinct()
                .ToList();

            if (!allowedColumns.Any())
                throw new UnauthorizedAccessException("Không có quyền truy cập cột nào của bảng Interns.");

            var intern = await _context.Interns.FindAsync(id);
            if (intern == null)
                return null;

            return FilterInternColumns(intern, allowedColumns);
        }

        public async Task<InternDto> CreateAsync(InternRequestDto internDto)
        {
            var intern = new Intern
            {
                InternName = internDto.InternName,
                InternAddress = internDto.InternAddress,
                ImageData = internDto.ImageData != null ? await ConvertFileToByteArray(internDto.ImageData) : null,
                DateOfBirth = internDto.DateOfBirth,
                InternMail = internDto.InternMail,
                InternMailReplace = internDto.InternMailReplace,
                University = internDto.University,
                CitizenIdentification = internDto.CitizenIdentification,
                CitizenIdentificationDate = internDto.CitizenIdentificationDate,
                Major = internDto.Major,
                Internable = internDto.Internable,
                FullTime = internDto.FullTime,
                Cvfile = internDto.Cvfile != null ? internDto.Cvfile.FileName : null,
                InternSpecialized = internDto.InternSpecialized,
                TelephoneNum = internDto.TelephoneNum,
                InternStatus = internDto.InternStatus,
                RegisteredDate = internDto.RegisteredDate,
                HowToKnowAlta = internDto.HowToKnowAlta,
                InternPassword = internDto.InternPassword,
                ForeignLanguage = internDto.ForeignLanguage,
                YearOfExperiences = internDto.YearOfExperiences,
                PasswordStatus = internDto.PasswordStatus,
                ReadyToWork = internDto.ReadyToWork,
                InternEnabled = internDto.InternEnabled,
                EntranceTest = internDto.EntranceTest,
                Introduction = internDto.Introduction,
                Note = internDto.Note,
                LinkProduct = internDto.LinkProduct,
                JobFields = internDto.JobFields,
                HiddenToEnterprise = internDto.HiddenToEnterprise
            };

            _context.Interns.Add(intern);
            await _context.SaveChangesAsync();

            return MapToInternDto(intern);
        }

        public async Task<InternDto> UpdateAsync(int id, InternRequestDto internDto)
        {
            var intern = await _context.Interns.FindAsync(id);
            if (intern == null)
                return null;

            intern.InternName = internDto.InternName;
            intern.InternAddress = internDto.InternAddress;
            intern.ImageData = internDto.ImageData != null ? await ConvertFileToByteArray(internDto.ImageData) : intern.ImageData;
            intern.DateOfBirth = internDto.DateOfBirth;
            intern.InternMail = internDto.InternMail;
            intern.InternMailReplace = internDto.InternMailReplace;
            intern.University = internDto.University;
            intern.CitizenIdentification = internDto.CitizenIdentification;
            intern.CitizenIdentificationDate = internDto.CitizenIdentificationDate;
            intern.Major = internDto.Major;
            intern.Internable = internDto.Internable;
            intern.FullTime = internDto.FullTime;
            intern.Cvfile = internDto.Cvfile != null ? internDto.Cvfile.FileName : intern.Cvfile;
            intern.InternSpecialized = internDto.InternSpecialized;
            intern.TelephoneNum = internDto.TelephoneNum;
            intern.InternStatus = internDto.InternStatus;
            intern.RegisteredDate = internDto.RegisteredDate;
            intern.HowToKnowAlta = internDto.HowToKnowAlta;
            intern.InternPassword = internDto.InternPassword;
            intern.ForeignLanguage = internDto.ForeignLanguage;
            intern.YearOfExperiences = internDto.YearOfExperiences;
            intern.PasswordStatus = internDto.PasswordStatus;
            intern.ReadyToWork = internDto.ReadyToWork;
            intern.InternEnabled = internDto.InternEnabled;
            intern.EntranceTest = internDto.EntranceTest;
            intern.Introduction = internDto.Introduction;
            intern.Note = internDto.Note;
            intern.LinkProduct = internDto.LinkProduct;
            intern.JobFields = internDto.JobFields;
            intern.HiddenToEnterprise = internDto.HiddenToEnterprise;

            await _context.SaveChangesAsync();
            return MapToInternDto(intern);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var intern = await _context.Interns.FindAsync(id);
            if (intern == null)
                return false;

            _context.Interns.Remove(intern);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<byte[]> ConvertFileToByteArray(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        private dynamic FilterInternColumns(Intern intern, List<string> allowedColumns)
        {
            var result = new ExpandoObject();
            var dict = (IDictionary<string, object>)result;

            dict["Id"] = intern.Id;

            foreach (var column in allowedColumns)
            {
                var property = typeof(Intern).GetProperty(column);
                if (property != null)
                {
                    var value = property.GetValue(intern);
                    dict[column] = value;
                }
            }

            return result;
        }

        private InternDto MapToInternDto(Intern intern)
        {
            return new InternDto
            {
                Id = intern.Id,
                InternName = intern.InternName,
                InternAddress = intern.InternAddress,
                ImageData = intern.ImageData,
                DateOfBirth = intern.DateOfBirth,
                InternMail = intern.InternMail,
                InternMailReplace = intern.InternMailReplace,
                University = intern.University,
                CitizenIdentification = intern.CitizenIdentification,
                CitizenIdentificationDate = intern.CitizenIdentificationDate,
                Major = intern.Major,
                Internable = intern.Internable,
                FullTime = intern.FullTime,
                Cvfile = intern.Cvfile,
                InternSpecialized = intern.InternSpecialized,
                TelephoneNum = intern.TelephoneNum,
                InternStatus = intern.InternStatus,
                RegisteredDate = intern.RegisteredDate,
                HowToKnowAlta = intern.HowToKnowAlta,
                InternPassword = intern.InternPassword,
                ForeignLanguage = intern.ForeignLanguage,
                YearOfExperiences = intern.YearOfExperiences,
                PasswordStatus = intern.PasswordStatus,
                ReadyToWork = intern.ReadyToWork,
                InternEnabled = intern.InternEnabled,
                EntranceTest = intern.EntranceTest,
                Introduction = intern.Introduction,
                Note = intern.Note,
                LinkProduct = intern.LinkProduct,
                JobFields = intern.JobFields,
                HiddenToEnterprise = intern.HiddenToEnterprise
            };
        }
    }
}
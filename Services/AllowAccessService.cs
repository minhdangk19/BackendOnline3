using BackendOnline3.Data;
using BackendOnline3.Dtos;
using BackendOnline3.Interface;
using BackendOnline3.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendOnline3.Services
{
    public class AllowAccessService : IAllowAccessService
    {
        private readonly AppDbContext _context;

        public AllowAccessService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AllowAccessDto>> GetAllAsync()
        {
            return await _context.AllowAccesses
                .Select(a => new AllowAccessDto
                {
                    Id = a.Id,
                    RoleId = a.RoleId,
                    TableName = a.TableName,
                    AccessProperties = a.AccessProperties
                })
                .ToListAsync();
        }

        public async Task<AllowAccessDto> GetByIdAsync(int id)
        {
            var allowAccess = await _context.AllowAccesses.FindAsync(id);
            if (allowAccess == null)
                return null;

            return new AllowAccessDto
            {
                Id = allowAccess.Id,
                RoleId = allowAccess.RoleId,
                TableName = allowAccess.TableName,
                AccessProperties = allowAccess.AccessProperties
            };
        }

        public async Task<AllowAccessDto> CreateAsync(AllowAccessRequestDto allowAccessDto)
        {
            var role = await _context.Roles.FindAsync(allowAccessDto.RoleId);
            if (role == null)
                throw new ArgumentException("Vai trò không tồn tại.");

            var allowAccess = new AllowAccess
            {
                RoleId = allowAccessDto.RoleId,
                TableName = allowAccessDto.TableName,
                AccessProperties = allowAccessDto.AccessProperties
            };

            _context.AllowAccesses.Add(allowAccess);
            await _context.SaveChangesAsync();

            return new AllowAccessDto
            {
                Id = allowAccess.Id,
                RoleId = allowAccess.RoleId,
                TableName = allowAccess.TableName,
                AccessProperties = allowAccess.AccessProperties
            };
        }

        public async Task<AllowAccessDto> UpdateAsync(int id, AllowAccessRequestDto allowAccessDto)
        {
            var allowAccess = await _context.AllowAccesses.FindAsync(id);
            if (allowAccess == null)
                return null;

            var role = await _context.Roles.FindAsync(allowAccessDto.RoleId);
            if (role == null)
                throw new ArgumentException("Vai trò không tồn tại.");

            allowAccess.RoleId = allowAccessDto.RoleId;
            allowAccess.TableName = allowAccessDto.TableName;
            allowAccess.AccessProperties = allowAccessDto.AccessProperties;

            await _context.SaveChangesAsync();

            return new AllowAccessDto
            {
                Id = allowAccess.Id,
                RoleId = allowAccess.RoleId,
                TableName = allowAccess.TableName,
                AccessProperties = allowAccess.AccessProperties
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var allowAccess = await _context.AllowAccesses.FindAsync(id);
            if (allowAccess == null)
                return false;

            _context.AllowAccesses.Remove(allowAccess);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
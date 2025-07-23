using BackendOnline3.Data;
using BackendOnline3.Dtos;
using BackendOnline3.Interface;
using BackendOnline3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendOnline3.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(string? searchQuery, int page, int pageSize)
        {
            var query = _context.Users
                .Include(u => u.Role)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    DateOfBirth = u.DateOfBirth,
                    Email = u.Email,
                    RoleId = u.RoleId,
                    RoleName = u.Role.RoleName
                });

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                query = query.Where(u => u.FullName.ToLower().Contains(searchQuery) ||
                                         u.Email.ToLower().Contains(searchQuery));
            }

            query = query.OrderBy(u => u.UserId).Skip((page - 1) * pageSize).Take(pageSize);
            return await query.ToListAsync();
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    DateOfBirth = u.DateOfBirth,
                    Email = u.Email,
                    RoleId = u.RoleId,
                    RoleName = u.Role.RoleName
                })
                .FirstOrDefaultAsync(u => u.UserId == id);

            return user;
        }

        public async Task<UserDto> CreateAsync(UserRequestDto userDto)
        {
            var role = await _context.Roles.FindAsync(userDto.RoleId);
            if (role == null)
                throw new ArgumentException("Vai trò không tồn tại.");

            var user = new User
            {
                FullName = userDto.FullName,
                DateOfBirth = userDto.DateOfBirth,
                Email = userDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password), // Mã hóa mật khẩu
                RoleId = userDto.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                RoleId = user.RoleId,
                RoleName = role.RoleName
            };
        }

        public async Task<UserDto> UpdateAsync(int id, UserRequestDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return null;

            var role = await _context.Roles.FindAsync(userDto.RoleId);
            if (role == null)
                throw new ArgumentException("Vai trò không tồn tại.");

            user.FullName = userDto.FullName;
            user.DateOfBirth = userDto.DateOfBirth;
            user.Email = userDto.Email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            user.RoleId = userDto.RoleId;

            await _context.SaveChangesAsync();

            return new UserDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                RoleId = user.RoleId,
                RoleName = role.RoleName
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
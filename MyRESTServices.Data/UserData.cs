using Microsoft.EntityFrameworkCore;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRESTServices.Data
{
    public class UserData : IUserData
    {
        private readonly AppDbContext _context;
        public UserData(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Task> ChangePassword(string username, string newPassword)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }
                user.Password = Md5Hash.GetHash(newPassword);
                await _context.SaveChangesAsync();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public Task<IEnumerable<User>> GetAllWithRoles()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            return user;
        }

        public async Task<User> GetUserWithRoles(string username)
        {
            try
            {
                var user = await _context.Users.Include(r => r.Roles).FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> Insert(User entity)
        {
            try
            {
                _context.Users.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task<User> Login(string username, string password)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == Md5Hash.GetHash(password));
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> Update(User entity)
        {
            try
            {
                var user = await GetByUsername(entity.Username);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }
                user.FirstName = entity.FirstName;
                user .LastName = entity.LastName;
                user.Address = entity.Address;   
                user.Email = entity.Email;
                user.Telp = entity.Telp;
                user.SecurityQuestion = entity.SecurityQuestion;
                user.SecurityAnswer = entity.SecurityAnswer;
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

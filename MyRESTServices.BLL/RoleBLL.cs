using AutoMapper;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRESTServices.BLL
{
    public class RoleBLL : IRoleBLL
    {
        private readonly IRoleData _roleData;
        private readonly IMapper _mapper;
        public RoleBLL(IRoleData roleData, IMapper mapper) 
        {
            _roleData = roleData;
            _mapper = mapper;
        }

        public Task<Task> AddRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<Task> AddUserToRole(string username, int roleId)
        {
            try
            {
                await _roleData.AddUserToRole(username, roleId);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task<IEnumerable<RoleDTO>> GetAllRoles()
        {
            try
            {
                var roles = await _roleData.GetAll();
                var rolesDTO = _mapper.Map<IEnumerable<RoleDTO>>(roles);
                return rolesDTO;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}

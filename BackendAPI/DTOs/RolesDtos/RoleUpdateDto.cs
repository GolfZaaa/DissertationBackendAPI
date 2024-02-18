using BackendAPI.DTOs.RolesDtos;
using FluentValidation;

namespace BackendAPI.DTOs.RolesDtos;
    public class RoleUpdateDto : RoleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }


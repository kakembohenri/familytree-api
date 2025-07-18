﻿using familytree_api.Enums;
using System.ComponentModel.DataAnnotations;

namespace familytree_api.Dtos.User
{
    public class UserInputDto
    {
        public int Id { get; set; }
        public int FamilyId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Role { get; set; } = UserRoles.Viewer.ToString();
        public string Born { get; set; } = string.Empty;
        public string PlaceOfBirth { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string? Died { get; set; }

        public string Email { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}

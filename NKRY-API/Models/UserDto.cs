﻿using static NKRY_API.Utilities.Constants;

namespace NKRY_API.Models
{
    public class UserDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Address { get; set; }
        public int MemberForHowLongInYears { get; set; }
    }
}

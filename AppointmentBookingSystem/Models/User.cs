// Models/User.cs
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser  // Inherits from IdentityUser
{
    public string FullName { get; set; }
}
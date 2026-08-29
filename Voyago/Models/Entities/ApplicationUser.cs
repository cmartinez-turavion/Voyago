using Microsoft.AspNetCore.Identity;
using Voyago.Models.Enums;

namespace Voyago.Models.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public MembershipTier MembershipTier { get; set; } = MembershipTier.Classic;
    public long MilesBalance { get; set; }
    public string PreferredCurrency { get; set; } = "USD";
}

namespace Tamam.Application.Settings;

public class PlatformAdminSettings
{
    public const string SectionName = "PlatformAdmin";

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string FullName { get; set; } = "Platform Admin";
}

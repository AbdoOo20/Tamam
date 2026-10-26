namespace Tamam.Application.Settings;

public class PublicLinksSettings
{
    public const string SectionName = "PublicLinks";

    public string BaseUrl { get; set; } = "https://tamam.app";

    public int ConfirmationLinkHours { get; set; } = 72;

    public int SurveyLinkHours { get; set; } = 168;
}

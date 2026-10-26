namespace Tamam.Domain.Enums;

public enum OrderSource : byte
{
    EntryPage = 1,

    PastedMessage = 2,

    FileUpload = 3,

    Webhook = 4,

    Api = 5,
}

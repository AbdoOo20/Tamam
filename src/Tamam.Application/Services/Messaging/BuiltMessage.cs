namespace Tamam.Application.Services.Messaging;

public record BuiltMessage(int? TemplateId, string Body, string? WhatsAppUrl);

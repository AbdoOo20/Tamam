using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Messaging;

public record MessageTemplateDto(int Id, string Code, string Language, MessageChannel Channel, MessageCategory Category, string Body);

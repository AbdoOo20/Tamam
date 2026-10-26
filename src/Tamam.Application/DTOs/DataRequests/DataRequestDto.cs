using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.DataRequests;

public record DataRequestDto(
    int Id,

    long? CustomerId,

    DataRequestType Type,

    DataRequestStatus Status,

    DateTime RequestedAt,

    DateTime? ProcessedAt,

    int? ProcessedByUserId);

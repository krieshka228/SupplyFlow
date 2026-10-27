namespace SupplyFlow.Application.Contracts;

public sealed class EntityNotFoundException(string entity, object key)
    : Exception($"{entity} with key '{key}' was not found.");

public sealed class BusinessRuleException(string message)
    : Exception(message);

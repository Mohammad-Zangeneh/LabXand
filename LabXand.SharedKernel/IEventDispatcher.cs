﻿namespace LabXand.SharedKernel;

public interface IEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent);
}
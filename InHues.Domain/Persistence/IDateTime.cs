using System;

namespace InHues.Domain.Persistence
{
    public interface IDateTime
    {
        DateTime Now { get; }
    }
}

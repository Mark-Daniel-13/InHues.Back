using InHues.Domain.Persistence;
using System;

namespace InHues.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}

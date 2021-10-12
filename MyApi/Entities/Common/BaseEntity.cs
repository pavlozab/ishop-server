using System;

namespace Entities
{
    public abstract class BaseEntity<TKey> 
    {
        public TKey Id { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<Guid>
    {
    }
}
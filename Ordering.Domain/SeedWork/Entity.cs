namespace Ordering.Domain.SeedWork
{
    public abstract class Entity
    {
        private int id;
        private int? requestedHashCode;
        private List<INotification>? domainEvents;

        public virtual int Id 
        { 
            get => id; 
            set => id = value; 
        }

        public IReadOnlyCollection<INotification>? DomainEvents => domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            domainEvents ??= new List<INotification>();
            domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            domainEvents?.Clear();
        }

        public bool IsTransient()
        {
            return this.Id == default;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null or not Entity)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            if (obj is not Entity item || item.IsTransient() || this.IsTransient())
            {
                return false;
            }

            return item.Id == this.Id;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!requestedHashCode.HasValue)
                {
                    requestedHashCode = this.Id.GetHashCode() ^ 25;
                }
            }

            return base.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return object.Equals(left, null) ? object.Equals(right, null) : left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
}

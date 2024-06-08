//Base class type that allows for the creation of event datas that can be used in further features

namespace Events
{
    public class EventData
    {
        public readonly EventIdentifiers EventIdentifiers;

        public EventData(EventIdentifiers identifiers)
        {
            EventIdentifiers = identifiers;
        }
    }
}

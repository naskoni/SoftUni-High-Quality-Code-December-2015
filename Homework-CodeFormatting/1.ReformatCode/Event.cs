namespace _1.ReformatCode
{
    using System;
    using System.Text;

    public class Event : IComparable<Event>
    {
        private DateTime date;

        private string location;

        private string title;

        public Event(DateTime date, string title, string location)
        {
            this.date = date;

            this.title = title;
            this.location = location;
        }

        public int CompareTo(Event @event)
        {
            Event other = @event as Event;
            int comparedByDate = this.date.CompareTo(other.date);
            int comparedByTitle = this.title.CompareTo(other.title);
            int comparedByLocation;
            comparedByLocation = this.location.CompareTo(other.location);
            if (comparedByDate == 0)
            {
                if (comparedByTitle == 0)
                {
                    return comparedByLocation;
                }

                return comparedByTitle;
            }

            return comparedByDate;
        }

        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();
            toString.Append(this.date.ToString("yyyy-MM-ddTHH:mm:ss"));

            toString.Append(" | " + this.title);
            if (this.location != null && this.location != string.Empty)
            {
                toString.Append(" | " + this.location);
            }

            return toString.ToString();
        }
    }
}
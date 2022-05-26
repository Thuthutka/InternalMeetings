using System;
using System.Collections.Generic;
using System.Text;

namespace InternalMeetings
{
    public class Room
    {
        public string RoomName { get; set; }
        public string ResponsiblePerson { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> Participants { get; set; }

    }

}

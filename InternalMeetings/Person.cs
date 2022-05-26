using System;
using System.Collections.Generic;
using System.Text;

namespace InternalMeetings
{
    class Person
    {
        public int PersonID { get; private set; }
        public string FirstName { get; set; }
        //public string LastName;

        public Person (string name)
        {
            this.FirstName = name;
        }

        /*public override string ToString()
        {
            return PersonID + FirstName; 
        }*/


    }
}

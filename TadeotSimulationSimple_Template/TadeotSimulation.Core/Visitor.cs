using System;

namespace TadeotSimulation.Core
{
    public class Visitor
    {
        public int Id { get;  }
        public int Adults { get;  }
        public DateTime EntryTime { get;  }
       

        public Visitor(int id, int adults, DateTime entryTime)
        {
            Id = id;
            Adults = adults;
            EntryTime = entryTime;
        }

    }
}

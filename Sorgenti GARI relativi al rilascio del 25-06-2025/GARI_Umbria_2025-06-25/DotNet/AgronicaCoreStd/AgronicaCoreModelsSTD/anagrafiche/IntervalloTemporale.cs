using System;
using System.Collections.Generic;
using System.Text;

namespace AgronicaCoreModelsSTD.anagrafiche
{
    public class IntervalloTemporale
    {
        // public int Id { get; set; }
        public DateTime inizio { get; set; }
        public DateTime fine { get; set; }

        public IntervalloTemporale(DateTime inizio, DateTime fine)
        {
            this.inizio = inizio;
            this.fine = fine;
        }

        public IntervalloTemporale() {
            this.inizio = new DateTime(1900, 1, 1);
            this.fine = new DateTime(2100, 12, 31);
        }

        public int durataInGiorni()
        {
            throw new NotImplementedException();
        }

        public bool isActive()
        {
            return isActive(new DateTime());
        }

        public bool isActive(DateTime referenceDate)
        {
            throw new NotImplementedException();
        }

        public bool overlaps(IntervalloTemporale timeInterval)
        {
            DateTime start = this.inizio.Date;
            DateTime end = this.fine.Date;
            
            DateTime comparisonStart = timeInterval.inizio.Date;
            DateTime comparisonEnd = timeInterval.fine.Date;

            return end >= comparisonStart && start <= comparisonEnd;
        }

        public bool contains(DateTime date)
        {
            DateTime start = this.inizio.Date;
            DateTime end = this.fine.Date;
            DateTime d = date.Date;

            return start <= d && end >= d;
        }
    }
}

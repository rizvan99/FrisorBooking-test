using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Core.Entities
{
    public class Treatment
    {
        public int TreatmentID { get; set; }
        public string TreatmentName { get; set; }
        public int TreatmentDuration { get; set; }
        public double TreatmentPrice { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AgronicaCoreDTOStd.InData.Agea
{
    public class Equipment
    {
        public Equipment() { }

        public Equipment(DataRow record) 
        {
            calibrationId = (record["numero_certificato"] ?? "").ToString();
        }

        public string calibrationId { get; set; }
    }

    public class EquipmentElement : Equipment
    {
        public EquipmentElement() : base(){ }

        public EquipmentElement(DataRow record) : base(record)
        {
            lastCheckDate = ((DateTime)(record["Validita_Taratura_Inizio"] ?? new DateTime(1900, 1, 1))).ToString("yyyy-MM-dd");
            description = $"{(record["Mac_Des"] ?? string.Empty).ToString()}";
        }

        public string lastCheckDate { get; set; }
        public string description { get; set; }

        public Equipment ToEquipment()
        {
            return new Equipment()
            {
                calibrationId = calibrationId,
            };
        }
    }
}

using AgronicaCoreModelsSTD.baseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AgronicaCoreModelsSTD.metaschema
{
    public class UnitaDiMisura : BaseCodeDescr
    {

        public string simbolo { get; set; }

        public BaseCodeDescr tipoControllo { get; set; }

        public UnitaDiMisura(int codice) : base(codice, "")
        {
            this.tipoControllo = new BaseCodeDescr(0, "");
            this.simbolo = "";
        }

        public UnitaDiMisura() : base(-1, "")
        {
        }
    }

}

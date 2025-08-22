using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace AgronicaCoreModelsSTD.documenti
{
    public interface IAttachmentCheckParams
    {
        string Piva { get; }
        int IdAgenda { get; }
        int MacCod { get; }
    }

    public class AttachmentCheckParams : IAttachmentCheckParams
    {
        public string Piva { get; set; } = "";
        public int IdAgenda { get; set; } = 0;
        public int MacCod { get; set; } = 0;
    }
}

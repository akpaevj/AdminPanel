using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Models
{
    public class InfoBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public InfoBaseConnectionType ConnectionType { get; set; }
        public string Server { get; set; }
        public string InfoBaseName { get; set; }
        public string Path { get; set; }
        public string URL { get; set; }
        public string IBasesContent { get; set; }
        public virtual List<InfoBaseInfoBasesList> InfoBaseInfoBasesLists { get; set; } = new List<InfoBaseInfoBasesList>();

        public void SetIBasesContent()
        {
            if (ConnectionType == InfoBaseConnectionType.File)
            {
                IBasesContent =
                    $"[{Name}]\n" +
                    $"Connect=File=\"{Path}\";\n" +
                    $"ID={Id}";
            }
            else if (ConnectionType == InfoBaseConnectionType.Server)
            {
                IBasesContent =
                    $"[{Name}]\n" +
                    $"Connect=Srvr=\"{Server}\";Ref=\"{InfoBaseName}\";\n" +
                    $"ID={Id}";
            }
            else if (ConnectionType == InfoBaseConnectionType.WebServer)
            {
                IBasesContent =
                    $"[{Name}]\n" +
                    $"Connect=ws=\"{URL}\";\n" +
                    $"ID={Id}";
            }
        }
    }
}

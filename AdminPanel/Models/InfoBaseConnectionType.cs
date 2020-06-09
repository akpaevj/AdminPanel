using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models
{
    public enum InfoBaseConnectionType
    {
        [Display(Name = "Файловый")]
        File,
        [Display(Name = "На сервере 1С")]
        Server,
        [Display(Name = "На web сервере")]
        WebServer
    }
}

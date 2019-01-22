using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.CMS.Chat
{
    public class MassageViewModel
    {
        public MassageViewModel()
        {
            Massages = new List<Massage>();
            NewMessage = new Massage();
        }
        public virtual List<Massage> Massages { get; set; }
        public MessageType MassageType { get; set; }
        public Massage NewMessage { get; set; }
    }
    public enum MessageType
    {
        Income,
        Outcome
    }
}
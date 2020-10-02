using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Email.ViewModel
{
    public class EmailVM
    {
        public List<string> To { get; set; }
        public List<string> Attachements { get; set; }

        public string EmailTemplateKey { get; set; }

        public List<Tuple<string, string>> TokensToReplace_Subject { get; set; }
        public List<Tuple<string, string>> TokensToReplace_Body { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}

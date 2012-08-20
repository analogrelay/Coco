using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coco.Controls
{
    public class FormattedText
    {
        public IList<FormattedTextRun> Runs { get; private set; }

        public FormattedText(TextClassification classification, string text) : this(new FormattedTextRun(classification, text)) { }
        public FormattedText(params FormattedTextRun[] runs)
        {
            Runs = new List<FormattedTextRun>(runs);
        }
    }
}

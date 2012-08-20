using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coco.Controls
{
    public class FormattedTextRun
    {
        public TextClassification Classification { get; private set; }
        public string Text { get; private set; }
        
        public FormattedTextRun(TextClassification classification, string text)
        {
            Classification = classification;
            Text = text;
        }
    }
}

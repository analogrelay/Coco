using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coco.Controls
{
    public static class TextClassifications
    {
        public static readonly TextClassification PlainText = new TextClassification("Core.PlainText", defaultForeground: null, defaultBackground: null);
        public static readonly TextClassification Error = new TextClassification("Core.Error", defaultForeground: ConsoleColor.Red, defaultBackground: null);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;

namespace Coco.Controls
{
    public class TextClassification
    {
        public string ClassificationId { get; private set; }
        public ConsoleColor? DefaultForeground { get; private set; }
        public ConsoleColor? DefaultBackground { get; private set; }

        public TextClassification(string id, ConsoleColor? defaultForeground, ConsoleColor? defaultBackground)
        {
            ClassificationId = id;
            DefaultForeground = defaultForeground;
            DefaultBackground = defaultBackground;
        }

        public virtual void ApplyToRange(TextRange range, IConsoleColorMap colorMap)
        {
            if (DefaultBackground != null)
            {
                range.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(colorMap.MapBackground(DefaultBackground.Value)));
            }
            if (DefaultForeground != null)
            {
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(colorMap.MapBackground(DefaultForeground.Value)));
            }
        }
    }
}

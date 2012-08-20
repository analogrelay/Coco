using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Coco.Controls
{
    public interface IConsoleColorMap
    {
        Color MapForeground(ConsoleColor color);
        Color MapBackground(ConsoleColor color);
    }

    public static class ConsoleColorMap
    {
        public static readonly IConsoleColorMap Default = new DefaultConsoleColorMap();
    }

    public class DefaultConsoleColorMap : IConsoleColorMap
    {
        private Dictionary<ConsoleColor, Color> _map;

        protected internal DefaultConsoleColorMap()
        {
            _map = new Dictionary<ConsoleColor, Color>();
            _map[ConsoleColor.Black] = Colors.Black;
            _map[ConsoleColor.DarkBlue] = Colors.DarkBlue;
            _map[ConsoleColor.DarkGreen] = Colors.DarkGreen;
            _map[ConsoleColor.DarkCyan] = Colors.DarkCyan;
            _map[ConsoleColor.DarkRed] = Colors.DarkRed;
            _map[ConsoleColor.DarkMagenta] = Colors.DarkMagenta;
            _map[ConsoleColor.DarkYellow] = Colors.Gold;
            _map[ConsoleColor.Gray] = Colors.Gray;
            _map[ConsoleColor.DarkGray] = Colors.DarkGray;
            _map[ConsoleColor.Blue] = Colors.Blue;
            _map[ConsoleColor.Green] = Colors.Green;
            _map[ConsoleColor.Cyan] = Colors.Cyan;
            _map[ConsoleColor.Red] = Colors.Red;
            _map[ConsoleColor.Magenta] = Colors.Magenta;
            _map[ConsoleColor.Yellow] = Colors.Yellow;
            _map[ConsoleColor.White] = Colors.White;
        }

        protected void Map(ConsoleColor consoleColor, Color color)
        {
            _map[consoleColor] = color;
        }

        public Color MapForeground(ConsoleColor color)
        {
            Color c;
            if (!_map.TryGetValue(color, out c))
            {
                return Colors.White;
            }
            return c;
        }

        public Color MapBackground(ConsoleColor color)
        {
            Color c;
            if (!_map.TryGetValue(color, out c))
            {
                return Colors.Black;
            }
            return c;
        }
    }
}

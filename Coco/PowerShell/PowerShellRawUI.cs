using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Host;
using System.Text;

namespace Coco.PowerShell
{
    class PowerShellRawUI : PSHostRawUserInterface
    {
        private PowerShellConsoleModel _model;

        public PowerShellRawUI(PowerShellConsoleModel model)
        {
            _model = model;

            ForegroundColor = ConsoleColor.White;
            BackgroundColor = ConsoleColor.Black;
        }

        public override ConsoleColor BackgroundColor { get; set; }
        public override ConsoleColor ForegroundColor { get; set; }

        public override Size BufferSize
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override Coordinates CursorPosition
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override int CursorSize
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool KeyAvailable
        {
            get { throw new NotImplementedException(); }
        }

        public override Size MaxPhysicalWindowSize
        {
            get { throw new NotImplementedException(); }
        }

        public override Size MaxWindowSize
        {
            get { throw new NotImplementedException(); }
        }

        public override Coordinates WindowPosition
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override Size WindowSize
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override string WindowTitle
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void FlushInputBuffer()
        {
            throw new NotImplementedException();
        }

        public override BufferCell[,] GetBufferContents(Rectangle rectangle)
        {
            throw new NotImplementedException();
        }

        public override KeyInfo ReadKey(ReadKeyOptions options)
        {
            throw new NotImplementedException();
        }

        public override void ScrollBufferContents(Rectangle source, Coordinates destination, Rectangle clip, BufferCell fill)
        {
            throw new NotImplementedException();
        }

        public override void SetBufferContents(Rectangle rectangle, BufferCell fill)
        {
            throw new NotImplementedException();
        }

        public override void SetBufferContents(Coordinates origin, BufferCell[,] contents)
        {
            throw new NotImplementedException();
        }
    }
}

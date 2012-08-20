using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Management.Automation.Host;

namespace Coco.PowerShell
{
    class PowerShellUI : PSHostUserInterface
    {
        private PowerShellConsoleModel _model;
        private PowerShellRawUI _rawUI;

        public override PSHostRawUserInterface RawUI
        {
            get { return _rawUI; }
        }

        public PowerShellUI(PowerShellConsoleModel model)
        {
            _model = model;
            _rawUI = new PowerShellRawUI(model);
        }

        public override Dictionary<string, PSObject> Prompt(string caption, string message, Collection<FieldDescription> descriptions)
        {
            throw new NotImplementedException();
        }

        public override int PromptForChoice(string caption, string message, Collection<ChoiceDescription> choices, int defaultChoice)
        {
            throw new NotImplementedException();
        }

        public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName, PSCredentialTypes allowedCredentialTypes, PSCredentialUIOptions options)
        {
            throw new NotImplementedException();
        }

        public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName)
        {
            throw new NotImplementedException();
        }

        public override string ReadLine()
        {
            throw new NotImplementedException();
        }

        public override System.Security.SecureString ReadLineAsSecureString()
        {
            throw new NotImplementedException();
        }

        public override void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
        {
            // TODO: Pay attention to ConsoleColors
            Write(value);
        }

        static readonly char[] NewlineChars = new char[] { '\r', '\n' };
        public override void Write(string value)
        {
            if (value.Contains('\n') || value.Contains('\r'))
            {
                var lines = value.Split(NewlineChars, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    _model.ConsoleHost.Write(line);
                    _model.ConsoleHost.InsertLineBreak();
                }
            }
            else
            {
                _model.ConsoleHost.Write(value);
            }
        }

        public override void WriteDebugLine(string message)
        {
            throw new NotImplementedException();
        }

        public override void WriteErrorLine(string value)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(string value)
        {
            throw new NotImplementedException();
        }

        public override void WriteProgress(long sourceId, ProgressRecord record)
        {
            throw new NotImplementedException();
        }

        public override void WriteVerboseLine(string message)
        {
            throw new NotImplementedException();
        }

        public override void WriteWarningLine(string message)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Host;
using System.Text;

namespace Coco.PowerShell
{
    internal class PowerShellHost : PSHost
    {
        private Guid _instanceId = Guid.NewGuid();
        private PowerShellConsoleModel _model;
        private PowerShellUI _ui;

        public override CultureInfo CurrentCulture
        {
            get { return CultureInfo.CurrentCulture; }
        }

        public override CultureInfo CurrentUICulture
        {
            get { return CultureInfo.CurrentUICulture; }
        }

        public override Guid InstanceId
        {
            get { return _instanceId; }
        }

        public override string Name
        {
            get { return "CocoHost"; }
        }

        public override PSHostUserInterface UI
        {
            get { return _ui; }
        }

        public override Version Version
        {
            get { return typeof(PowerShellHost).Assembly.GetName().Version; }
        }

        public PowerShellHost(PowerShellConsoleModel model)
        {
            _model = model;
            _ui = new PowerShellUI(model);
        }

        public override void EnterNestedPrompt()
        {
            throw new NotImplementedException("Not yet implemented");
        }

        public override void ExitNestedPrompt()
        {
            throw new NotImplementedException("Not yet implemented");
        }

        public override void NotifyBeginApplication()
        {
            throw new NotImplementedException("Not yet implemented");
        }

        public override void NotifyEndApplication()
        {
            throw new NotImplementedException("Not yet implemented");
        }

        public override void SetShouldExit(int exitCode)
        {
            throw new NotImplementedException("Not yet implemented");
        }
    }
}

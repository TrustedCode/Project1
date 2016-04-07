
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Animation_Engine.Engine.Core;
using Animation_Engine.Engine.Core.Graphics;

namespace Animation_Engine
{
    public partial class EngineForm : Form
    {
        private bool _hasFocus;

        private CoreEngine _engine = new CoreEngine();

        public EngineForm()
        {
            InitializeComponent();
            _renderingWindow.GotFocus += new EventHandler(FocusChanged);
            _renderingWindow.LostFocus += new EventHandler(FocusChanged);
            this.GotFocus += new EventHandler(FocusChanged);
            this.LostFocus += new EventHandler(FocusChanged);
        }
        
        private void EngineForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SystemEntry.SYSTEM_SHUTDOWN = true;
        }

        private void EngineForm_Load(object sender, EventArgs e)
        {
            _renderingWindow.Context.MakeCurrent(null);
            _engine.RenderingEngine.SetRenderingTarget(_renderingWindow);
            _engine.StartEngine(this);
        }

        private void FocusChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                _hasFocus = false;
                return;
            }
            _hasFocus = _renderingWindow.Focused;
        }
        
        public bool HasFocus
        {
            get { return _hasFocus; }
            set { _hasFocus = value; }
        }

        private void _compileButton_Click(object sender, EventArgs e)
        {
        }

        private void _textbox_script_TextChanged(object sender, EventArgs e)
        {
        }
    }
}

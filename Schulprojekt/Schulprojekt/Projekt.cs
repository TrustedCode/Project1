using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Schulprojekt 
{
	public class Projekt : Form
	{
		static public void Main ()
		{
			Application.Run (new Projekt());
		}

		public Projekt ()
		{
			this.Width = 500;
			this.Height = 500;
			this.Text = "Test Fenster";
		}
	}
}



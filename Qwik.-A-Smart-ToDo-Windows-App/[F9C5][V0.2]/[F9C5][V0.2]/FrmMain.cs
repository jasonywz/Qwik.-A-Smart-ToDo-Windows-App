//this class is used to set/change the shortcut key for the application
//author: G Swetha
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;


namespace QWIK
{
	/// <summary>
	/// Summary description for FrmMain.
	/// </summary>
	public class FrmMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown NumMinMod;
		private System.Windows.Forms.TextBox TxtKeyEnumValue;
		private System.Windows.Forms.Button BtnChangeHotkey;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.ComponentModel.IContainer components;

		public FrmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            if (File.Exists(Application.StartupPath + "\\HotkeyValue.txt"))
            {
                StreamReader reader = File.OpenText(Application.StartupPath + "\\HotkeyValue.txt");
                int val = -1;
                try
                {
                    val = Int32.Parse(reader.ReadToEnd().Trim());
                }
                catch { }
                if (val != -1)
                {
                    Keys k = (Keys)val;
                    bool success = FrmMain.RegisterHotKey(this.Handle, this.GetType().GetHashCode(), ShortcutInput.Win32ModifiersFromKeys(k), ShortcutInput.CharCodeFromKeys(k));
                    if (success)
                        TxtKeyEnumValue.Text = val.ToString();
                    else
                        MessageBox.Show("Could not register Hotkey - there is probably a conflict.  ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.NumMinMod = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.TxtKeyEnumValue = new System.Windows.Forms.TextBox();
			this.BtnChangeHotkey = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.NumMinMod)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Minimum Modifiers:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// NumMinMod
			// 
			this.NumMinMod.Location = new System.Drawing.Point(152, 8);
			this.NumMinMod.Maximum = new System.Decimal(new int[] {
																	  3,
																	  0,
																	  0,
																	  0});
			this.NumMinMod.Name = "NumMinMod";
			this.NumMinMod.Size = new System.Drawing.Size(48, 20);
			this.NumMinMod.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 104);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Key enum value (Int32):";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// TxtKeyEnumValue
			// 
			this.TxtKeyEnumValue.Location = new System.Drawing.Point(152, 104);
			this.TxtKeyEnumValue.Name = "TxtKeyEnumValue";
			this.TxtKeyEnumValue.ReadOnly = true;
			this.TxtKeyEnumValue.Size = new System.Drawing.Size(88, 20);
			this.TxtKeyEnumValue.TabIndex = 3;
			this.TxtKeyEnumValue.Text = "";
			// 
			// BtnChangeHotkey
			// 
			this.BtnChangeHotkey.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.BtnChangeHotkey.Location = new System.Drawing.Point(72, 192);
			this.BtnChangeHotkey.Name = "BtnChangeHotkey";
			this.BtnChangeHotkey.Size = new System.Drawing.Size(112, 23);
			this.BtnChangeHotkey.TabIndex = 4;
			this.BtnChangeHotkey.Text = "Change Hotkey";
			this.BtnChangeHotkey.Click += new System.EventHandler(this.BtnChangeHotkey_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(32, 136);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(224, 40);
			this.label3.TabIndex = 6;
			this.label3.Text = "(This is the value that can be saved to a config file as it can be cast to \'Keys\'" +
				")";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(32, 32);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(224, 56);
			this.label4.TabIndex = 7;
			this.label4.Text = "(of the 4 modifier keys - Shift, Control, Windows && Alt - you may have a minimum" +
				" requirement to reduce the risk of a conflict with another app)";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 232);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(264, 40);
			this.label5.TabIndex = 8;
			this.label5.Text = "Once you have set a hotkey, test it by going to another window and pressing the h" +
				"otkey. This window should come to the forefront (activate).";
			// 
			// FrmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(280, 285);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.BtnChangeHotkey);
			this.Controls.Add(this.TxtKeyEnumValue);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.NumMinMod);
			this.Controls.Add(this.label1);
			this.Name = "FrmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Hotkey Demo";
			this.Load += new System.EventHandler(this.FrmMain_Load);
			this.Closed += new System.EventHandler(this.FrmMain_Closed);
			((System.ComponentModel.ISupportInitialize)(this.NumMinMod)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion



		private void BtnChangeHotkey_Click(object sender, System.EventArgs e)
		{
			byte minMod = (byte)NumMinMod.Value;
			Keys k = (TxtKeyEnumValue.Text.Length > 0) ? (Keys)Int32.Parse(TxtKeyEnumValue.Text) : Keys.None;
			FrmSpecifyShortcut frm = new FrmSpecifyShortcut(minMod, k);
			if (frm.ShowDialog() == DialogResult.OK)
			{
				bool success = FrmMain.RegisterHotKey(this.Handle, this.GetType().GetHashCode(), frm.ShortcutInput1.Win32Modifiers, frm.ShortcutInput1.CharCode);
				if (success)
				{
					TxtKeyEnumValue.Text = ((int)frm.ShortcutInput1.Keys).ToString();
					StreamWriter writer = File.CreateText(Application.StartupPath + "\\HotkeyValue.txt");
					writer.Write(TxtKeyEnumValue.Text);
					writer.Close();
				}
				else
					MessageBox.Show("Could not register Hotkey - there is probably a conflict.  ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		
		private void FrmMain_Load(object sender, System.EventArgs e)
		{
			if (File.Exists(Application.StartupPath + "\\HotkeyValue.txt"))
			{
				StreamReader reader = File.OpenText(Application.StartupPath + "\\HotkeyValue.txt");
				int val = -1;
				try
				{
					val = Int32.Parse(reader.ReadToEnd().Trim());
				} 
				catch {}
				if (val != -1)
				{
					Keys k = (Keys) val;
					bool success = FrmMain.RegisterHotKey(this.Handle, this.GetType().GetHashCode(), ShortcutInput.Win32ModifiersFromKeys(k), ShortcutInput.CharCodeFromKeys(k));
					if (success)
						TxtKeyEnumValue.Text = val.ToString();
					else
						MessageBox.Show("Could not register Hotkey - there is probably a conflict.  ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}


		private void FrmMain_Closed(object sender, System.EventArgs e)
		{
			FrmMain.UnregisterHotKey(this.Handle, this.GetType().GetHashCode());
		}


		[DllImport("user32.dll")]
		public static extern bool RegisterHotKey(IntPtr hWnd,int id,int fsModifiers,int vlc);
		[DllImport("user32.dll")]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);


		protected override void WndProc(ref Message m)
		{

            if (m.Msg == 0x0312)
            {
                Controller.UI.Activate();
                Controller.UI.Visible = true;
                Controller.UI.ShowIcon = false;
                //MessageBox.Show("Shortcut key successful");
            }
         
			base.WndProc(ref m);
		}

	}
}

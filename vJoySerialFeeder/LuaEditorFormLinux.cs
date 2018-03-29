/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 27.3.2018 г.
 * Time: 15:07 ч.
 */
using System;
using System.Windows.Forms;
using MoonSharp.Interpreter;

namespace vJoySerialFeeder
{
    /// <summary>
    /// Description of LuaEditorForm.
    /// </summary>
    public partial class LuaEditorForm : Form
    {
        public string ScriptSource { get; private set; }
        public LuaEditorForm(string script)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            if (script == null || script.Trim().Length == 0)
            {
                script = Resources.script_template;
            }
            editorBox.Text = script;
        }

        void ButtonSaveClick(object sender, EventArgs e)
        {
            if (testCompile())
            {
                ScriptSource = editorBox.Text;
                DialogResult = DialogResult.OK;
            }
        }

        void ButtonTestCompileClick(object sender, EventArgs e)
        {
            if (testCompile())
                MessageBox.Show("Script compiled successfully");
        }

        bool testCompile()
        {
            try
            {
                var lua = new Lua(editorBox.Text);
                lua.Test();
            }
            catch (InterpreterException ex)
            {
                MessageBox.Show(ex.DecoratedMessage);
                return false;
            }
            return true;
        }

        void CMenuOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mCopy.Enabled = mCut.Enabled = editorBox.SelectionLength > 0;
            mUndo.Enabled = editorBox.CanUndo;
            //mRedo.Enabled = editorBox.CanRedo;
        }

        /// <summary>
        /// TextBox on Linux seems to have broken TAB support
        /// here we replace TAB with 4 spaces
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void EditBox_OnKeyDown(object sender, KeyEventArgs e)
        {
        	if (e.KeyCode == Keys.Tab) {
        		int caretPosition = editorBox.SelectionStart;
                editorBox.Text = editorBox.Text.Insert(caretPosition, "    ");
                editorBox.SelectionStart = caretPosition + 4;
        		e.Handled = true;
        	}
        }
	}

}

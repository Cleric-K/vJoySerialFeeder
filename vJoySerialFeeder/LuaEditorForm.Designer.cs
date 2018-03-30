/*
 * Created by SharpDevelop.
 * User: Cleric
 * Date: 27.3.2018 г.
 * Time: 15:07 ч.
 */
namespace vJoySerialFeeder
{
	partial class LuaEditorForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LuaEditorForm));
			this.buttonSave = new System.Windows.Forms.Button();
			this.buttonTestCompile = new System.Windows.Forms.Button();
			this.editorBox = new FastColoredTextBoxNS.FastColoredTextBox();
			this.cMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.mCut = new System.Windows.Forms.ToolStripMenuItem();
			this.mPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.mSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mUndo = new System.Windows.Forms.ToolStripMenuItem();
			this.mRedo = new System.Windows.Forms.ToolStripMenuItem();
			this.buttonCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.editorBox)).BeginInit();
			this.cMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonSave
			// 
			this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSave.Location = new System.Drawing.Point(615, 508);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new System.Drawing.Size(103, 23);
			this.buttonSave.TabIndex = 1;
			this.buttonSave.Text = "Save and Close";
			this.buttonSave.UseVisualStyleBackColor = true;
			this.buttonSave.Click += new System.EventHandler(this.ButtonSaveClick);
			// 
			// buttonTestCompile
			// 
			this.buttonTestCompile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonTestCompile.Location = new System.Drawing.Point(432, 508);
			this.buttonTestCompile.Name = "buttonTestCompile";
			this.buttonTestCompile.Size = new System.Drawing.Size(85, 23);
			this.buttonTestCompile.TabIndex = 2;
			this.buttonTestCompile.Text = "Test Compile";
			this.buttonTestCompile.UseVisualStyleBackColor = true;
			this.buttonTestCompile.Click += new System.EventHandler(this.ButtonTestCompileClick);
			// 
			// editorBox
			// 
			this.editorBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.editorBox.AutoCompleteBracketsList = new char[] {
						'(',
						')',
						'{',
						'}',
						'[',
						']',
						'\"',
						'\"',
						'\'',
						'\''};
			this.editorBox.AutoIndentCharsPatterns = "\r\n^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>.+)\r\n";
			this.editorBox.AutoScrollMinSize = new System.Drawing.Size(27, 14);
			this.editorBox.BackBrush = null;
			this.editorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.editorBox.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
			this.editorBox.CharHeight = 14;
			this.editorBox.CharWidth = 8;
			this.editorBox.CommentPrefix = "--";
			this.editorBox.ContextMenuStrip = this.cMenu;
			this.editorBox.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.editorBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.editorBox.Font = new System.Drawing.Font("Courier New", 9.75F);
			this.editorBox.IsReplaceMode = false;
			this.editorBox.Language = FastColoredTextBoxNS.Language.Lua;
			this.editorBox.LeftBracket = '(';
			this.editorBox.LeftBracket2 = '{';
			this.editorBox.Location = new System.Drawing.Point(12, 12);
			this.editorBox.Name = "editorBox";
			this.editorBox.Paddings = new System.Windows.Forms.Padding(0);
			this.editorBox.RightBracket = ')';
			this.editorBox.RightBracket2 = '}';
			this.editorBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.editorBox.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("editorBox.ServiceColors")));
			this.editorBox.Size = new System.Drawing.Size(706, 490);
			this.editorBox.TabIndex = 3;
			this.editorBox.Zoom = 100;
			// 
			// cMenu
			// 
			this.cMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.mCopy,
									this.mCut,
									this.mPaste,
									this.mSelectAll,
									this.toolStripMenuItem2,
									this.mUndo,
									this.mRedo});
			this.cMenu.Name = "contextMenuStrip1";
			this.cMenu.Size = new System.Drawing.Size(123, 142);
			this.cMenu.Opening += new System.ComponentModel.CancelEventHandler(this.CMenuOpening);
			// 
			// mCopy
			// 
			this.mCopy.Image = global::vJoySerialFeeder.Resources.mCopy_Image;
			this.mCopy.Name = "mCopy";
			this.mCopy.Size = new System.Drawing.Size(122, 22);
			this.mCopy.Text = "Copy";
			this.mCopy.Click += new System.EventHandler(this.MCopyClick);
			// 
			// mCut
			// 
			this.mCut.Image = global::vJoySerialFeeder.Resources.mCut_Image;
			this.mCut.Name = "mCut";
			this.mCut.Size = new System.Drawing.Size(122, 22);
			this.mCut.Text = "Cut";
			this.mCut.Click += new System.EventHandler(this.MCutClick);
			// 
			// mPaste
			// 
			this.mPaste.Image = global::vJoySerialFeeder.Resources.mPaste_Image;
			this.mPaste.Name = "mPaste";
			this.mPaste.Size = new System.Drawing.Size(122, 22);
			this.mPaste.Text = "Paste";
			this.mPaste.Click += new System.EventHandler(this.MPasteClick);
			// 
			// mSelectAll
			// 
			this.mSelectAll.Name = "mSelectAll";
			this.mSelectAll.Size = new System.Drawing.Size(122, 22);
			this.mSelectAll.Text = "Select All";
			this.mSelectAll.Click += new System.EventHandler(this.MSelectAllClick);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(119, 6);
			// 
			// mUndo
			// 
			this.mUndo.Image = global::vJoySerialFeeder.Resources.mUndo_Image;
			this.mUndo.Name = "mUndo";
			this.mUndo.Size = new System.Drawing.Size(122, 22);
			this.mUndo.Text = "Undo";
			this.mUndo.Click += new System.EventHandler(this.MUndoClick);
			// 
			// mRedo
			// 
			this.mRedo.Image = global::vJoySerialFeeder.Resources.mRedo_Image;
			this.mRedo.Name = "mRedo";
			this.mRedo.Size = new System.Drawing.Size(122, 22);
			this.mRedo.Text = "Redo";
			this.mRedo.Click += new System.EventHandler(this.MRedoClick);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(534, 508);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// LuaEditorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(730, 543);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.editorBox);
			this.Controls.Add(this.buttonTestCompile);
			this.Controls.Add(this.buttonSave);
			this.Icon = global::vJoySerialFeeder.Resources.editor_icon;
			this.Name = "LuaEditorForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Lua Editor";
			((System.ComponentModel.ISupportInitialize)(this.editorBox)).EndInit();
			this.cMenu.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ToolStripMenuItem mSelectAll;
		private System.Windows.Forms.ToolStripMenuItem mRedo;
		private System.Windows.Forms.ToolStripMenuItem mUndo;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mPaste;
		private System.Windows.Forms.ToolStripMenuItem mCut;
		private System.Windows.Forms.ToolStripMenuItem mCopy;
		private System.Windows.Forms.ContextMenuStrip cMenu;
		private System.Windows.Forms.Button buttonCancel;
		private FastColoredTextBoxNS.FastColoredTextBox editorBox;
		private System.Windows.Forms.Button buttonTestCompile;
		private System.Windows.Forms.Button buttonSave;
		
		void MCopyClick(object sender, System.EventArgs e)
		{
			editorBox.Copy();
		}
		
		void MCutClick(object sender, System.EventArgs e)
		{
			editorBox.Cut();
		}
		
		void MPasteClick(object sender, System.EventArgs e)
		{
			editorBox.Paste();
		}
		
		void MUndoClick(object sender, System.EventArgs e)
		{
			editorBox.Undo();
		}
		
		void MRedoClick(object sender, System.EventArgs e)
		{
			editorBox.Redo();
		}
		
		void MSelectAllClick(object sender, System.EventArgs e)
		{
			editorBox.Selection.SelectAll();
		}
	}
}

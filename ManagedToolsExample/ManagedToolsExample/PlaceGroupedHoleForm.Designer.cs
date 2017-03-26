/*--------------------------------------------------------------------------------------+
|
|     $Source: MstnExamples/ManagedToolsExample/PlaceGroupedHoleForm.Designer.cs $
|
|  $Copyright: (c) 2015 Bentley Systems, Incorporated. All rights reserved. $
|
+--------------------------------------------------------------------------------------*/
namespace ManagedToolsExample
{
    partial class PlaceGroupedHoleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
        if (disposing && (components != null))
        {
        components.Dispose();
        }
        base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ChkAddFill = new System.Windows.Forms.CheckBox();
            this.CmbNoOfHoles = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ChkAddFill);
            this.groupBox1.Controls.Add(this.CmbNoOfHoles);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(292, 153);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // ChkAddFill
            // 
            this.ChkAddFill.AutoSize = true;
            this.ChkAddFill.Checked = true;
            this.ChkAddFill.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkAddFill.Location = new System.Drawing.Point(13, 58);
            this.ChkAddFill.Name = "ChkAddFill";
            this.ChkAddFill.Size = new System.Drawing.Size(113, 21);
            this.ChkAddFill.TabIndex = 2;
            this.ChkAddFill.Text = "Add Fill Color";
            this.ChkAddFill.UseVisualStyleBackColor = true;
            this.ChkAddFill.CheckedChanged += new System.EventHandler(this.ChkAddFill_CheckedChanged);
            // 
            // CmbNoOfHoles
            // 
            this.CmbNoOfHoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbNoOfHoles.FormattingEnabled = true;
            this.CmbNoOfHoles.Items.AddRange(new object[] {
            "1",
            "2",
            "4"});
            this.CmbNoOfHoles.Location = new System.Drawing.Point(134, 97);
            this.CmbNoOfHoles.Name = "CmbNoOfHoles";
            this.CmbNoOfHoles.Size = new System.Drawing.Size(142, 24);
            this.CmbNoOfHoles.TabIndex = 1;
            this.CmbNoOfHoles.SelectedIndexChanged += new System.EventHandler(this.CmbNoOfHoles_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of Holes:";
            // 
            // PlaceGroupedHoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 177);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlaceGroupedHoleForm";
            this.Text = "Place Grouped Hole";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox CmbNoOfHoles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ChkAddFill;
    }
}
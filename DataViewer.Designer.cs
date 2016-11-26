namespace DataViewer
{
	partial class DataViewer
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
            this.gridParameters = new System.Windows.Forms.DataGridView();
            this.gridResults = new System.Windows.Forms.DataGridView();
            this.labelQueries = new System.Windows.Forms.Label();
            this.labelParameters = new System.Windows.Forms.Label();
            this.buttonGo = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.cboProcs = new System.Windows.Forms.ComboBox();
            this.DisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParameterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParameterValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridParameters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridResults)).BeginInit();
            this.SuspendLayout();
            // 
            // gridParameters
            // 
            this.gridParameters.AllowUserToAddRows = false;
            this.gridParameters.AllowUserToDeleteRows = false;
            this.gridParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridParameters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DisplayName,
            this.ParameterName,
            this.DataType,
            this.ParameterValue});
            this.gridParameters.Location = new System.Drawing.Point(111, 37);
            this.gridParameters.Margin = new System.Windows.Forms.Padding(4);
            this.gridParameters.Name = "gridParameters";
            this.gridParameters.Size = new System.Drawing.Size(501, 134);
            this.gridParameters.TabIndex = 2;
            // 
            // gridResults
            // 
            this.gridResults.AllowUserToAddRows = false;
            this.gridResults.AllowUserToDeleteRows = false;
            this.gridResults.AllowUserToOrderColumns = true;
            this.gridResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResults.Location = new System.Drawing.Point(111, 234);
            this.gridResults.Margin = new System.Windows.Forms.Padding(4);
            this.gridResults.Name = "gridResults";
            this.gridResults.ReadOnly = true;
            this.gridResults.Size = new System.Drawing.Size(501, 332);
            this.gridResults.TabIndex = 8;
            this.gridResults.TabStop = false;
            // 
            // labelQueries
            // 
            this.labelQueries.AutoSize = true;
            this.labelQueries.Location = new System.Drawing.Point(16, 11);
            this.labelQueries.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelQueries.Name = "labelQueries";
            this.labelQueries.Size = new System.Drawing.Size(58, 17);
            this.labelQueries.TabIndex = 9;
            this.labelQueries.Text = "Queries";
            // 
            // labelParameters
            // 
            this.labelParameters.AutoSize = true;
            this.labelParameters.Location = new System.Drawing.Point(16, 37);
            this.labelParameters.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelParameters.Name = "labelParameters";
            this.labelParameters.Size = new System.Drawing.Size(81, 17);
            this.labelParameters.TabIndex = 10;
            this.labelParameters.Text = "Parameters";
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(111, 178);
            this.buttonGo.Margin = new System.Windows.Forms.Padding(4);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(100, 48);
            this.buttonGo.TabIndex = 3;
            this.buttonGo.Text = "&Go";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(240, 178);
            this.buttonExport.Margin = new System.Windows.Forms.Padding(4);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(100, 48);
            this.buttonExport.TabIndex = 4;
            this.buttonExport.Text = "&Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // cboProcs
            // 
            this.cboProcs.FormattingEnabled = true;
            this.cboProcs.Location = new System.Drawing.Point(111, 0);
            this.cboProcs.Margin = new System.Windows.Forms.Padding(4);
            this.cboProcs.Name = "cboProcs";
            this.cboProcs.Size = new System.Drawing.Size(283, 24);
            this.cboProcs.TabIndex = 1;
            this.cboProcs.SelectedIndexChanged += new System.EventHandler(this.cboProcs_SelectedIndexChanged);
            // 
            // DisplayName
            // 
            this.DisplayName.HeaderText = "Parameter Name";
            this.DisplayName.Name = "DisplayName";
            this.DisplayName.ReadOnly = true;
            // 
            // ParameterName
            // 
            this.ParameterName.HeaderText = "Hidden Parameter Name";
            this.ParameterName.Name = "ParameterName";
            this.ParameterName.Visible = false;
            // 
            // DataType
            // 
            this.DataType.HeaderText = "Data Type";
            this.DataType.Name = "DataType";
            this.DataType.Visible = false;
            // 
            // ParameterValue
            // 
            this.ParameterValue.HeaderText = "Parameter Value";
            this.ParameterValue.Name = "ParameterValue";
            // 
            // DataViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 599);
            this.Controls.Add(this.cboProcs);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.labelParameters);
            this.Controls.Add(this.labelQueries);
            this.Controls.Add(this.gridResults);
            this.Controls.Add(this.gridParameters);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DataViewer";
            this.Text = "Query Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.gridParameters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView gridParameters;
		private System.Windows.Forms.DataGridView gridResults;
		private System.Windows.Forms.Label labelQueries;
		private System.Windows.Forms.Label labelParameters;
		private System.Windows.Forms.Button buttonGo;
		private System.Windows.Forms.Button buttonExport;
		private System.Windows.Forms.ComboBox cboProcs;
        private System.Windows.Forms.DataGridViewTextBoxColumn DisplayName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParameterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParameterValue;
    }
}


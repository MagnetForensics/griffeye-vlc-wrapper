namespace Griffeye.VideoPlayerForms.Test;

partial class VideoPlayerForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.videoPlayerPanel = new System.Windows.Forms.Panel();
            this.subProcessButton = new System.Windows.Forms.Button();
            this.loadFileButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // videoPlayerPanel
            // 
            this.videoPlayerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.videoPlayerPanel.AutoSize = true;
            this.videoPlayerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.videoPlayerPanel.Location = new System.Drawing.Point(17, 20);
            this.videoPlayerPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.videoPlayerPanel.Name = "videoPlayerPanel";
            this.videoPlayerPanel.Size = new System.Drawing.Size(1359, 682);
            this.videoPlayerPanel.TabIndex = 0;
            // 
            // subProcessButton
            // 
            this.subProcessButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.subProcessButton.Location = new System.Drawing.Point(17, 742);
            this.subProcessButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.subProcessButton.Name = "subProcessButton";
            this.subProcessButton.Size = new System.Drawing.Size(174, 65);
            this.subProcessButton.TabIndex = 1;
            this.subProcessButton.Text = "Start SubProcess";
            this.subProcessButton.UseVisualStyleBackColor = true;
            this.subProcessButton.Click += new System.EventHandler(this.subProcessButton_Click);
            // 
            // loadFileButton
            // 
            this.loadFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.loadFileButton.Location = new System.Drawing.Point(200, 742);
            this.loadFileButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.loadFileButton.Name = "loadFileButton";
            this.loadFileButton.Size = new System.Drawing.Size(174, 65);
            this.loadFileButton.TabIndex = 2;
            this.loadFileButton.Text = "Load File";
            this.loadFileButton.UseVisualStyleBackColor = true;
            this.loadFileButton.Click += new System.EventHandler(this.loadFileButton_Click);
            // 
            // playButton
            // 
            this.playButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.playButton.Location = new System.Drawing.Point(504, 742);
            this.playButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(91, 65);
            this.playButton.TabIndex = 3;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stopButton.Location = new System.Drawing.Point(604, 742);
            this.stopButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(91, 65);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "Pause";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // VideoPlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1393, 841);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.loadFileButton);
            this.Controls.Add(this.subProcessButton);
            this.Controls.Add(this.videoPlayerPanel);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "VideoPlayerForm";
            this.Text = "Video Player Form";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private Panel videoPlayerPanel;
    private Button subProcessButton;
    private Button loadFileButton;
    private Button playButton;
    private Button stopButton;
}
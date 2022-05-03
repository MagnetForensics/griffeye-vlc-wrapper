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
            this.stepBackButton = new System.Windows.Forms.Button();
            this.stepForwardButton = new System.Windows.Forms.Button();
            this.skipToEndButton = new System.Windows.Forms.Button();
            this.skipToStartButton = new System.Windows.Forms.Button();
            this.timeHeadingLabel = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.timeLengthHeadingLabel = new System.Windows.Forms.Label();
            this.timeLengthLabel = new System.Windows.Forms.Label();
            this.statusHeadingLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.mediaHeadingLabel = new System.Windows.Forms.Label();
            this.mediaLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // videoPlayerPanel
            // 
            this.videoPlayerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.videoPlayerPanel.AutoSize = true;
            this.videoPlayerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.videoPlayerPanel.Location = new System.Drawing.Point(12, 12);
            this.videoPlayerPanel.Name = "videoPlayerPanel";
            this.videoPlayerPanel.Size = new System.Drawing.Size(952, 414);
            this.videoPlayerPanel.TabIndex = 0;
            // 
            // subProcessButton
            // 
            this.subProcessButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.subProcessButton.Location = new System.Drawing.Point(12, 478);
            this.subProcessButton.Name = "subProcessButton";
            this.subProcessButton.Size = new System.Drawing.Size(122, 39);
            this.subProcessButton.TabIndex = 1;
            this.subProcessButton.Text = "Start SubProcess";
            this.subProcessButton.UseVisualStyleBackColor = true;
            this.subProcessButton.Click += new System.EventHandler(this.subProcessButton_Click);
            // 
            // loadFileButton
            // 
            this.loadFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.loadFileButton.Location = new System.Drawing.Point(140, 478);
            this.loadFileButton.Name = "loadFileButton";
            this.loadFileButton.Size = new System.Drawing.Size(122, 39);
            this.loadFileButton.TabIndex = 2;
            this.loadFileButton.Text = "Load File";
            this.loadFileButton.UseVisualStyleBackColor = true;
            this.loadFileButton.Click += new System.EventHandler(this.loadFileButton_Click);
            // 
            // playButton
            // 
            this.playButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.playButton.Location = new System.Drawing.Point(570, 478);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(64, 39);
            this.playButton.TabIndex = 3;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // stepBackButton
            // 
            this.stepBackButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stepBackButton.Location = new System.Drawing.Point(500, 478);
            this.stepBackButton.Name = "stepBackButton";
            this.stepBackButton.Size = new System.Drawing.Size(64, 39);
            this.stepBackButton.TabIndex = 5;
            this.stepBackButton.Text = "<<";
            this.stepBackButton.UseVisualStyleBackColor = true;
            this.stepBackButton.Click += new System.EventHandler(this.stepBackButton_Click);
            // 
            // stepForwardButton
            // 
            this.stepForwardButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stepForwardButton.Location = new System.Drawing.Point(640, 478);
            this.stepForwardButton.Name = "stepForwardButton";
            this.stepForwardButton.Size = new System.Drawing.Size(64, 39);
            this.stepForwardButton.TabIndex = 6;
            this.stepForwardButton.Text = ">>";
            this.stepForwardButton.UseVisualStyleBackColor = true;
            this.stepForwardButton.Click += new System.EventHandler(this.stepForwardButton_Click);
            // 
            // skipToEndButton
            // 
            this.skipToEndButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.skipToEndButton.Location = new System.Drawing.Point(710, 478);
            this.skipToEndButton.Name = "skipToEndButton";
            this.skipToEndButton.Size = new System.Drawing.Size(64, 39);
            this.skipToEndButton.TabIndex = 7;
            this.skipToEndButton.Text = ">|";
            this.skipToEndButton.UseVisualStyleBackColor = true;
            this.skipToEndButton.Click += new System.EventHandler(this.skipToEndButton_Click);
            // 
            // skipToStartButton
            // 
            this.skipToStartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.skipToStartButton.Location = new System.Drawing.Point(430, 478);
            this.skipToStartButton.Name = "skipToStartButton";
            this.skipToStartButton.Size = new System.Drawing.Size(64, 39);
            this.skipToStartButton.TabIndex = 8;
            this.skipToStartButton.Text = "|<";
            this.skipToStartButton.UseVisualStyleBackColor = true;
            this.skipToStartButton.Click += new System.EventHandler(this.skipToStartButton_Click);
            // 
            // timeHeadingLabel
            // 
            this.timeHeadingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.timeHeadingLabel.AutoSize = true;
            this.timeHeadingLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.timeHeadingLabel.Location = new System.Drawing.Point(430, 445);
            this.timeHeadingLabel.Name = "timeHeadingLabel";
            this.timeHeadingLabel.Size = new System.Drawing.Size(38, 15);
            this.timeHeadingLabel.TabIndex = 9;
            this.timeHeadingLabel.Text = "Time:";
            // 
            // timeLabel
            // 
            this.timeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(480, 445);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(0, 15);
            this.timeLabel.TabIndex = 10;
            // 
            // timeLengthHeadingLabel
            // 
            this.timeLengthHeadingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.timeLengthHeadingLabel.AutoSize = true;
            this.timeLengthHeadingLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.timeLengthHeadingLabel.Location = new System.Drawing.Point(600, 445);
            this.timeLengthHeadingLabel.Name = "timeLengthHeadingLabel";
            this.timeLengthHeadingLabel.Size = new System.Drawing.Size(38, 15);
            this.timeLengthHeadingLabel.TabIndex = 9;
            this.timeLengthHeadingLabel.Text = "Length:";
            // 
            // timeLengthLabel
            // 
            this.timeLengthLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.timeLengthLabel.AutoSize = true;
            this.timeLengthLabel.Location = new System.Drawing.Point(650, 445);
            this.timeLengthLabel.Name = "timeLengthLabel";
            this.timeLengthLabel.Size = new System.Drawing.Size(0, 15);
            this.timeLengthLabel.TabIndex = 10;
            // 
            // statusHeadingLabel
            // 
            this.statusHeadingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statusHeadingLabel.AutoSize = true;
            this.statusHeadingLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.statusHeadingLabel.Location = new System.Drawing.Point(10, 445);
            this.statusHeadingLabel.Name = "statusHeadingLabel";
            this.statusHeadingLabel.Size = new System.Drawing.Size(45, 15);
            this.statusHeadingLabel.TabIndex = 11;
            this.statusHeadingLabel.Text = "Status:";
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(60, 445);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 15);
            this.statusLabel.TabIndex = 12;
            // 
            // mediaHeadingLabel
            // 
            this.mediaHeadingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mediaHeadingLabel.AutoSize = true;
            this.mediaHeadingLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.mediaHeadingLabel.Location = new System.Drawing.Point(150, 445);
            this.mediaHeadingLabel.Name = "mediaHeadingLabel";
            this.mediaHeadingLabel.Size = new System.Drawing.Size(45, 15);
            this.mediaHeadingLabel.TabIndex = 11;
            this.mediaHeadingLabel.Text = "Media:";
            // 
            // mediaLabel
            // 
            this.mediaLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mediaLabel.AutoSize = true;
            this.mediaLabel.Location = new System.Drawing.Point(200, 445);
            this.mediaLabel.Name = "mediaLabel";
            this.mediaLabel.Size = new System.Drawing.Size(0, 15);
            this.mediaLabel.TabIndex = 12;
            // 
            // VideoPlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 538);
            this.Controls.Add(this.mediaLabel);
            this.Controls.Add(this.mediaHeadingLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.statusHeadingLabel);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.timeHeadingLabel);
            this.Controls.Add(this.timeLengthLabel);
            this.Controls.Add(this.timeLengthHeadingLabel);
            this.Controls.Add(this.skipToStartButton);
            this.Controls.Add(this.skipToEndButton);
            this.Controls.Add(this.stepForwardButton);
            this.Controls.Add(this.stepBackButton);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.loadFileButton);
            this.Controls.Add(this.subProcessButton);
            this.Controls.Add(this.videoPlayerPanel);
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
    private Button stepBackButton;
    private Button stepForwardButton;
    private Button skipToEndButton;
    private Button skipToStartButton;
    private Label timeHeadingLabel;
    private Label timeLabel;
    private Label timeLengthHeadingLabel;
    private Label timeLengthLabel;
    private Label statusHeadingLabel;
    private Label statusLabel;
    private Label mediaHeadingLabel;
    private Label mediaLabel;
}
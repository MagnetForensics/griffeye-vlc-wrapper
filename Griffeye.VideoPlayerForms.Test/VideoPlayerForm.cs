using System.Security;
using Griffeye.VideoPlayerContract.Messages.Requests;

namespace Griffeye.VideoPlayerForms.Test;

public partial class VideoPlayerForm : Form
{
    private VideoPlayer videoPlayer = null!;

    public VideoPlayerForm()
    {
        InitializeComponent();
        ChangeButtonsEnabled(false);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        videoPlayer?.Dispose();
        base.Dispose(disposing);
    }

    private void subProcessButton_Click(object sender, EventArgs e)
    {
        if (videoPlayer?.IsRunning ?? false)
        {
            RemoveVideoPlayer();
            return;
        }
        CreateVideoPlayer();
    }

    private void RemoveVideoPlayer()
    {
        ChangeButtonsEnabled(false);
        videoPlayer?.Dispose();
    }

    private async void CreateVideoPlayer()
    {
        ChangeButtonsEnabled(false);
        videoPlayer = new VideoPlayer(videoPlayerPanel.Handle);

        try
        {
            await videoPlayer.Init();

            videoPlayer.Exited += OnVideoPlayerExit;
            videoPlayer.Playing += OnVideoPlayerPlaying;
            videoPlayer.Paused += OnVideoPlayerPaused;

            loadFileButton.Invoke(x => x.Enabled = true);
            subProcessButton.Text = @"Close SubProcess";
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private void OnVideoPlayerPlaying(object? sender, EventArgs e)
    {
        ChangePlayButtons(true);
    }

    private void OnVideoPlayerPaused(object? sender, EventArgs e)
    {
        ChangePlayButtons();
    }

    private void OnVideoPlayerExit(object? sender, EventArgs e)
    {
        ChangeButtonsEnabled(false);
    }

    private void ChangePlayButtons(bool playing = false)
    {
        playButton.Invoke(x => x.Enabled = !playing);
        stopButton.Invoke(x => x.Enabled = playing);
    }

    private void ChangeButtonsEnabled(bool enabled = true)
    {
        subProcessButton.Invoke(x => x.Text = $@"{(enabled ? "Close" : "Open")} SubProcess");
        loadFileButton.Invoke(x => x.Enabled = enabled);
        playButton.Invoke(x => x.Enabled = enabled);
        stopButton.Invoke(x => x.Enabled = enabled);
    }

    private async void loadFileButton_Click(object sender, EventArgs e)
    {
        var fileDialog = new OpenFileDialog()
        {
            Filter = @"All files (*.*)|*.*",
            Title = @"Open video file"
        };

        if (fileDialog.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        try
        {
            var filePath = fileDialog.FileName;
            await videoPlayer.LoadFile(filePath);
            ChangePlayButtons();
        }
        catch (SecurityException ex)
        {
            ShowError(ex.Message);
        }
    }

    private async void playButton_Click(object sender, EventArgs e)
    {
        try
        {
            await videoPlayer.Play();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private async void stopButton_Click(object sender, EventArgs e)
    {
        try
        {
            await videoPlayer.Pause();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private static void ShowError(string message)
    {
        MessageBox.Show(message, @"Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
using System.Globalization;
using System.Security;
using Griffeye.VideoPlayerContract.Messages.Events;

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

    private async void subProcessButton_Click(object sender, EventArgs e)
    {
        if (videoPlayer?.IsRunning ?? false)
        {
            RemoveVideoPlayer();
            return;
        }
        await CreateVideoPlayerAsync();
    }

    private void RemoveVideoPlayer()
    {
        ChangeButtonsEnabled(false);
        videoPlayer?.Dispose();
    }

    private async Task CreateVideoPlayerAsync()
    {
        ChangeButtonsEnabled(false);
        videoPlayer = new VideoPlayer(videoPlayerPanel.Handle);

        try
        {
            await videoPlayer.InitAsync();

            videoPlayer.Exited += OnVideoPlayerExit;
            videoPlayer.Playing += OnVideoPlayerPlaying;
            videoPlayer.Paused += OnVideoPlayerPaused;
            videoPlayer.LengthChanged += OnVideoPlayerLengthChanged;
            videoPlayer.TimeChanged += OnVideoPlayerTimeChanged;
            videoPlayer.MediaTrackChanged += OnVideoPlayerMediaTrackChanged;
            videoPlayer.EndReached += (_, _) => ShowStatus("End Reached");

            loadFileButton.Invoke(x => x.Enabled = true);
            subProcessButton.Text = @"Close SubProcess";
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private void OnVideoPlayerMediaTrackChanged(object? sender, MediaTrackChangedEvent e)
    {
        var mediaStr = "No Media Loaded";
        var track = e.MediaTracks?.FirstOrDefault();
        if (track == null)
        {
            mediaLabel.Invoke(x => x.Text = mediaStr);
            return;
        }
        
        mediaStr = $"{track.Bitrate} kbps, {track.Codec}";
        mediaLabel.Invoke(x => x.Text = mediaStr);
    }

    private void OnVideoPlayerLengthChanged(object? sender, float e)
    {
        timeLengthLabel.Invoke(x => x.Text = e.ToString(CultureInfo.CurrentCulture));
    }

    private void OnVideoPlayerTimeChanged(object? sender, float e)
    {
        timeLabel.Invoke(x => x.Text = e.ToString(CultureInfo.CurrentCulture));
    }

    private void OnVideoPlayerPlaying(object? sender, EventArgs e)
    {
        ChangePlayButtons(true);
    }

    private void OnVideoPlayerPaused(object? sender, EventArgs e)
    {
        ChangePlayButtons(false);
    }

    private void OnVideoPlayerExit(object? sender, EventArgs e)
    {
        ChangeButtonsEnabled(false);
    }

    private void ChangePlayButtons(bool playing)
    {
        ShowStatus(playing ? "Playing" : "Paused");
        playButton.Invoke(x => x.Enabled = true);
        playButton.Invoke(x => x.Text = playing ? "Pause" : "Play");
        skipToEndButton.Invoke(x => x.Enabled = true);
        skipToStartButton.Invoke(x => x.Enabled = true);
        stepBackButton.Invoke(x => x.Enabled = true);
        stepForwardButton.Invoke(x => x.Enabled = true);
    }

    private void ChangeButtonsEnabled(bool enabled = true)
    {
        subProcessButton.Invoke(x => x.Text = $@"{(enabled ? "Close" : "Open")} SubProcess");
        loadFileButton.Invoke(x => x.Enabled = enabled);
        playButton.Invoke(x => x.Enabled = enabled);
        skipToEndButton.Invoke(x => x.Enabled = enabled);
        skipToStartButton.Invoke(x => x.Enabled = enabled);
        stepBackButton.Invoke(x => x.Enabled = enabled);
        stepForwardButton.Invoke(x => x.Enabled = enabled);
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
            await videoPlayer.LoadFileAsync(filePath);
            ChangePlayButtons(false);
            await videoPlayer.PlayAsync();
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
            if (await videoPlayer.IsPlaying)
            {
                await videoPlayer.PauseAsync();
            }
            else
            {
                await videoPlayer.PlayAsync();
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private async void skipToStartButton_Click(object sender, EventArgs e)
    {
        await videoPlayer.SkipToStartAsync();
    }

    private async void skipToEndButton_Click(object sender, EventArgs e)
    {
        await videoPlayer.SkipToEndAsync();
    }

    private async void stepForwardButton_Click(object sender, EventArgs e)
    {
        await videoPlayer.StepForwardAsync();
    }

    private async void stepBackButton_Click(object sender, EventArgs e)
    {
        await videoPlayer.StepBackAsync();
    }

    private void ShowStatus(string status)
    {
        statusLabel.Invoke(x => x.Text = status);
    }
    
    private static void ShowError(string message)
    {
        MessageBox.Show(message, @"Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
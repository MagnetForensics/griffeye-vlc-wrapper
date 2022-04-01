namespace Griffeye.VideoPlayerForms.Test;

public static class WinFormsExtensions
{
    public static void Invoke<TControlType>(this TControlType control, Action<TControlType> del) 
        where TControlType : Control
    {
        if (control.InvokeRequired)
            control.Invoke(() => del(control));
        else
            del(control);
    }
}
namespace Griffeye.VideoPlayerForms.Test;

public static class TaskExtension
{
#pragma warning disable S3168 // "async" methods should not return "void"
    /// <summary>
    /// Fire and forget the <paramref name="task"/> but log any exception not listed as <paramref name="acceptableExceptions"/>
    /// </summary>
    /// <param name="task"></param>
    /// <param name="log"></param>
    /// <param name="acceptableExceptions"></param>
    public static async void ForgetButLogExceptions(this Task task, params Type[] acceptableExceptions)
#pragma warning restore S3168 // "async" methods should not return "void"
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            if (!acceptableExceptions.Contains(ex.GetType()))
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
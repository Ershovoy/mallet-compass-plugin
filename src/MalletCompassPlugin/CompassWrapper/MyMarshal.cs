using System.Runtime.InteropServices;
using System.Security;

namespace CompassWrapper;

/// <summary>
/// Нужен для реализации метода <see cref="GetActiveObject(string)"/>, 
/// который доступен только в .Net Framework.
/// </summary>
public static class MyMarshal
{
    [SecurityCritical]
    public static object GetActiveObject(string progID)
    {
        object? ppunk = null;
        Guid clsid;
        try
        {
            CLSIDFromProgIDEx(progID, out clsid);
        }
        catch (Exception)
        {
            CLSIDFromProgID(progID, out clsid);
        }

        GetActiveObject(ref clsid, IntPtr.Zero, out ppunk);
        return ppunk;
    }

    [DllImport("ole32.dll", PreserveSig = false)]
    [SuppressUnmanagedCodeSecurity]
    [SecurityCritical]
    private static extern void CLSIDFromProgIDEx(
        [MarshalAs(UnmanagedType.LPWStr)] string progId,
        out Guid clsid);

    [DllImport("ole32.dll", PreserveSig = false)]
    [SuppressUnmanagedCodeSecurity]
    [SecurityCritical]
    private static extern void CLSIDFromProgID(
        [MarshalAs(UnmanagedType.LPWStr)] string progId,
        out Guid clsid);

    [DllImport("oleaut32.dll", PreserveSig = false)]
    [SuppressUnmanagedCodeSecurity]
    [SecurityCritical]
    private static extern void GetActiveObject(
        ref Guid rclsid,
        IntPtr reserved,
        [MarshalAs(UnmanagedType.Interface)] out object ppunk);
}

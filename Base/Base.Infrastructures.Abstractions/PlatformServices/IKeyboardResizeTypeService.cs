using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Abstractions.PlatformServices;

/// <summary>
/// This servive mostly used for Android version bellow 14. For Android 15 ApplyPaddingWithKeyboard of LifecyclePage(Native .NET Android project) handles the Pan\Resize
/// This is used to set Activity's WindowSoftInputMode to pan, resize to place the bottom inputs to above the keyboard
/// </summary>
public interface IKeyboardResizeTypeService
{
    void SetResize(string instanceId);
    void SetPan(string instanceId);
    void SetDoNothing(string instanceId);
    bool CanResizeContent(string instanceId);
}

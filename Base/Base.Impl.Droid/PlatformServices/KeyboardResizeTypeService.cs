using Base.Abstractions.PlatformServices;

namespace Base.Impl.PlatformServices;
public class KeyboardResizeTypeService : IKeyboardResizeTypeService
{
    private Dictionary<string, bool> pageInstances = new Dictionary<string, bool>();

    public void SetPan(string pageId)
    {
        this.SetCanResize(pageId, false);
    }

    public void SetDoNothing(string pageId)
    {
        this.SetCanResize(pageId, false);
    }

    public void SetResize(string pageId)
    {
        this.SetCanResize(pageId, true);
    }

    public bool CanResizeContent(string pageId)
    {
        var isResizeable = false;
        if (pageInstances.ContainsKey(pageId))
        {
            isResizeable = pageInstances[pageId];
        }

        return isResizeable;
    }

    private void SetCanResize(string pageId, bool canResize)
    {
        // set chat page instance can resize
        if (pageInstances.ContainsKey(pageId))
        {
            pageInstances[pageId] = canResize;
        }
        else
        {
            pageInstances.Add(pageId, canResize);
        }
    }
}

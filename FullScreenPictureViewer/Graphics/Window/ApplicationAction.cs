namespace FullScreenPictureViewer.Graphics.Window
{
    public enum ApplicationActionType
    {
        PreviousImage,
        NextImage
    }

    public class ApplicationAction
    {
        public string DisplayName { get; }
        public ApplicationActionType Id { get; }

        public ApplicationAction(string name, ApplicationActionType id)
        {
            DisplayName = name;
            Id = id;
        }
    }
}
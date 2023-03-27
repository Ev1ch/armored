namespace Players.Abstracts
{
    public interface IEntityInputSource
    {
        public float HorizontalDirection { get; }
        public float VerticalDirection { get; }

        public bool Jump { get; }
        public bool Attack { get; }

        public void ResetOneTimeActions();
    }
}

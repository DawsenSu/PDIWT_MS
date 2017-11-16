namespace PDIWT_MS_PiledWharf
{
    class KeyinCommands
    {
        public static void Command(string unparsed)
        {
            Views.MainView.ShowWindow();
        }

        public static void DrawPileAxis(string unparsed)
        {
            Views.DrawPileAxisView.ShowWindow();
        }
    }
}

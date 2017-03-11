namespace Assets.Scripts
{
    public static class UI
    {
        public static string ColorizeText(string text, string color)
        {
            return "<color=" + color + ">" + text + "</color>";
        }
    }
}

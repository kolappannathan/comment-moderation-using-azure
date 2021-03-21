using System;

namespace Comment_Moderation
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Enter the text to be moderated: ");
                var text = Console.ReadLine();
                var moderation = new ModerationLib();
                moderation.ModerateText(text);
                Console.WriteLine("");
            } while (true);
        }
    }
}

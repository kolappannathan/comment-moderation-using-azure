using System;
using System.Collections.Generic;

namespace Comment_Moderation
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("1: Manage Terms List");
                Console.WriteLine("2: Moderate Text");
                Console.WriteLine("Enter your choice: ");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        HandleListManagement();
                        break;
                    case "2":
                        HandleModerateText();
                        break;
                    default:
                        Console.WriteLine("Invalid Choice");
                        break;
                };
                Console.WriteLine("");
                Console.ReadLine();
            } while (true);
        }

        public static void HandleModerateText()
        {
            Console.WriteLine("Is there a moderation list: Y/N");
            var choice = Console.ReadKey().KeyChar;
            string listId = null;
            Console.ReadLine();
            if (choice == 'Y' || choice == 'y')
            {
                Console.WriteLine("Enter List Id");
                listId = Console.ReadLine();
            }
            Console.WriteLine("Enter the text to be moderated: ");
            var text = Console.ReadLine();
            var moderationLib = new ModerationLib();
            moderationLib.ModerateText(text, listId);
        }

        #region List Management

        public static void HandleListManagement()
        {
            Console.WriteLine("1: Create Terms List");
            Console.WriteLine("2: Add terms to a Term List");
            Console.WriteLine("3: List all Terms in a List");
            Console.WriteLine("4: Remove a term from List");
            Console.WriteLine("5: Empty a List");
            Console.WriteLine("6: Delete a List");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    CreateTermList();
                    break;
                case "2":
                    AddTermsToList();
                    break;
                case "3":
                    ListAllTerms();
                    break;
                case "4":
                    RemoveOneTerm();
                    break;
                case "5":
                    RemoveAllTerms();
                    break;
                case "6":
                    DeleteList();
                    break;
                default:
                    Console.WriteLine("Invalid Choice");
                    break;
            };
        }

        public static void CreateTermList()
        {
            Console.WriteLine("Enter list name");
            var listName = Console.ReadLine();
            Console.WriteLine("Enter list description");
            var listDesc = Console.ReadLine();
            var moderationLib = new ModerationLib();
            var listId = moderationLib.CreateTermList(listName, listDesc);
            Console.WriteLine($"List created with id:{listId}");
        }

        public static void AddTermsToList()
        {
            Console.WriteLine("Enter number of terms to add");
            var no_of_terms = Convert.ToInt32(Console.ReadLine());
            List<string> terms_List = new List<string>();
            Console.WriteLine("Enter the terms");
            for (var i=0; i<no_of_terms; i++)
            {
                terms_List.Add(Console.ReadLine());
            }
            Console.WriteLine("Enter List Id");
            var listId = Console.ReadLine();
            var moderationLib = new ModerationLib();
            moderationLib.AddToTermsList(listId, terms_List);
            Console.WriteLine("Terms added successfully");
        }

        public static void ListAllTerms()
        {
            Console.WriteLine("Enter List Id");
            var listId = Console.ReadLine();
            var moderationLib = new ModerationLib();
            var terms = moderationLib.GetAllTerms(listId);
            var csv = string.Join(",", terms);
            Console.WriteLine($"Terms are: {csv}");
        }

        public static void RemoveOneTerm()
        {
            Console.WriteLine("Enter List Id");
            var listId = Console.ReadLine();
            Console.WriteLine("Enter the term to be removed");
            var term = Console.ReadLine();
            var moderationLib = new ModerationLib();
            moderationLib.DeleteTerm(listId, term);
            Console.WriteLine("Term is removed");
        }

        public static void RemoveAllTerms()
        {
            Console.WriteLine("Enter List Id");
            var listId = Console.ReadLine();
            var moderationLib = new ModerationLib();
            moderationLib.DeleteAllTerms(listId);
            Console.WriteLine("All terms are removed from the list");
        }

        public static void DeleteList()
        {
            Console.WriteLine("Enter List Id");
            var listId = Console.ReadLine();
            var moderationLib = new ModerationLib();
            moderationLib.DeleteTermList(listId);
            Console.WriteLine("Terms list deleted");
        }

        #endregion List Management
    }
}
